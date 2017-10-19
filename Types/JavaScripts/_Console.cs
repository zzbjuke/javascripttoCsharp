using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class _Console : BaseType {
        /// <summary>
        /// 输出窗口
        /// </summary>
        public static Action<string> WriteLog;

        public override _U_ __Call__(string name, _U_ _this, params _U_[] args) {
            switch (name) {
                case "log":
                    return Log(args);
                case "read":
                    return Read();
                default:
                    return base.__Call__(name, _this, args);
            }            
        }

        public _U_ Read() {
            var r = Console.ReadLine();
            return _U_.NewString(r);
        }

        /// <summary>
        /// 日子输出
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private _U_ Log(params _U_[] args) {
            if (args != null && WriteLog!=null) 
                WriteLog(string.Join<_U_>(" ", args));

            return _U_.Undefined();
        }
        
    }
}
