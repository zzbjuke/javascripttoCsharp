using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.interfaces;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JIndexEleme : BEleme, IVariableValue {
        public BEleme IndexEleme { get; set; }

        public override string ToCode() {
            return this.IndexEleme.ToCode();
        }
    }
}
