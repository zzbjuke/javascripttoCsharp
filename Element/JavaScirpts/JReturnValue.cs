using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JReturnValue : BEleme {
        public BEleme Value { get; set; }
        public string Sing { get; set; }
        public override string ToCode() {
            var sbCode = new StringBuilder();
            if (this.Value == null)
                sbCode.AppendLine("return;");
            else {
                sbCode.AppendFormat("__result{0}__.Value = (", this.Sing);
                this.Value.ElemeAllToString(sbCode);
                sbCode.AppendLine(").Value;return;");
            }
            return sbCode.ToString();
        }
    }
}
