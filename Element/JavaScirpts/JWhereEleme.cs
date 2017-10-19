using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Element.JavaScirpts {
    public class JWhereEleme : BEleme {

        public ValueEleme Where { get; set; }

        public List<JDefinitionEleme> DefPars { get; set; }
    }
}
