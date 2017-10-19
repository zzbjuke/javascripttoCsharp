using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public static class ToObject {
        /// <summary>
        /// 获取数组的项
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static _U_ __Get__(this _U_[] args, int index = 0) {
            if (args != null && args.Length > index)
                return args[index];
            else
                return _U_.Undefined();
        }

        /// <summary>
        /// 将数组转换为_Array
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static _U_ __ChangeArg__(this _U_[] args) {
            var array = new _Array();
            if (args != null)
                foreach (var a in args)
                    array.__Add__(a);

            return _U_.New<_Array>(array);
        }


        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(string value) {
            try {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch {
                throw new Exception(string.Format("无法将 \"{0}\" 转化为:{1}", value, typeof(T).Name));
            }
        }

        public static DateTime ToDateTimeForUseVar(this _U_ v, out bool success) {
            DateTime _dt;
            success = false;
            if (DateTime.TryParse(v.ToString(), out _dt))
                success = true;

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static double? GetValueDoutle(this _U_ u) {
            if (u.Value is _Number) {
                return (u.Value as _Number);
            }
            else if (u.Value is _String) {
                double outValue = 0;
                if (double.TryParse(u.ToString(), out outValue))
                    return outValue;
            }
            else if (u.Value is _Bool) {
                bool v = u.Value as _Bool;
                return v == true ? 1 : 0;
            }
            return null;
        }

        public static bool IsNullNaNUndefined(this _U_ _this) {
            if (_this.Value is NaN || _this.Value is Null || _this.Value is Undefined)
                return true;
            else
                return false;
        }

        public static string[] ToArrayForUseVar(this _U_[] args, int startIndex = 0) {
            if (args == null || args.Length <= startIndex)
                return new string[0];

            var array = new string[args.Length - startIndex];
            for (var i = startIndex; i < args.Length; i++)
                array[i - startIndex] = args[i].ToString();

            return array;
        }
    }
}
