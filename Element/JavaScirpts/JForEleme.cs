using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JForEleme : JWhereEleme, IHChild, IBreak {
        //public BEleme Statement1 { get; set; }
        public BEleme Statement2 { get; set; }

        public override string ToCode() {
            var sbCode = new StringBuilder();
            // this.Statement1.ElemeAllToString(sbCode);
            sbCode.Append("for(;");
            this.Statement2.ElemeAllToString(sbCode);
            sbCode.Append(";");
            this.Where.ElemeAllToString(sbCode);
            sbCode.AppendLine("){");
            this.ElemeAllToString(sbCode);
            sbCode.AppendLine("}");
            return sbCode.ToString();
        }
    }
}
