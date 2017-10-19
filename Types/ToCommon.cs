using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XZ.ParseLanguage.Types {
    public static class ToCommon {


        /// <summary>
        /// 判断 泛型是否有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this List<T> list) {
            return !(list != null && list.Count > 0);
        }

        /// <summary>
        /// 判断数组是否有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T[] array) {
            return !(array != null && array.Length > 0);
        }

        /// <summary>
        /// 替换字符串二端字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Trim(this string str, params string[] args) {
            if (args.Length == 0)
                return str.Trim();

            return str.TrimStart(args).TrimEnd(args);
        }
        /// <summary>
        ///  从当前 System.String 对象移除数组中指定的一组字符的所有前导匹配项。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimStr">要除去的字符</param>
        /// <returns></returns>
        public static string TrimStart(this string str, params string[] args) {

            if (string.IsNullOrEmpty(str) || args.IsNull())
                return str;

        _While:

            bool isWith = false;
            int i = 0;
            while (i < args.Length) {
                var trimStr = args[i];
                if (str.StartsWith(trimStr)) {
                    isWith = true;
                    str = str.Substring(trimStr.Length);
                    break;
                }
                i++;
            }

            if (isWith)
                goto _While;

            return str;
            ;

        }

        /// <summary>
        /// 从当前 System.String 对象移除数组中指定的一组字符的所有结尾匹配项。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimStr"></param>
        /// <returns></returns>
        public static string TrimEnd(this string str, params string[] args) {
            if (string.IsNullOrEmpty(str) || args.IsNull())
                return str;

        _While:

            bool isWith = false;
            int i = 0;
            while (i < args.Length) {
                var trimStr = args[i];
                if (str.EndsWith(trimStr)) {
                    isWith = true;
                    str = str.Substring(0, str.Length - trimStr.Length);
                    break;
                }
                i++;
            }

            if (isWith)
                goto _While;

            return str;

        }

        /// <summary>
        /// 将字符串转化为数组
        /// </summary>
        /// <param name="strContent">转化的内容</param>
        /// <param name="strSplit">分割符</param>
        /// <returns></returns>
        public static string[] ToSplitStrings(this string strContent, string strSplit = ",") {
            if (!string.IsNullOrWhiteSpace(strContent)) {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        #region 类型转换

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ToInt(this object expression, int defValue = 0) {
            if (expression != null)
                return expression.ToString().ToInt(defValue);

            return defValue;
        }

        #endregion
    }
}
