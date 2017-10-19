using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Utils;
using XZ.ParseLanguage.Parser.JavaScripts;
using XZ.ParseLanguage.interfaces;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JMethodEleme : MethodEleme, IVariableValue {
        public override string ToCode() {
            var sbCode = new StringBuilder();
            sbCode.Append(".__Call__(\"");
            sbCode.Append(Name);
            sbCode.Append("\"");
            if (!this.Arguments.IsNull()) {
                sbCode.Append(",");
                for (var i = 0; i < this.Arguments.Count; i++) {
                    this.Arguments[i].ElemeAllToString(sbCode);
                    if (i < this.Arguments.Count - 1)
                        sbCode.Append(",");
                }
            }

            sbCode.Append(")");
            return sbCode.ToString();
        }

        //private void AddValue(
    }
}
