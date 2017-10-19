using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JForeachEleme : JWhereEleme, IHChild {
        public override string ToCode() {
            var sbCode = new StringBuilder();

            sbCode.Append("foreach(_U_ ");
            sbCode.Append(this.Name);
            sbCode.Append(" in (");
            this.Where.ElemeAllToString(sbCode);
            sbCode.Append(").Value)");
            sbCode.AppendLine("{");
            this.ElemeAllToString(sbCode);
            sbCode.AppendLine("}");

            return sbCode.ToString();
        }
    }
}
