using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JVariableEleme : VariableEleme {

        /// <summary>
        /// 表示是否是开始
        /// </summary>
        public bool IsStart { get; set; }

        public override string ToCode() {
            string value = this.GetValue();
            if (this.Characteristic == null)
                return value;

            var sbCode = new StringBuilder();
            sbCode.Append("__" + this.Characteristic.GetMethodName() + "__(");
            sbCode.Append(value);
            sbCode.Append(")");
            return sbCode.ToString();
        }


        private string GetValue() {
            var sbCode = new StringBuilder();
            if (!string.IsNullOrEmpty(this.Name))
                sbCode.Append(this.Name);
            if (this.Property != null) {
                sbCode.Append(this.Property.ToCode());
                this.Property.ElemeAllToString(sbCode);
            }
            else if (this.Method != null) {
                sbCode.Append(this.Method.ToCode());
                this.Method.ElemeAllToString(sbCode);
            }
            else if (this.Index != null) {
                if (this.Index is JIndexEleme) {
                    sbCode.Append("[");
                    var jindexE = this.Index as JIndexEleme;
                    if (jindexE.IndexEleme is JValueEleme)
                        foreach (var i in jindexE.IndexEleme.Childs)
                            i.ElemeAllToString(sbCode);
                    else
                        sbCode.Append(this.Index.ToCode());
                    sbCode.Append("]");
                }
                else
                    throw new Exception("索引错误");

                this.Index.ElemeAllToString(sbCode);
            }
            return sbCode.ToString();
        }
    }
}
