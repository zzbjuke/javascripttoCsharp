using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class ValueSpan : Span {
        #region 构造
        public ValueSpan(Span span)
            : base(span) {
        }

        public ValueSpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }
        #endregion
                
        public bool Continuous = true;

        private static readonly HashSet<char> AfterChar = new HashSet<char>(){
            '+','-','*','/','%','!','=','|','&','.','[','>','<',';'
        };

        public void SetValueEleme(ValueEleme valueEleme) {
            this.pValueEleme = valueEleme;
        }

        private ValueEleme pValueEleme;
        //private bool isVar = false;
        public override void Init() {
            if (pValueEleme == null) {
                this.pValueEleme = new JValueEleme();
                this.AddFather(this.pValueEleme);
            }           
            string oper = string.Empty;
        Start:
            this.GetChar();
            switch (this._Char) {
                case '(':
                    this.AutoFun();
                    break;
                case '[':
                    this.Array();
                    break;
                case '{':
                    this.Dictionary();
                    break;
                case '!':
                case '~':
                #region 加减
                case '+':
                case '-':
                    oper += this._Char.ToString();
                    goto Start;
                #endregion
                #region 数字
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    this.Number(oper);
                    break;
                #endregion
                #region 字符串
                case '\'':
                case '"':
                    this._String(oper);
                    break;
                #endregion
                default:
                    Var(oper);
                    break;
            }
            this.End();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AutoFun() {
            this.GetTag();
            if (this._Tag != "function")
                this.Error();
            var fSpan = new FunctionSpan(this);
            fSpan.IsValidName = false;
            fSpan.Init();
            this.ValidChar(')');

            this.SetUpPoint();
            this.GetChar();
            this.ResetUpPoint();
            if (this._Char == '(') {
                var varElem = new JVariableEleme();
                this.AddFather(varElem);

                var varSpan = new VariableSpan(this.PBParser, varElem);
                varSpan.Init();
            }


        }

        /// <summary>
        /// 数组
        /// </summary>
        private void Array() {
            var arrayE = new JArrayEleme();
            this.AddFather(this.pValueEleme, arrayE);
            new ArraySpan(this.PBParser, arrayE).Init();
        }

        /// <summary>
        /// JSON
        /// </summary>
        private void Dictionary() {
            var dictE = new JDictionaryEleme();
            this.AddFather(this.pValueEleme, dictE);
            new DictionarySpan(this.PBParser, dictE).Init();
        }

        /// <summary>
        /// 字符串
        /// </summary>
        private void _String(string oper) {
            if (!string.IsNullOrEmpty(oper))
                this.Error();
            var tag = this.GetString(this._Char);
            var consE = new JConstEleme(tag, EValueType.String);
            this.AddFather(this.pValueEleme, consE);
        }

        /// <summary>
        /// 数字
        /// </summary>
        private void Number(string oper) {
            if (!string.IsNullOrEmpty(oper) && !(oper == "-" || oper == "~"))
                this.Error();
            var value = oper + this.GetIntString();
            var consE = new JConstEleme(value, EValueType.Number);
            var noper = false;
            this.SetUpPoint();
            this._Char = this.PBParser.GetNextChar(false);
            if (this.IsAddOrReduce) {
                var uChar = this.PBParser.GetNextChar(false);
                if (this._Char == uChar) {                   
                    value += value + uChar + uChar;
                    noper = true;
                }
            }
            if (!noper)
                this.ResetUpPoint();


            this.AddFather(this.pValueEleme, consE);
        }

        /// <summary>
        /// 变量
        /// </summary>
        /// <param name="oper"></param>
        private void Var(string oper) {
            if (!Tools.HasVarHead(this._Char))
                this.Error();
            this.GetTagForBack(false);
            switch (this._Tag) {
                case "true":
                case "false":
                    this.AddFather(pValueEleme, new JConstEleme(this._Tag, EValueType.Bool));
                    break;
                case "undefined":
                    this.AddFather(pValueEleme, new JConstEleme(this._Tag, EValueType.Undefined));
                    break;
                case "null":
                    this.AddFather(pValueEleme, new JConstEleme(this._Tag, EValueType.Null));
                    break;
                //case "window":
                //    this.AddFather(pValueEleme, new JConstEleme(this._Tag, EValueType.Const));
                //break;
                case "new":
                    new NewSpan(this).Init();
                    break;
                case "typeof":
                    new TypeofInstanceofSpan(this.PBParser, pValueEleme).Init();
                    break;
                case "function":
                    var fSpan = new FunctionSpan(this.PBParser, this.pValueEleme);
                    fSpan.IsValidName = false;
                    fSpan.Init();
                    break;
                case "arguments":
                    Arguments(oper);
                    break;
                case "this":
                    this.This(oper);
                    break;
                default:
                    Default(oper);
                    break;


            }
        }
        private void This(string oper) {
            if (!string.IsNullOrEmpty(oper))
                this.Error();

            var funElem = this.NEleme.GetFather<JFunEleme>();
            if (funElem == null)
                this.Error();

            this._Tag = funElem.GetThisName;
            Default(oper, false);
        }

        private void Arguments(string oper) {
            if (!string.IsNullOrEmpty(oper))
                this.Error();

            var funElem = this.NEleme.GetFather<JFunEleme>();
            if (funElem == null)
                this.Error();
            this._Tag = funElem.GetAraumerName;
            funElem.SetArgEleme();
            Default(oper, false);
        }

        private void Default(string oper, bool validTag = true) {
            if (validTag) {
                var varName = this.ValidateNameExists(this._Tag);
                if (varName == null)
                    this.Error(this._Tag + " 未定义");
                this._Tag = varName.GetAliasName();
            }
            var varElem = new JVariableEleme();
            varElem.IsStart = true;
            varElem.Name = this._Tag;
            if (!string.IsNullOrEmpty(oper)) {
                if (!this.VaildatePChar(oper))
                    this.Error();
                varElem.Characteristic = new CharacteristicInfo(EBAType.Before, oper);
            }
            this.AddFather(this.pValueEleme, varElem);
            var variableSpan = new VariableSpan(this.PBParser, varElem);
            variableSpan.Init();
        }

        private void End() {
            if (!Continuous)
                return;
            this.SetUpPoint();
            this.GetChar();
            char fChar = this._Char;
            if (AfterChar.Contains(this._Char)) {
                switch (this._Char) {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '%':
                        if (this.IsAddOrReduce) {
                            var point = new CPoint(this.PBParser.Point);
                            var nchar = this.PBParser.GetNextChar(false);
                            if (nchar == this._Char) {
                                this.ResetUpPoint();
                                break;
                            }

                            this.PBParser.Point.X = point.X;
                            this.PBParser.Point.Y = point.Y;

                        }   
                        this.AddFather(this.pValueEleme, new OperatorEleme(this._Char));
                        this.Init();

                        break;
                    case '>':
                    case '<':
                        this.GetChar(false, null);
                        if (this._Char == '=')
                            this.AddFather(this.pValueEleme, new OperatorEleme(fChar, this._Char));
                        else {
                            this.AddFather(this.pValueEleme, new OperatorEleme(fChar));
                            this.BackPoinX();
                        }
                        this.Init();
                        break;
                    case '=':
                    case '!':
                        this.GetChar(false, null);
                        if (this._Char == '=')
                            this.AddFather(this.pValueEleme, new OperatorEleme(fChar + "="));
                        else
                            this.Error();
                        this.Init();
                        break;
                    case '|':
                    case '&':
                        this.GetChar(false, null);
                        if (this._Char == fChar)
                            this.AddFather(this.pValueEleme, new OperatorEleme(fChar, fChar));
                        else {
                            this.AddFather(this.pValueEleme, new OperatorEleme(fChar));
                            this.BackPoinX();
                        }
                        this.Init();
                        break;
                    case ';':
                        break;
                }
            }
            else
                this.ResetUpPoint();
        }
    }
}
