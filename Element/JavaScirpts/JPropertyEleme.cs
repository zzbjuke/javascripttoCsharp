using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JPropertyEleme : PropertyEleme, IVariableValue {
        public bool IsSetValue { get; set; }
        public ValueEleme Value { get; set; }
        public override string ToCode() {
            if (this.IsSetValue) {
                var sbCode = new StringBuilder();
                sbCode.Append(".__Set__(\"");
                sbCode.Append(this.Name);
                sbCode.Append("\",");
                //sbCode.Append(",");
                if (this.Value != null)
                    this.Value.ElemeAllToString(sbCode);
                sbCode.Append(")");
                return sbCode.ToString();
            }
            else
                return ".__Get__(\"" + this.Name + "\")";
        }
    }
}
