using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Entity;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JConstEleme : ConstEleme {
        public JConstEleme(string value, EValueType type) {
            this.Value = value;
            this.PValueType = type;
        }


        public override string ToCode() {
            switch (PValueType) {
                case EValueType.String:
                    return "_U_.NewString(\"" + Value + "\")";
                case EValueType.SimString:
                    return "_U_.NewString(" + Value + ")";
                case EValueType.Number:
                    return "_U_.NewNumber(" + Value + ")";
                case EValueType.Bool:
                    return "_U_.NewBoolean(" + Value + ")";
                case EValueType.Null:
                    return "_U_.Null()";
                case EValueType.Const:
                    return Value;
                default:
                    return "_U_.Undefined()";
            }
        }

    }
}
