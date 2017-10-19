using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class Undefined : BaseType {

        public override string ToString() {
            return "undefined";
        }

        public override string GetTypeName {
            get {
                return "undefined";
            }
        }
    }
}
