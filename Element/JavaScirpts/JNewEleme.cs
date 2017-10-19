using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JNewEleme : BEleme, IHChild {
        public List<ValueEleme> Arguments { get; set; }

        public override string ToCode() {
            bool isSys = false;
            var sbCode = new StringBuilder();
            if (SysNew.Contains(this.Name)) {
                sbCode.AppendFormat("_U_.New<_{0}>(new _{0}(", this.Name);
                isSys = true;
            }
            else
                sbCode.Append(this.Name + ".__New__(\"" + this.Name + "\"");
            if (this.Arguments != null) {
                if (this.Arguments.Count > 0 && !isSys)
                    sbCode.Append(",");
                for (var i = 0; i < Arguments.Count; i++) {
                    Arguments[i].ElemeAllToString(sbCode);
                    if (i < Arguments.Count - 1)
                        sbCode.Append(",");
                }
            }
            sbCode.Append(")");
            if (isSys)
                sbCode.Append(")");
            return sbCode.ToString();

        }

        public readonly static string[] SysNew = { "Date" };
    }
}
