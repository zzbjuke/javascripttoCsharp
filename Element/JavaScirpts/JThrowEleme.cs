using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.Parser.JavaScripts;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JThrowEleme : BEleme{
        public new CPoint Point { get; set; }
        public override string ToCode() {
            var sbCode = new StringBuilder();
            sbCode.Append("throw new ExceptionPoint((");
            this.ElemeAllToString(sbCode);
            sbCode.Append(").ToString(),");
            sbCode.Append(this.Point.X);
            sbCode.Append(",");
            sbCode.Append(this.Point.Y);
            sbCode.AppendLine(");");
            return sbCode.ToString();            
        }
    }
}
