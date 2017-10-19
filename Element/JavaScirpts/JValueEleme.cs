using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JValueEleme : ValueEleme {

        public override string ToCode() {
            var sbCode = new StringBuilder();
            if (this.Characteristic != null)
                sbCode.Append("__" + this.Characteristic.GetMethodName() + "__(");
            return sbCode.ToString();
        }
    }
}
