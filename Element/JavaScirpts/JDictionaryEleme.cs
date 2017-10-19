using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JDictionaryEleme : BEleme, IHChild {

        public Dictionary<string, BEleme> Dict { get; set; }

        public override string ToCode() {
            var sbCode = new StringBuilder();
            sbCode.Append("_U_.New<_Dictionary>(new _Dictionary()");
            if (this.Dict != null) {
                foreach (var kv in Dict) {
                    sbCode.Append(".__Add__(");
                    sbCode.Append("\"" + kv.Key + "\",");
                    kv.Value.ElemeAllToString(sbCode);
                    sbCode.Append(")");
                }
            }
            sbCode.Append(")");

            return sbCode.ToString();
        }
    }
}
