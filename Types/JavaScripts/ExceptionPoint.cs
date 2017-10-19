using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class ExceptionPoint : Exception {
        private string message;
        public ExceptionPoint(string msg, int x, int y) {
            message = string.Format("位置，行：{0}，列：{1}。错误内容：{2}", y + 1, x + 1, msg);
        }

        public override string Message {
            get {
                return message;
            }
        }
    }
}
