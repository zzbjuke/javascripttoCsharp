using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class JavaSciptSpan : Span {
        #region 构造
        public JavaSciptSpan(Span span)
            : base(span) {
        }

        public JavaSciptSpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }
        #endregion

        /// <summary>
        /// 结束字符串
        /// </summary>
        public bool IsCase { get; set; }

        public override void Init() {
        Start:
            this.SetUpPoint();
            this.GetChar();
            switch (this._Char) {
                case Tools.AllEndChar:
                    return;
                case ';':
                    goto Start;
                case '}':
                    this.ResetUpPoint();
                    return;
                case '+':
                case '-':
                    this.AddOrReduce();
                    goto Start;
                case '(':
                    SetAutoFun();
                    goto Start;
            }

            this.GetTagForBack(false);
            switch (this._Tag) {
                case null:
                    this.Error();
                    break;
                case "var":
                    new VarSpan(this).Init();
                    break;
                case "function":
                    new FunctionSpan(this).Init();
                    break;
                case "return":
                    this.Return();
                    break;
                case "else":
                case "if":
                    var ife = new JIFEleme();
                    ife.Name = this._Tag;
                    this.AddFather(ife);
                    var ifs = new IFSpan(this.PBParser, ife);
                    ifs.Init();
                    break;
                case "for":
                    new ForSpan(this).Init();
                    break;
                case "while":
                    new WhileSpan(this).Init();
                    break;
                case "switch":
                case "case":
                case "default":
                    if (this.IsCase && this._Tag != "switch") {
                        this.BackPoinX(this._Tag.Length);
                        return;
                    }
                    new SwitchSpan(this).Init(this._Tag);
                    break;
                case "break":
                    Break();
                    break;
                case "try":
                    new TrySpan(this).Init();
                    break;
                case "throw":
                    new ThrowSpan(this).Init();
                    break;
                case "this":
                    this.This();
                    break;
                case "__include__":
                    this.SetInclude();
                    break;
                case "__start__":
                    this.SetInclude(true);
                    break;
                default:
                    this.SetValue();
                    break;
            }
            this.ClearLineAnnotation();

            Init();
        }

        private void SetValue() {
            var e = this.ValidateNameExists(this._Tag);
            this.SetUpPoint();
            this.GetChar();
            switch (this._Char) {
                case '=':
                    this.ResetUpPoint();
                    var vSpan = new VarSpan(this);
                    if (e != null) {
                        vSpan.VarName = e.GetAliasName();
                        vSpan.IsDef = true;
                    }
                    else
                        vSpan.VarName = this._Tag;
                    vSpan.Init();
                    return;
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                    char fChar = this._Char;
                    var point = new CPoint(this.PBParser.Point);
                    this.GetChar();
                    if (this._Char != '=') {
                        if (this.IsAddOrReduce) {
                            this.ResetUpPoint();
                            break;
                        }
                        this.Error();
                    }
                    if (e == null)
                        this.Error(this._Tag + " 未定义");
                    this.PBParser.Point.X = point.X;
                    this.PBParser.Point.Y = point.Y;
                    vSpan = new VarSpan(this);
                    vSpan.VarName = e.GetAliasName();
                    vSpan.IsDef = true;
                    vSpan.OperationChar = fChar.ToString();
                    vSpan.Init();
                    return;
            }
            if (e != null) {
                this.ResetUpPoint();
                var varElem = new JVariableEleme();
                varElem.Name = e.GetAliasName();
                this.AddFather(varElem);
                var variableSpan = new VariableSpan(this.PBParser, varElem);
                variableSpan.IsSet = true;
                variableSpan.Init();
                this.AddFather(new MarkElem(";\r\n"));
            }
            else
                this.Error(this._Tag + " 未定义");
        }

        private void This() {
            this.SetUpPoint();
            this.ValidChar('.');
            var funElem = this.NEleme.GetFather<JFunEleme>();
            if (funElem == null)
                this.Error();

            //this.ResetUpPoint();

            //this.SetUpPoint();
            this.GetTagNotNull();
            this.GetChar();
            if (this._Char == '=') {
                var funPro = new JDefinitionEleme();
                funPro.Name = this._Tag;
                this.AnalysisAnnotation(funPro);
                funElem.AddPropertyValue(this._Tag, funPro);
            }
            this.ResetUpPoint();
            var varElem = new JVariableEleme();
            varElem.Name = funElem.GetThisName;
            this.AddFather(varElem);
            var variableSpan = new VariableSpan(this.PBParser, varElem);
            variableSpan.IsSet = true;
            variableSpan.Init();
            this.AddFather(new MarkElem(";\r\n"));

        }

        private void Return() {
            this.SetUpPoint();
            this.GetChar();
            var retE = new JReturnValue();
            this.AddFather(retE);
            if (this._Char == ';' || this._Char == '}' || this._Char == Tools.AllEndChar) {
                if (this._Char != ';')
                    this.ResetUpPoint();

                return;
            }
            this.ResetUpPoint();
            var fatherFun = retE.GetFather<JFunEleme>();
            if (fatherFun == null)
                this.Error("未知错误");

            retE.Sing = fatherFun.Sign;

            var vE = new JValueEleme();
            vE.Father = this.NEleme;
            retE.Value = vE;

            var valueSpane = new ValueSpan(this.PBParser, vE);
            valueSpane.Init();


        }

        private void Break() {
            var f = this.NEleme.GetFather<IBreak>();
            if (f == null)
                this.Error("非法的语句 break 所在的位置无效");

            if (f is JCaseEleme) {
                var swE = (f as JCaseEleme).Father;
                if (!(swE is JSwitchEleme))
                    this.Error();
                this.AddFather(new MarkElem("goto " + (swE as JSwitchEleme).EndGoToTagName + ";"));
            }
            else
                this.AddFather(new MarkElem("break;\r\n"));
        }

        private void SetAutoFun() {
            var valueSpan = new ValueSpan(this);
            valueSpan.AutoFun();
            this.AddFather(new MarkElem(";\r\n"));
        }

        private void AddOrReduce() {
            var fchar = this._Char;
            this.GetChar();
            if (this._Char != fchar)
                this.Error();
            this.ResetUpPoint();
            var line = new JLineEleme();
            this.AddFather(line);
            var valueSpan = new ValueSpan(this.PBParser, line);
            valueSpan.Init();

        }

        /// <summary>
        /// 设置包含
        /// <param name="isStart">是否是启动文件</param>
        /// </summary>
        private void SetInclude(bool isStart = false) {
            this.ValidChar('(');
            this.GetChar();
            if (this._Char != '\'' && this._Char != '"')
                this.Error();

            string url = this.GetString(this._Char);
            if (url == "")
                return;

            url = this.GetValidUrl(url, this.Error);
            if (!isStart && !this.PBParser.LumpElem.Childs.IsNull())
                this.Error("__include__ 必须定义在文件开头");

            var fele = this.PBParser.FileAnalysis(url);

            if (fele == null)
                if (this.PBParser.AnalysisFileAction != null)
                    this.PBParser.AnalysisFileAction(url);

            fele = this.PBParser.FileAnalysis(url);
            if (fele == null || !(fele is RangeEleme))
                this.Error("文件不存在：" + url);

            if (isStart) {
                this.AddFather(new MarkElem("\r\nnew " + url.GetFileNameForCs(".js") + "();\r\n"));
                if (this.PBParser.FilePath.ToLower() != Path.GetFullPath(Path.Combine(this.PBParser.RootPath.ToLower(), "main.js")))
                    this.Error("__start__() 函数只能出现在起始页中");
            }
            else {
                this.AddIncludeFile(url);
                var rangeE = fele as RangeEleme;
                if (rangeE.Variables != null)
                    foreach (var v in rangeE.Variables)
                        this.AddIncludeVar(v.Key, v.Value, url.GetFileNameForCs(".js"));
            }
            this.ValidChar(')');
        }
    }
}
