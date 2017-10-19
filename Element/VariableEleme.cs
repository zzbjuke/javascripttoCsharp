using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.interfaces;

namespace XZ.ParseLanguage.Element {
    /// <summary>
    /// 变量
    /// </summary>
    public class VariableEleme : BEleme, IVariableAttr {

        
        /// <summary>
        /// 属性节点
        /// </summary>
        public PropertyEleme Property { get; set; }

        /// <summary>
        /// 调用方法节点
        /// </summary>
        public MethodEleme Method { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CharacteristicInfo Characteristic { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public BEleme Index { get; set; }

    }
}
