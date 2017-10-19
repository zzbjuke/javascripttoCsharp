using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.Parser;
using XZ.ParseLanguage.Parser.JavaScripts;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage {
    public abstract class AnalysisBase {

        protected static Dictionary<string, BEleme> Datas = new Dictionary<string, BEleme>();

        protected static object lockWrite = new object();

        public AnalysisBase(string root, ELanguage type = ELanguage.JavaScript) {
            this.Root = root;
            this.EType = type;

        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static BEleme GetData(string fileName) {
            BEleme outElem;
            if (Datas.TryGetValue(fileName, out outElem))
                return outElem;

            return null;

        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="fileName"></param>
        public static void RemoveDatas(string fileName) {
            lock (lockWrite) {
                if (Datas.ContainsKey(fileName))
                    Datas.Remove(fileName);
            }
        }

        public void AddData(string fileName, BEleme value, BParser bParser) {
            lock (lockWrite) {
                if (Datas.ContainsKey(fileName) && this.IsStartUp)
                    return;

                Datas[fileName] = value;
                this.WriteFile(bParser, value, fileName);

            }

            if (!Datas.ContainsKey(fileName)) {
                lock (lockWrite) {
                    if (!Datas.ContainsKey(fileName)) {
                        Datas[fileName] = value;
                        this.WriteFile(bParser, value, fileName);
                    }
                }
            }
        }

        /// <summary>
        /// 清除缓存中的数据
        /// </summary>
        public static void CelarDatas() {
            Datas.Clear();
        }

        public bool IsStartUp { get; set; }

        protected Encoding ecoding = Encoding.UTF8;

        /// <summary>
        /// 设置读取文件编码
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public void SetEncoding(string encoding) {
            if (string.IsNullOrWhiteSpace(encoding))
                return;
            ecoding = Encoding.GetEncoding(encoding);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public BParser CreateParser() {
            BParser bparse = null;
            switch (this.EType) {
                case ELanguage.JavaScript:
                    bparse = new JavaScriptCodeParser();
                    break;
                //default:
                //    bparse = new CSharpCodeParser();
                //    break;
            }
            bparse.FileAnalysisFunc = this.FileAnalysis;
            return bparse;
        }

        protected virtual BEleme FileAnalysis(string file) {
            return null;
        }

        public ELanguage EType { get; set; }

        /// <summary>
        /// 替换字符行是否直接通过。如果不通过只消除替换字符
        /// </summary>
        public bool ReplaceCharLinePass { get; set; }

        /// <summary>
        /// 更目录
        /// </summary>
        public string Root { get; set; }

        public string WriteRoot { get; set; }

        //protected BParser PBparser { get; set; }

        private Func<string, string, bool> writeCompileText;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public bool GetCompileText(string fileName, string content) {
            if (this.writeCompileText != null)
                return this.writeCompileText(fileName, content);

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writeCompileText"></param>
        public void SetCompileText(Func<string, string, bool> writeCompileText) {
            this.writeCompileText = writeCompileText;
        }

        /// <summary>
        /// 执行之前
        /// </summary>
        public Func<string, BEleme> ExecBeforeFunc { get; set; }

        /// <summary>
        /// 执行之前
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual BEleme ExecBefore(string fileName) {
            if (ExecBeforeFunc != null)
                return ExecBeforeFunc(fileName);

            return null;
        }

        protected string CompileNameSpan = "Test";

        /// <summary>
        /// 设置编译文件之后的命名空间
        /// </summary>
        /// <param name="nameSpan"></param>
        public void SetNameSpane(string nameSpan) {
            this.CompileNameSpan = nameSpan;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public abstract void Eexc(string path);

        public virtual string FileExtension { get; private set; }


        /// <summary>
        /// 获取CS文件的名称
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileNameForCs(string fileName) {
            return fileName.GetFileNameForCs(this.FileExtension);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="b"></param>
        /// <param name="fileName"></param>
        protected virtual void WriteFile(BParser parser, BEleme b, string fileName) { }
    }
}
