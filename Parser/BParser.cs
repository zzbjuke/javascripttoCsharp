using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser {
    public abstract class BParser {



        /// <summary>
        /// 当前坐标
        /// </summary>
        public CPoint Point = new CPoint();

        /// <summary>
        /// 代码字符串
        /// </summary>
        protected List<string> CodeStrings { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        internal string FilePath { get; set; }

        /// <summary>
        /// 根目录
        /// </summary>
        internal string RootPath { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pt"></param>
        protected void Init(ParserText pt) {
            FilePath = pt.Path;
            this.RootPath = pt.RootPath;
            if (pt.Content.IsNull())
                CodeStrings = Tools.ReadFileLins(pt.Path);
            else
                CodeStrings = pt.Content;
        }

        public int VarIndexCount { get; set; }

        #region 获取标签

        /// <summary>
        /// 替换字符行是否直接通过。如果不通过只消除替换字符
        /// </summary>
        public bool ReplaceCharLinePass { get; set; }

        /// <summary>
        /// 获取下一个字符串
        /// <param name="isEnter">是否换行查找</param>
        /// </summary>
        public char GetNextChar(bool isEnter = true) {
            if (Point.Y >= this.CodeStrings.Count)
                return Tools.AllEndChar;

            if (this.CodeStrings[Point.Y].Length <= Point.X) {
                if (isEnter) {
                    Point.Y++;
                    Point.X = 0;
                    return GetNextChar(isEnter);
                }
                return Tools.LineEndChar;
            }
            else
                return this.CodeStrings[Point.Y][Point.X++];

        }

        /// <summary>
        /// 行注释
        /// </summary>
        //public string LineAnnotation { get; set; }



        /// <summary>
        /// 查找下一个字符串
        /// </summary>
        /// <param name="isEnter">是否允许换行</param>
        /// <param name="allows">允许出现的字符</param>
        /// <returns></returns>
        public char GetNextChar(bool isEnter, params char[] allows) {
        Start:
            var ch = this.GetNextChar(isEnter);
            if (ch == Tools.AllEndChar)
                return Tools.AllEndChar;
            char nc = Tools.NullChar;
            switch (ch) {
                case '/':
                    nc = this.GetNextChar(false);
                    if (nc == '/') {
                        //this.LineAnnotation += this.CodeStrings[Point.Y] + "\r\n";
                        this.POperate.AddLineAnnotation(this.CodeStrings[Point.Y]);
                        this.Point.X = 0;
                        this.Point.Y++;                        
                        goto Start;
                    }
                    this.Point.X--;
                    break;
                case '#':
                    nc = this.GetNextChar(false);
                    if (nc == '#') {
                        if (this.ReplaceCharLinePass) {
                            this.Point.X = 0;
                            this.Point.Y++;
                            goto Start;
                        }
                        nc = this.GetNextChar(false);
                        while (nc == '#')
                            nc = this.GetNextChar(false);

                        this.Point.X--;
                        goto Start;
                    }
                    this.Point.X--;
                    break;
            }
            if (allows == null || allows.Length == 0)
                return ch;

            if (allows.Contains(ch))
                goto Start;

            return ch;
        }

        /// <summary>
        /// 查找下一个字符串 允许换行查找
        /// </summary>
        /// <param name="allows">允许出现的字符</param>
        /// <returns></returns>
        public char GetNextChar(params char[] allows) {
            return this.GetNextChar(true, allows);
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="isEnter">是否跨行查找</param>
        /// <returns></returns>
        public string GetTag(bool isEnter = true) {
            return this.GetTag(isEnter, null);
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="isEnter">是否跨行查找</param>
        /// <param name="allows">允许出现的字符串</param>
        /// <returns></returns>
        public string GetTag(bool isEnter, params char[] allows) {
            int length = this.CodeStrings[this.Point.Y].Length;
            bool isStart = false;
            string tag = null;
            char c;
            while (true) {
                if (length <= this.Point.X) {
                    if (tag == null && isEnter && this.Point.Y < this.CodeStrings.Count - 1) {
                        this.Point.Y++;
                        this.Point.X = 0;
                        return GetTag(isEnter, allows);
                    }
                    return tag;
                }
                c = this.CodeStrings[this.Point.Y][this.Point.X];
                if (HasSepartorChar.Contains(c)) {
                    if (allows != null && allows.Contains(c) && !isStart) {
                        this.Point.X++;
                        continue;
                    }
                    else
                        return tag;

                    //if (allows == null || !(allows.Contains(c) && !isStart))
                    //    return tag;
                    //else {
                    //    this.Point.X++;
                    //    continue;
                    //}
                }
                tag += c;
                isStart = true;
                this.Point.X++;
            }
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="isBackSpace">是否退一个字符</param>
        /// <param name="isEnter">是否跨行查找</param>
        /// <returns></returns>
        public string GetTag(bool isBackSpace, bool isEnter = true) {
            return GetTag(isBackSpace, isEnter, null);
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="allows">允许出现的字符串</param>
        /// <returns></returns>
        public string GetTagAllow(params char[] allows) {
            return GetTag(false, true, allows);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="isBackSpace"></param>
        /// <param name="isEnter"></param>
        /// <param name="allows"></param>
        /// <returns></returns>
        public string GetTag(bool isBackSpace, bool isEnter, params char[] allows) {
            if (isBackSpace)
                this.Point.X--;
            return this.GetTag(isEnter, allows);
        }

        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="c">字符串字符 '或"</param>
        /// <param name="isEnter">是否允许回车</param>
        /// <returns></returns>
        public string GetString(char sC, bool isEnter = false) {
            int length = this.CodeStrings[this.Point.Y].Length;
            string tag = null;
            char nc = Tools.NullChar;
            char c;
            while (true) {
                if (length <= this.Point.X) {
                    if (isEnter) {
                        this.Point.Y++;
                        this.Point.X = 0;
                        return GetString(sC, isEnter);
                    }
                    return tag;
                }
                c = this.CodeStrings[this.Point.Y][this.Point.X];
                if (c == sC) {
                    if (nc != '\\') {
                        this.Point.X++;
                        return tag ?? "";
                    }
                }
                tag += c;
                nc = c;
                this.Point.X++;
            }
        }

        #endregion

        #region 外部调用方法

        /// <summary>
        /// 当前快
        /// </summary>
        //public RangeEleme REleme { get; set; }

        /// <summary>
        /// 解析文件
        /// </summary>
        public Action<string> AnalysisFileAction { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        public abstract BEleme Execute(ParserText pt);

        /// <summary>
        /// 解析文件
        /// </summary>
        /// <param name="writeFile">写入文件委托</param>
        internal void AnalysisFile(Action<string> writeFile) {
            this.AnalysisFileAction = writeFile;
        }

        /// <summary>
        /// 判断文件是否已经被解析
        /// </summary>
        public Func<string, BEleme> FileAnalysisFunc { get; set; }

        /// <summary>
        /// 文件是否已经解析
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal BEleme FileAnalysis(string file) {
            if (FileAnalysisFunc != null)
                return FileAnalysisFunc(file);
            else
                return null;
        }

        /// <summary>
        /// 读取文件之前
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal void ReadFileBefore(string content) {

        }

        #endregion

        public virtual Operate POperate { get; private set; }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string GetIncludeVar(string name) {
            return null;
        }

        /// <summary>
        /// 块 如 page class
        /// </summary>
        public virtual BEleme LumpElem { get; private set; }

        /// <summary>
        /// 验证变量是否合法
        /// </summary>
        /// <param name="tag"></param>
        public virtual void ValidVar(string tag) {
            if (!Regex.IsMatch(tag, "^[_a-zA-Z][_a-zA-Z0-9]*$"))
                this.Error(tag + ":变量定义不合法");

        }


        /// <summary>
        /// 抛出错误
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Error(string msg) {
            throw new Exception(msg + "\r\n位置："
                + string.Format("Line:{0},Chars:{1}\r\n", this.Point.Y + 1, this.Point.X + 1)
                + "文件：" + this.FilePath
                );
        }

        #region 字符
        /// <summary>
        /// 分割字符
        /// </summary>
        /// <returns></returns>
        public virtual HashSet<char> HasSepartorChar {
            get {
                return new HashSet<char>(){
                    ' ',
                    '.',
                    '(',
                    ')',
                    '[',
                    ']',
                    '{',
                    '}',
                    '|',
                    '&',
                    '!',
                    '<',
                    '>',
                    '=',
                    '+',
                    '-',
                    '*',
                    '/',
                    ';',
                    ',',
                    ':'
                };
            }
        }

        /// <summary>
        /// 开头允许出现的字符
        /// </summary>
        public virtual HashSet<char> HasHeadChars {
            get {
                return new HashSet<char>() {
                    '+','-','*','/','=','{','}','[',']','(',')','!','>','<'
                };
            }
        }

        /// <summary>
        /// 变量之间操作
        /// </summary>
        public virtual HashSet<char> HasVarLink {
            get {
                return new HashSet<char>() { 
                    '+','-','*','/','!','|','&','<','>'
                };
            }
        }

        #endregion

    }
}
