using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class VariableSpan : Span {
        #region 构造
        public VariableSpan(Span span)
            : base(span) {
        }

        public VariableSpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }
        #endregion

        /// <summary>
        /// 设置一个值 是否允许赋值
        /// </summary>
        public bool IsSet { get; set; }

        private IVariableAttr pVariableAttr;

        public override void Init() {
            if (!(this.NEleme is IVariableAttr))
                this.Error("NEleme 不是有效的 IVariableAttr");
            pVariableAttr = this.NEleme as IVariableAttr;

            this.SetUpPoint();
            this.GetChar();
            switch (this._Char) {
                case '.':
                    this.Property();
                    break;
                case '[':
                    this.Index();
                    break;
                case '(':
                    this.CallOwn();
                    break;
                case ';':
                    break;
                case '+':
                case '-':
                    this.Increment(this._Char);
                    break;
                default:
                    this.ResetUpPoint();
                    break;
            }
        }

        /// <summary>
        /// 属性
        /// </summary>
        private void Property() {
            this.GetTagNotNull();
            this.SetUpPoint();
            this.GetChar();
            switch (this._Char) {
                case '(':
                    this.Method();
                    return;
                case '=':
                    if (this.IsSet) {
                        this.SetValue();
                        return;
                    }
                    else {
                        //this.Error();
                        this.ResetUpPoint();
                    }
                    break;

            }
            this.ResetUpPoint();
            var varElem = new JPropertyEleme();
            varElem.Name = this._Tag;
            varElem.Father = this.NEleme;
            this.pVariableAttr.Property = varElem;
            this.End(EPType.Property, varElem);
        }

        private void SetValue() {
            var varElem = new JPropertyEleme();
            varElem.IsSetValue = true;
            varElem.Name = this._Tag;
            varElem.Father = this.NEleme;
            this.pVariableAttr.Property = varElem;

            var valueE = new ValueEleme();
            valueE.Father = this.NEleme;
            varElem.Value = valueE;
            var valueSpan = new ValueSpan(this.PBParser, valueE);
            valueSpan.Init();
        }

        private void Method() {
            this.SetUpPoint();
            this.GetChar();
            var varElem = new JMethodEleme();
            varElem.Name = this._Tag;
            varElem.Father = this.NEleme;
            this.pVariableAttr.Method = varElem;
            if (this._Char != ')') {

                this.ResetUpPoint();
            Start:
                var noElem = new NoEleme();
                noElem.Father = this.NEleme;
                var valueSpan = new ValueSpan(this.PBParser, noElem);
                valueSpan.Init();
                if (varElem.Arguments == null)
                    varElem.Arguments = new List<BEleme>();
                varElem.Arguments.Add(noElem.Childs[0]);

                this.GetChar();
                switch (this._Char) {
                    case ',':
                        goto Start;
                    case ')':
                        break;
                    default:
                        this.Error();
                        break;

                }
            }
            this.End(EPType.Method, varElem);
        }

        private void CallOwn() {
            this.Method();
            this.pVariableAttr.Method.Name = "__C__" + (this.NEleme.Name ?? this.PBParser.Point.ToString());
        }

        /// <summary>
        /// 索引
        /// </summary>
        private void Index() {
        Start:
            this.SetUpPoint();
            this.GetChar();
            BEleme elem = null;
            if (Tools.StrStartChars.Contains(this._Char))
                elem = new JConstEleme("\"" + this.GetString(this._Char) + "\"", EValueType.Const);
            else if (char.IsNumber(this._Char))
                elem = new JConstEleme(this.GetIntString(), EValueType.Const);
            else {
                this.ResetUpPoint();
                elem = new JValueEleme();
                elem.Father = this.NEleme;
                var valueS = new ValueSpan(this.PBParser, elem);
                valueS.Init();
            }
            this.GetChar();
            switch (this._Char) {
                case ',':
                    goto Start;
                case ']':
                    var jIndex = new JIndexEleme();
                    jIndex.IndexEleme = elem;
                    elem.Father = jIndex;
                    this.pVariableAttr.Index = jIndex;
                    jIndex.Father = this.NEleme;
                    this.End(EPType.Index, jIndex);
                    break;
                default:
                    this.Error();
                    break;
            }

        }

        private void End(EPType p, BEleme e) {
            this.SetUpPoint();
            this.GetChar();
            switch (this._Char) {
                case '+':
                case '-':
                case '.':
                case '[':
                case '(':
                    if (this._Char == '+' || this._Char == '-') {
                        var c = this._Char;
                        var point = new CPoint(this.UpPoint);
                        this.GetChar(false, null);
                        if (this._Char != c) {
                            this.PBParser.Point.X = point.X;
                            this.PBParser.Point.Y = point.Y;
                            break;
                        }

                        if (p == EPType.Method)
                            this.Error();

                        IVariableAttr att = null;
                        BEleme father = this.NEleme.Father;
                        while (father is IVariableValue) {
                            father = father.Father;
                            if (father is JVariableEleme) {
                                if ((father as JVariableEleme).IsStart) {
                                    att = father as JVariableEleme;
                                    break;
                                }
                                father = father.Father;
                            }
                        }
                        SetCharacteristic(c, att);
                        break;
                    }

                    this.ResetUpPoint();
                    var varElem = new JVariableEleme();
                    this.AddFather(e, varElem);
                    var variableSpan = new VariableSpan(this.PBParser, varElem);
                    variableSpan.Init();
                    break;
                case ';':
                    break;
                default:
                    this.ResetUpPoint();
                    break;
            }
        }

        private void Increment(char c) {
            if (this.UpPoint.X + 1 != this.PBParser.Point.X) {
                this.ResetUpPoint();
                return;
            }
            var point = new CPoint(this.UpPoint);
            this.GetChar(false, null);
            if (this._Char != c) {
                this.PBParser.Point.X = point.X;
                this.PBParser.Point.Y = point.Y;
                return;
            }
            SetCharacteristic(c);
        }

        private void SetCharacteristic(char c, IVariableAttr attr = null) {
            var cha = new CharacteristicInfo(EBAType.After, c.ToString() + c);
            if (attr == null)
                attr = this.pVariableAttr;
            attr.Characteristic = cha;
        }

    }
}
