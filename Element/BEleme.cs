using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Entity;

namespace XZ.ParseLanguage.Element {
    public class BEleme {

        /// <summary>
        /// 别名
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取坐标点
        /// </summary>
        public CPoint Point { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; } 

        /// <summary>
        /// 子节点
        /// </summary>
        public List<BEleme> Childs { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 父类节点
        /// </summary>
        public BEleme Father { get; set; }

        /// <summary>
        /// 设置坐标点
        /// </summary>
        /// <param name="point"></param>
        public void SetPoint(CPoint point) {
            this.Point = new CPoint(point);
        }

        /// <summary>
        /// 转化为语言代码
        /// </summary>
        /// <returns></returns>
        public virtual string ToCode() {
            return string.Empty;
        }
    }
}
