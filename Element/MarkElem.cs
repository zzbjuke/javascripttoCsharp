using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Element {
    public class MarkElem :BEleme{
        public MarkElem(string value) {
            this.Value = value;
        }
        public string Value { get; set; }

        public override string ToCode() {
            return this.Value ?? string.Empty;
        }
    }
}
