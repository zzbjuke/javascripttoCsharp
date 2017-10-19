using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Entity;

namespace XZ.ParseLanguage.Element {

    /// <summary>
    /// 函数节点
    /// </summary>
    public class FunEleme : RangeEleme {

        public FunEleme() {
            this.PVariableType = EVariableType.Definition;
        }

        public EVariableType PVariableType { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public List<string> Arguments { get; set; }
    }
}
