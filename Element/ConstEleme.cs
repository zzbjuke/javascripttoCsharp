using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Entity;

namespace XZ.ParseLanguage.Element {
    /// <summary>
    /// 常量
    /// </summary>
    public class ConstEleme : BEleme {

        /// <summary>
        /// 属性节点
        /// </summary>
        public PropertyEleme Property { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public List<BEleme> Index { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public EValueType PValueType { get; set; }

    }
}
