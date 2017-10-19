using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JArrayEleme : BEleme, IHChild {

        public override string ToCode() {
            StringBuilder sbCode = new StringBuilder();
            sbCode.Append("_U_.New<_Array>(new _Array()");
            if (this.Childs != null) {
                foreach (var c in Childs) {
                    sbCode.Append(".__Add__(");
                    c.ElemeAllToString(sbCode);
                    sbCode.Append(")");
                }
            }
            sbCode.Append(")");

            return sbCode.ToString();
        }
    }
}
