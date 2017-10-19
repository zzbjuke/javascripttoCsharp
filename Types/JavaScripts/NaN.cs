using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class NaN : BaseType {

        public override string ToString() {
            return "NaN";
        }

        public override string GetTypeName {
            get {
                return "number";
            }
        }
    }
}
