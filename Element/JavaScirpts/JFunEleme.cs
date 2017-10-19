using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JFunEleme : FunEleme, IHChild {

        public string ArgumentsEleme { get; set; }

        public SortedDictionary<string, BEleme> PropertyValue { get; set; }

        public override string ToCode() {
            StringBuilder sbCode = new StringBuilder();
            //if (this.PVariableType == Entity.EVariableType.Definition)
            //    sbCode.Append("var ");
            if (!string.IsNullOrEmpty(this.Name)) {
                if (string.IsNullOrEmpty(this.AliasName))
                    sbCode.Append(Name);
                else
                    sbCode.Append(AliasName);
                sbCode.Append(" = ");
            }
            sbCode.AppendLine("_U_.New<_Function>(new _Function((__result" + this.Sign + "__,__this" + this.Sign + "__,__paramers" + this.Sign + "__)=>{");
            if (this.Variables != null) {
                sbCode.AppendLine("#region 定义变量");
                foreach (var v in this.Variables)
                    sbCode.AppendLine(string.Format("var {0} = _U_.Undefined();", v.Value.GetAliasName()));
                sbCode.AppendLine("#endregion");
            }
            if (this.ArgumentsEleme != null)
                sbCode.Append(this.ArgumentsEleme);

            this.ElemeAllToString(sbCode);
            sbCode.Append("}))");
            if (!string.IsNullOrEmpty(this.Name))
                sbCode.AppendLine(";");

            return sbCode.ToString();
        }

        public void SetArgEleme() {
            if (this.ArgumentsEleme == null)
                this.ArgumentsEleme = string.Format("var __arguments{0}__ = __paramers{0}__.__ChangeArg__();\r\n", this.Sign);

        }

        public string GetAraumerName {
            get {
                return "__arguments" + this.Sign + "__";
            }
        }

        public string GetThisName {
            get {
                return "__this" + this.Sign + "__";
            }
        }

        /// <summary>
        /// 添加类 属性值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eleme"></param>
        public void AddPropertyValue(string name, BEleme eleme) {
            if (PropertyValue == null)
                this.PropertyValue = new SortedDictionary<string, BEleme>();
            PropertyValue[name] = eleme;
        }
    }
}
