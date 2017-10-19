using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {

    public delegate void _Delegate(_U_ result, _U_ _this, params _U_[] args);

    public class _Function : BaseType {

        private Dictionary<string, _U_> _dels = new Dictionary<string, _U_>();

        private _Delegate _delegate;

        public _Function(_Delegate _del) {
            _delegate = _del;
        }

        public override VarType VarType {
            get {
                return VarType.Object;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        public void __Init__(_U_ _this, params _U_[] pars) {
            var result = new _U_(new Undefined());
            _delegate(result, _this, pars);
            //return this;
        }

        public override void __Set__(string name, _U_ value) {
            if (name == "prototype") {
                if (value.Value is _Dictionary) {
                    var kvs = (value.Value as _Dictionary).GetValue();
                    foreach (var kv in kvs)
                        this.MethodAndPropery[kv.Key] = kv.Value;
                }
            }
            else
                base.__Set__(name, value);
        }

        /// <summary>
        /// 调用系统
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        private _U_ Call(_U_ _this, params _U_[] pars) {
            var result = new _U_(new Undefined());
            _delegate(result, _this, pars);
            return result;
        }

        public override _U_ __Call__(string name, _U_ _this, params _U_[] args) {
            switch (name) {
                case "":
                    return Call(_this, args);
                default:
                    return base.__Call__(name, _this, args);
            }
        }

        public override string ToString() {
            return "function(){ }";
        }

        public override string GetTypeName {
            get {
                return "function";
            }
        }

        public override bool _Exists {
            get {
                return true;
            }
        }
    }
}
