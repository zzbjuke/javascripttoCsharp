using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JTypeofEleme : BEleme, IHChild {


        public override string ToCode() {
            var sbCode = new StringBuilder();
            sbCode.Append("__typeof__(");
            foreach (var c in Childs)
                c.ElemeAllToString(sbCode);
            sbCode.Append(")");
            return sbCode.ToString();
        }
    }
}
