using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class NewSpan : Span {
        public NewSpan(Span span)
            : base(span) { }

        public NewSpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }

        public override void Init() {
            this.GetTagNotNull();
            this.ValidVar();
            this.ValidChar('(');
            switch (this._Tag) {
                case "Array":
                    this.SetArray();
                    break;
                default:
                    NewClass();
                    break;
            }
        }

        private void NewClass() {
            var exists = this.ValidateNameExists(this._Tag);
            var newE = new JNewEleme();
            if (exists == null)
                this.Error(this._Tag + " 未定义");
            newE.Name = exists.GetAliasName();
            this.AddFather(newE);
            this.SetUpPoint();
            this.GetChar();
            if (this._Char == ')')
                return;
            this.ResetUpPoint();

            if (newE.Arguments == null)
                newE.Arguments = new List<ValueEleme>();

        Loop:
            var valueE = new ValueEleme();
            valueE.Father = this.NEleme;
            var valueS = new ValueSpan(this.PBParser, valueE);
            valueS.Init();
            newE.Arguments.Add(valueE);
            this.GetChar();
            switch (this._Char) {
                case ',':
                    goto Loop;
                case ')':
                    break;
                default:
                    this.Error();
                    break;
            }
        }

        /// <summary>
        /// 设置数组
        /// </summary>
        private void SetArray() {

            var arrayE = new JArrayEleme();
            this.AddFather(arrayE);
            var aSpan = new ArraySpan(this.PBParser, arrayE);
            aSpan.ArrayEndChar = ')';
            aSpan.Init();
        }

    }
}
