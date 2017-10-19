using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Entity;

namespace XZ.ParseLanguage.Element {
    public class DefinitionEleme : BEleme {

        public DefinitionEleme() {
            this.PVariableType = EVariableType.Definition;
        }

        public string OperationChar { get; set; }

        public EVariableType PVariableType { get; set; }
    }
}
