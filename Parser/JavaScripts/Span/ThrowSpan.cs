using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Entity;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class ThrowSpan : Span {
        #region 构造
        public ThrowSpan(Span span)
            : base(span) { }

        #endregion

        public override void Init() {
            this.SetUpPoint();
            this.GetChar();
            bool isC = false;
            if (this._Char == '(') 
                isC = true;     
            else
                this.ResetUpPoint();
            
            var tE = new JThrowEleme();
            tE.Point = new CPoint(this.PBParser.Point);
            this.AddFather(tE);
            var vSpan = new ValueSpan(this.PBParser, tE);
            vSpan.Init();
            if (isC)
                this.ValidChar(')');
        }
    }
}
