using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class TypeofInstanceofSpan : Span{
        public TypeofInstanceofSpan(Span span)
            : base(span) { }
        public TypeofInstanceofSpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }
        public override void Init() {
            this.SetUpPoint();
            this.GetChar();
            bool isC = false;
            if (this._Char == '(')
                isC = true;
            else
                this.ResetUpPoint();

            var typeE = new JTypeofEleme();
            this.AddFather(typeE);
            var vSpan = new ValueSpan(this.PBParser, typeE);
            vSpan.Continuous = false;
            vSpan.Init();
            if (isC)
                this.ValidChar(')');
        }

    }
}
