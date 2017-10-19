using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Entity {
    /// <summary>
    /// 要解析的文本
    /// </summary>
    public class ParserText {

        /// <summary>
        /// 跟目录
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public List<string> Content { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public ELanguage Type { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

    }
}
