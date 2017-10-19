using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Entity;

namespace XZ.ParseLanguage.Parser {
    public abstract class Operate {

        public BParser PBParser { get; set; }

        public Operate(BParser parser) {
            PBParser = parser;
        }

        /// <summary>
        /// 将该节点添加到父节点中
        /// </summary>
        /// <param name="faher"></param>
        /// <param name="node"></param>
        public virtual void AddChild(BEleme faher, BEleme node) {

        }

        /// <summary>
        /// 验证变量是否存在
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="name">要查找的名称</param>
        /// <param name="isFatherFind">是否继续往父类查找</param>
        /// <returns></returns>
        public virtual BEleme ValidateNameExists(BEleme node, string name, bool isFatherFind) {
            return null;
        }

        public virtual bool VaildatePChar(string chars) {
            return chars == "++" || chars == "--" || chars == "-" || chars == "+";
        }

        /// <summary>
        /// 添加包含文件
        /// </summary>
        /// <param name="url"></param>
        public abstract void AddIncludeFile(string url);

        /// <summary>
        /// 添加被引用的变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        public abstract void AddIncludeVar(string name, BEleme eleme, string fileName);

        /// <summary>
        /// 重包含文件中获取变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract string GetIncludeVar(string name);

        /// <summary>
        /// 分析注释
        /// </summary>
        /// <param name="eleme"></param>
        public void AnalysisAnnotation(BEleme eleme) {
            StringBuilder sbDesc;
            if (lineEnd.TryGetValue(this.PBParser.Point.Y - 1, out sbDesc)) {
                if (!(eleme is DefinitionEleme || eleme is FunEleme))
                    return;

                LineAnnotation.Instance(this).Add(new LineAnnInfo() {
                    Desc = sbDesc.ToString(),
                    Eleme = eleme
                });
            }            
        }



        /// <summary>
        /// 清空行注释
        /// </summary>
        public void ClearLineAnnotation() {
            //this.PBParser.LineAnnotation = null;
        }

        protected bool lineExists = false;
        protected int lineExistsIndex = 0;
        protected int lineAnnioationIndex = 0;
        protected StringBuilder lineAnnioationDesc;
        protected HashSet<int> lineDict = new HashSet<int>();
        protected Dictionary<int, StringBuilder> lineEnd = new Dictionary<int, StringBuilder>();
        public void AddLineAnnotation(string desc) {
            if (lineDict.Contains(this.PBParser.Point.Y)) {
                this.lineExists = true;
                this.lineExistsIndex = this.PBParser.Point.Y;
                return;
            }
            if (this.lineExists && this.lineExistsIndex + 1 == this.PBParser.Point.Y) {
                this.lineExistsIndex++;
                return;
            }
            this.lineExists = false;
            if (lineAnnioationIndex + 1 != this.PBParser.Point.Y) {
                this.lineAnnioationDesc = new StringBuilder();
                lineDict.Add(this.PBParser.Point.Y);
            }            
            lineAnnioationDesc.AppendLine(desc);
            lineEnd.Add(this.PBParser.Point.Y, lineAnnioationDesc);
            this.lineAnnioationIndex = this.PBParser.Point.Y;
        }

        /// <summary>
        /// 读取行中的数据
        /// </summary>
        /// <param name="info"></param>
        public abstract void ReadLineAnnotation(LineAnnInfo info);
    }
}
