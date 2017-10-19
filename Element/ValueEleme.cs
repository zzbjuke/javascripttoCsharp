using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.interfaces;

namespace XZ.ParseLanguage.Element {
    public class ValueEleme : BEleme, IVariableAttr {

        public override string ToString() {
            
            return string.Empty;
        }

        public PropertyEleme Property {
            get;
            set;
        }

        public MethodEleme Method {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public CharacteristicInfo Characteristic {
            get;
            set;
        }

        public BEleme Index {
            get;
            set;
        }
    }
}
