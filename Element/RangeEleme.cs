using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Entity;

namespace XZ.ParseLanguage.Element {
    public class RangeEleme : BEleme {

        public string Sign { get; set; }

        public CPoint EndPoint { get; set; }

        /// <summary>
        /// 定义的节点
        /// </summary>
        public SortedDictionary<string, BEleme> Variables { get; set; }

        public void AddEndPoint(CPoint point) {
            this.EndPoint = new CPoint(point);
        }
    }
}
