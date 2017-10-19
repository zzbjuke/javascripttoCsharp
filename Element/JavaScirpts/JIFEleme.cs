using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JIFEleme : JWhereEleme, IHChild {

        public override string ToCode() {
            var sbCode = new StringBuilder();
            sbCode.Append(this.Name);
            if (this.Name != "else") {
                sbCode.Append("(");
                this.Where.ElemeAllToString(sbCode);
                sbCode.Append(")");
            }
            sbCode.AppendLine("{");
            this.ElemeAllToString(sbCode);
            sbCode.AppendLine("}");


            return sbCode.ToString();
        }
    }
}
