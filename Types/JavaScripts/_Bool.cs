using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class _Bool : BaseType {
        private bool _value;

        public _Bool(bool value) {
            this._value = value;
        }

        public static implicit operator _Bool(bool v) {
            return new _Bool(v);
        }

        public static implicit operator bool(_Bool v) {
            return v._value;
        }

        public override string GetTypeName {
            get {
                return "boolean";
            }
        }

        public override bool _Exists {
            get {
                return _value;
            }
        }

        public override bool Equals(object obj) {
            if (obj is _Bool && (obj as _Bool)._value == _value)
                return true;
            else if (obj is _Number && ((_Number)obj == (this._value ? 1 : 0)))
                return true;

            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return _value.ToString().ToLower();
        }
    }
}
