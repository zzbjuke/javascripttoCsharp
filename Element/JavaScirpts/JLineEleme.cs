using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JLineEleme : BEleme {

        public override string ToCode() {
            var sbCode = new StringBuilder();
            if (this.Childs != null)
                foreach (var c in Childs)
                    c.ElemeAllToString(sbCode);
            sbCode.AppendLine(";");
            return sbCode.ToString();
        }
    }
}
