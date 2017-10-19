using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public static class window {

        private static Dictionary<string, _U_> data = new Dictionary<string, _U_>();

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void __Set__(string name, _U_ value) {
            data[name] = value;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static _U_ __Get__(string name) {
            if (data.ContainsKey(name))
                return data[name];
            return _U_.Undefined();
        }
    }
}
