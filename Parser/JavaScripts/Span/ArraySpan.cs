using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.interfaces;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class ArraySpan : Span {
        #region 构造
        public ArraySpan(Span span)
            : base(span) { }

        public ArraySpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }

        #endregion

        public char ArrayEndChar = ']';
        

        public override void Init() {
            this.SetUpPoint();
            this.GetChar();
            if (this._Char == this.ArrayEndChar)
                return;

            this.ResetUpPoint();

            Start:

            var valueE = new JValueEleme();
            this.AddFather(valueE);
            var valueSpan = new ValueSpan(this.PBParser, valueE);
            valueSpan.Init();

            this.GetChar();
            switch (this._Char) { 
                case ',':
                    goto Start;
                default:
                    if (this._Char == ArrayEndChar)
                        break;
                    this.Error();
                    break;
            }
        }
    }
}
