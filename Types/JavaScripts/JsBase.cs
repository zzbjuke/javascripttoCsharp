using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class JsBase {
        protected static _U_ console = _U_.New<_Console>();


        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        protected _U_ __typeof__(_U_ u) {
            return _U_.NewString(u.Value.GetTypeName);
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="u"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        protected void __throw__(_U_ u) {
            throw new Exception(u.ToString());
        }

        protected _U_ __AfterAdds__(_U_ u) {
            var d = u.GetValueDoutle();
            if (d == null) {
                u.Value = _U_.NaN().Value;
                return _U_.NaN();
            }
            else {
                u.Value = new _Number(d.Value + 1);
                return _U_.NewNumber(d.Value);
            }
        }

        protected _U_ __AfterSubs__(_U_ u) {
            var d = u.GetValueDoutle();
            if (d == null) {
                u.Value = _U_.NaN().Value;
                return _U_.NaN();
            }
            else {
                u.Value = new _Number(d.Value - 1);
                return _U_.NewNumber(d.Value);
            }
        }

        protected _U_ __BeforeSubs__(_U_ u) {
            var d = u.GetValueDoutle();
            if (d == null) {
                u.Value = _U_.NaN().Value;
                return _U_.NaN();
            }
            else {
                u.Value = new _Number(d.Value - 1);
                return u;
            }
        }

        protected _U_ __BeforeAdds__(_U_ u) {
            var d = u.GetValueDoutle();
            if (d == null) {
                u.Value = _U_.NaN().Value;
                return _U_.NaN();
            }
            else {
                u.Value = new _Number(d.Value + 1);
                return u;
            }
        }

        protected _U_ __BeforeSub__(_U_ u) {
            var d = u.GetValueDoutle();
            if (d == null) {
                u.Value = _U_.NaN().Value;
                return _U_.NaN();
            }
            else {
                u.Value = new _Number(d.Value * -1);
                return u;
            }
        }

        protected _U_ __BeforeAdd__(_U_ u) {
            var d = u.GetValueDoutle();
            if (d == null) {
                u.Value = _U_.NaN().Value;
                return _U_.NaN();
            }
            else {
                u.Value = new _Number(d.Value);
                return u;
            }
        }
    }
}
