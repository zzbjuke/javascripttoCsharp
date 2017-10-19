using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    /// <summary>
    /// 变量类型
    /// </summary>
    public enum VarType {
        /// <summary>
        /// 值类型
        /// </summary>
        Value,
        /// <summary>
        /// 引用类型
        /// </summary>
        Object,
        /// <summary>
        /// 表示类型 如undefined
        /// </summary>
        Token
    }
}
