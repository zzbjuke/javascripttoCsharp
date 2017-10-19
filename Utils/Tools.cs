using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using XZ.ParseLanguage.Parser;

namespace XZ.ParseLanguage.Utils {
    public class Tools {

        /// <summary>
        /// 表示行已经结束
        /// </summary>
        public const char LineEndChar = NullChar;

        /// <summary>
        /// 空字符串
        /// </summary>
        public const char NullChar = '\0';

        /// <summary>
        /// 全部结束
        /// </summary>
        public const char AllEndChar = (char)250;

        /// <summary>
        /// 未定义字符串
        /// </summary>
        public const char NoChar = (char)249;

        /// <summary>
        /// 空格字符
        /// </summary>
        public static readonly char[] SpaceChars = { ' ', '\t' };

        /// <summary>
        /// 字符串开头
        /// </summary>
        public static readonly char[] StrStartChars = { '\'', '"' };

        /// <summary>
        /// 行分隔符
        /// </summary>
        public const char LineSpaceChar = ';';

        public static readonly HashSet<char> HasVarHeadChar = new HashSet<char>(){
         '_','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
         };

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFile(string path) {
            using (FileStream fs = File.OpenRead(path)) {
                using (StreamReader sr = new StreamReader(fs))
                    return sr.ReadToEnd();
            }
        }

        public static List<string> ReadFileLins(string path) {
            return ReadFileLins(path, Encoding.UTF8);
        }

        /// <summary>
        /// 读取文件行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> ReadFileLins(string path,Encoding encoding) {
            using (FileStream fs = File.OpenRead(path)) {
                using (StreamReader sr = new StreamReader(fs, encoding)) {
                    var list = new List<string>();
                    while (sr.Peek() > -1)
                        list.Add(sr.ReadLine());
                    return list;
                }
            }
        }

        

        /// <summary>
        /// 验证是否是数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ValidInt(string value) {
            if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return false;
            else
                return true;
        }

        /// <summary>
        /// 是否已经结束
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsEnd(char c) {
            return c == LineEndChar || c == AllEndChar || c == ';';
        }
                

        

        /// <summary>
        /// 判断是否是变量的开头
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool HasVarHead(char c) {
            return HasVarHeadChar.Contains(c);
        }

        /// <summary>
        /// 获取文件名的类名称
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileClass(string fileName) {
            return fileName.Replace("/", "_");
        }
        
    }
}
