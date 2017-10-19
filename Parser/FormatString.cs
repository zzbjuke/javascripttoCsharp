using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser {
    /// <summary>
    /// 格式化字符串
    /// </summary>
    public class FormatString {
        StringBuilder sbCode = new StringBuilder();

        private BEleme _beleme;

        public FormatString(BEleme ele) {
            _beleme = ele;
        }

        ///// <summary>
        ///// 内容
        ///// </summary>
        //private void Body() {
        //    foreach (var child in this._beleme.Childs) {
        //        sbCode.Append(child.ToString());
        //        this.ForChild(child);
        //        if (child.IsSetVar())
        //            sbCode.AppendLine(";");
        //    }
        //}

        /// <summary>
        /// 循环子节点
        /// </summary>
        /// <param name="b"></param>
        private void ForChild(BEleme b) {
            if (b.Childs.IsNull())
                return;
            foreach (var child in b.Childs) {
                //if (!child.IsHide)
                sbCode.Append(child.ToString());

                //this.ForChild(child);
                //this.SetElemEndChar(child);
                //if (child is DefinitionEleme)
                //   sbCode.AppendLine(";");
            }
        }

        public override string ToString() {
            if (_beleme == null)
                return string.Empty;

            //if (_beleme.Childs.IsNull())
            //    return string.Empty;

            //this.ForChild(this._beleme);

            return this._beleme.ToCode();
        }

        /// <summary>
        /// 设置结尾字符
        /// </summary>
        /// <param name="b"></param>
        public void SetElemEndChar(BEleme b) {
            //if (b is SMElem)
            //   this.sbCode.Append((b as SMElem).EndChar);
        }
    }
}
