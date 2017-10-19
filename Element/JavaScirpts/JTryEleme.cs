using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JTryEleme : BEleme {
        public JCatchEleme CatchValue { get; set; }
        public BEleme FinallyValue { get; set; }
        public override string ToCode() {
            var sbCode = new StringBuilder();
            sbCode.AppendLine("try{");
            this.ElemeAllToString(sbCode);
            sbCode.Append("}catch");
            if (this.CatchValue.Name != null)
                sbCode.Append("(Exception " + this.CatchValue.Name + ")");
            
            sbCode.AppendLine("{");
            this.CatchValue.ElemeAllToString(sbCode);
            sbCode.AppendLine("}");
            if (this.FinallyValue != null) {
                sbCode.AppendLine("finally{");
                this.FinallyValue.ElemeAllToString(sbCode);
                sbCode.AppendLine("}");
            }
            return sbCode.ToString();
        }
    }
}
