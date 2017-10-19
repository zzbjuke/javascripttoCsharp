using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class _Number : BaseType {
        private double _value;

        /// <summary>
        /// 创建数字类型
        /// </summary>
        /// <param name="value"></param>
        public _Number(double value) {
            this._value = value;
        }
        
        public static implicit operator _Number(double v) {
            return new _Number(v);
        }

        public static implicit operator double(_Number v) {
            return v._value;
        }

        /// <summary>
        /// 设置委托
        /// </summary>
        /// <param name="action"></param>
        public void SetAction(Func<double,double> action) {
            this._value = action(this._value);
        }       

        internal override BaseType Clone() {
            return new _Number(this._value);
        }

        public override bool _Exists {
            get {
                return this._value != 0;
            }
        }

        public double GSNumberValue {
            get {
                return _value;
            }
            set {
                _value = value;
            }
        }

        public override void __Set__(string name, _U_ value) {
            //base.__Set__(name, value);
        }

        public override _U_ __Call__(string name, _U_ _this, params _U_[] args) {
            switch (name) {
                case "toFixed":
                    return _U_.NewNumber(Math.Round(this._value, args.__Get__().ToInt()));
                default:
                    return base.__Call__(name, _this, args);
            }
            
        }

        public override bool Equals(object obj) {
            if (obj is _Number && (obj as _Number)._value == _value)
                return true;
            else if (obj is _Bool && ((_Bool)obj == (this._value == 0 ? false : true)))
                return true;
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return _value.ToString();
        }

        public override string GetTypeName {
            get {
                return "number";
            }
        }
    }
}
