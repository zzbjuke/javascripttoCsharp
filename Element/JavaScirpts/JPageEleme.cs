using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JPageEleme : RangeEleme {

        public SortedDictionary<string, BEleme> IncludeVar { get; set; }

        public override string ToCode() {
            StringBuilder sbCode = new StringBuilder();
            this.ElemeAllToString(sbCode);
            return sbCode.ToString();
        }
    }
}
