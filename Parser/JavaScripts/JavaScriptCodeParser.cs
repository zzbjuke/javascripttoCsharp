using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class JavaScriptCodeParser : BParser {
        /// <summary>
        /// 执行 开始分析代码
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override BEleme Execute(ParserText pt) {
            this.Init(pt);
            this.Line();
            return this.LumpElem;
        }

        private BEleme lumpElem = new JPageEleme();

        public override BEleme LumpElem {
            get {
                return lumpElem;
            }
        }

        private Operate operate;

        public override Operate POperate {
            get {
                if (this.operate == null)
                    this.operate = new JavaScriptOperate(this);
                return this.operate;
            }
        }

        public HashSet<string> IncludeFile { get; set; }

        /// <summary>
        /// 解析行
        /// </summary>
        /// <param name="line"></param>
        private void Line() {
            var c = this.GetNextChar(' ');
            if (c == Tools.AllEndChar)
                return;
            this.Point.X--;
            var jspan = new JavaSciptSpan(this, this.lumpElem);
            jspan.Init();

            this.Line();
        }


    }
}
