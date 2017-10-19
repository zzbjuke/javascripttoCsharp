using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Parser;

namespace XZ.ParseLanguage.Utils {
    public static class ToObject {
        /// <summary>
        /// 判断列队是否为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="isZero"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this List<T> list, bool isZero = true) {
            if (list == null || (isZero && list.Count == 0))
                return true;
            else
                return false;
        }

        

        /// <summary>
        /// 判断字符是否出现在数组中
        /// </summary>
        /// <param name="args"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsContains(this  char[] args, char c) {
            return args != null && args.Contains(c);
        }

        /// <summary>
        /// 判断字符串是否出现在数组中
        /// </summary>
        /// <param name="args"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsContains(this string[] args, string str) {
            return args != null && args.Contains(str);
        }

        

        /// <summary>
        /// 获取有效的Url地址
        /// </summary>
        /// <param name="span"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetValidUrl(this Span span, string url, Action<string> error) {
            if (url.StartsWith("/"))
                return url.TrimStart('/');
            else {
                string newUrl = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(span.PBParser.FilePath), url));
                if (!newUrl.StartsWith(span.PBParser.RootPath))
                    error(url + " 无效");
                return newUrl.Substring(span.PBParser.RootPath.Length).TrimStart('\\');
            }
        }

        /// <summary>
        /// 获取CS文件的名称
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileNameForCs(this string fileName, string fileExtension) {
            if (fileName.EndsWith(fileExtension, StringComparison.CurrentCultureIgnoreCase))
                fileName = fileName.Substring(0, fileName.Length - fileExtension.Length);

            return fileName.Replace("/", "_").Replace("\\", "_");
        }

        /// <summary>
        /// 获取文本中的字符串
        /// </summary>
        /// <param name="span"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetString(this Span span, char c) {
            string tag = span.PBParser.GetString(c);
            if (c == '\'')
                tag = tag.Replace("\\'", "'");

            return tag;
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static string GetIntString(this Span span) {
            var value = span.PBParser.GetTag(true, false);
            if (!Tools.ValidInt(value))
                span.Error("无效数据：" + value);

            span.SetUpPoint();
            var c = span.PBParser.GetNextChar(false);
            if (c == '.') {
                var _x = span.PBParser.GetTag(false);
                if (!Tools.ValidInt(_x))
                    span.Error("无效数据：" + _x);
                value += "." + _x;
            }
            else if (c != Tools.LineEndChar)
                span.ResetUpPoint();

            return value;
        }

        /// <summary>
        /// X--
        /// </summary>
        /// <param name="span"></param>
        public static void BackPoinX(this Span span, int value = 1) {
            span.PBParser.Point.X -= value;
        }

        /// <summary>
        /// x++
        /// </summary>
        /// <param name="span"></param>
        public static void AddPoinX(this Span span) {
            span.PBParser.Point.X++;
        }

        /// <summary>
        /// 获取父类
        /// </summary>
        /// <param name="e"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T GetFather<T>(this BEleme e) where T : class {
            var f = e;
            while (f != null) {
                if (f is T)
                    return f as T;

                f = f.Father;
            }

            return default(T);
        }

        public static string SetFirstLower(this string str) {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            else if (str.Length == 1)
                return str.ToLower();
            return str[0].ToString().ToLower() + str.Substring(1);
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
       
    }
}
