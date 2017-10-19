using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class _Dictionary : BaseType {

        private Dictionary<string, _U_> _dict = new Dictionary<string, _U_>();

        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public _Dictionary __Add__(string key, _U_ value) {
            _dict[key] = value;
            return this;
        }

        protected override _U_ GetInvoke(string name) {
            if (_dict.ContainsKey(name))
                return _dict[name];
            else
                return _U_.Undefined();
        }

        public Dictionary<string, _U_> GetValue() {
            return _dict;
        }

        public override void __Set__(string name, _U_ value) {
            _dict[name] = value;
        }

        public override string ToString() {
            var array = new List<string>();
            foreach (var kv in _dict)
                array.Add(kv.Key + ":" + kv.Value);
            return "{" + string.Join(",", array) + "}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator GetEnumerator() {
            foreach (var kv in _dict.Keys)
                yield return _U_.NewString(kv);
        }


        public override bool _Exists {
            get {
                return this._dict.Count > 0;
            }
        }
    }
}
