using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JWhileEleme : JWhereEleme, IHChild, IBreak {

        public override string ToCode() {
            var sbCode = new StringBuilder();
            sbCode.Append("while(");
            this.Where.ElemeAllToString(sbCode);
            sbCode.AppendLine("){");
            this.ElemeAllToString(sbCode);
            sbCode.AppendLine("}");
            return sbCode.ToString();
        }
    }
}
