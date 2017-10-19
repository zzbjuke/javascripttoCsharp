using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class WhileSpan : Span {
        #region 构造
        public WhileSpan(Span span)
            : base(span) { }

        public WhileSpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }

        #endregion

        public override void Init() {
            var whileE = new JWhileEleme();
            this.AddFather(whileE);
            this.ValidChar('(');
            whileE.Where = new ValueEleme();
            whileE.Where.Father = this.NEleme;
            var vSpan = new ValueSpan(this.PBParser, whileE.Where);
            vSpan.Init();
            this.ValidChar(')');
            this.ValidChar('{');
            new JavaSciptSpan(this.PBParser, whileE).Init();
            this.ValidChar('}');

        }
    }
}
