using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Element {
    public class MethodEleme : VariableEleme {

        /// <summary>
        /// 参数
        /// </summary>
        public List<BEleme> Arguments { get; set; }
    }
}
