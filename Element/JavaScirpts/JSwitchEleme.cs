using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Parser.JavaScripts;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JSwitchEleme : JWhereEleme {
        public List<JCaseEleme> Case { get; set; }
        public JCaseEleme Default { get; set; }


        public override string ToCode() {
            var sbCode = new StringBuilder();
            sbCode.AppendLine("#region Switch");
            string tag = "__boole" + this.Sign + "_";
            sbCode.Append("var " + this.GetSwitchWhere + " = ");
            foreach (var w in Where.Childs) 
                w.ElemeAllToString(sbCode);
            sbCode.AppendLine(";");
            sbCode.AppendLine("var " + tag + " =  _U_.NewBoolean(false);");
            if (this.Case != null) {
                foreach (var c in Case) {
                    sbCode.Append("if(" + tag + " ||" + this.GetSwitchWhere + " == ");
                    c.Value.ElemeAllToString(sbCode);
                    sbCode.AppendLine("){");
                    c.ElemeAllToString(sbCode);
                    sbCode.Append(tag + " = _U_.NewBoolean(true);");
                    sbCode.AppendLine("}");
                }
            }

            this.Default.ElemeAllToString(sbCode);

            sbCode.AppendLine(this.EndGoToTagName + ":{}");
            sbCode.AppendLine("#endregion");
            return sbCode.ToString();
        }

        public string Sign { get; set; }

        public string EndGoToTagName {
            get {
                return "EndSwitch_" + this.Sign + "_";
            }
        }

        public string GetSwitchWhere {
            get {
                return "SwitchWhere_" + this.Sign + "_";
            }
        }
    }
}
