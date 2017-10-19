using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JCaseEleme : BEleme, IBreak {
        public ValueEleme Value { get; set; }
    }
}
