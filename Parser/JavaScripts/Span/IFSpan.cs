using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class IFSpan : Span {
        public IFSpan(Span span)
            : base(span) { }

        public IFSpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }

        private JIFEleme pifEleme;

        public override void Init() {
            this.pifEleme = this.NEleme as JIFEleme;
            var isWhere = true;
            if (this.NEleme.Name == "else") {
                this.SetUpPoint();
                this.GetChar();                
                if (this._Char == '{') {
                    isWhere = false;
                    this.ResetUpPoint();
                }
                else if (this._Char == 'i') {
                    this.GetTagForBack();
                    this.NEleme.Name = "else if";
                    if (this._Tag != "if")
                        this.Error();
                }
                else
                    this.Error();
            }

            if (isWhere)
                this.Where();


            this.ValidChar('{');

            new JavaSciptSpan(this.PBParser, pifEleme).Init();

            this.ValidChar('}');
            //this.AddFather(new MarkElem(";\r\n"));
        }
        /// <summary>
        /// 条件部分
        /// </summary>
        private void Where() {
            this.ValidChar('(');
            var w = new ValueEleme();
            w.Father = this.NEleme;
            this.pifEleme.Where = w;
            var vSpan = new ValueSpan(this.PBParser, w);
            vSpan.Init();
            this.ValidChar(')');
        }
    }
}
