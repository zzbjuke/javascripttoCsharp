using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JCatchEleme : BEleme, IHChild {
        public ValueEleme Value { get; set; }
        public override string ToCode() {
            return string.Empty;
        }

       
    }
}
