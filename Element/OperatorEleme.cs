using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Element {
    /// <summary>
    /// 运算符
    /// </summary>
    public class OperatorEleme : BEleme {

        public OperatorEleme(string name) {
            this.Name = name;
        }

        public OperatorEleme(char c) {
            this.Name = c.ToString();
        }

        public OperatorEleme(char c, char c1) {
            this.Name = c.ToString() + c1.ToString();
        }

        public override string ToCode() {
            return " " + Name + " ";
        }
    }
}
