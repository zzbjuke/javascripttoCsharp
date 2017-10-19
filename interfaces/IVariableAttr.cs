using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Entity;

namespace XZ.ParseLanguage.interfaces {
    public interface IVariableAttr {
        /// <summary>
        /// 属性节点
        /// </summary>
        PropertyEleme Property { get; set; }

        /// <summary>
        /// 调用方法节点
        /// </summary>
        MethodEleme Method { get; set; }

        /// <summary>
        /// 
        /// </summary>
        CharacteristicInfo Characteristic { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        BEleme Index { get; set; }
    }
}
