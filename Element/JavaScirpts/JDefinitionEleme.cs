using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JDefinitionEleme : DefinitionEleme {

        /// <summary>
        /// 是否是函数参数
        /// </summary>
        public bool IsFunArg { get; set; }

        public override string ToCode() {
            if (this.Childs == null)
                throw new Exception("DefinitionEleme 必须包含值");

            StringBuilder sbCode = new StringBuilder();
            //if (this.PVariableType == Entity.EVariableType.Definition)
            //    sbCode.Append("var ");
            if (string.IsNullOrEmpty(this.AliasName))
                sbCode.Append(this.Name);
            else
                sbCode.Append(this.AliasName);
            sbCode.Append(" ");
            if (!string.IsNullOrEmpty(this.OperationChar))
                sbCode.Append(this.OperationChar);
            sbCode.Append("= ");
            this.ElemeAllToString(sbCode);
            sbCode.AppendLine(";");
            return sbCode.ToString();
        }
    }
}
