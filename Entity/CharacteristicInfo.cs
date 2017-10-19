using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Entity {
    public class CharacteristicInfo {
        public EBAType PBAType { get; set; }
        public string Value { get; set; }

        public CharacteristicInfo(EBAType baType, string value) {
            this.PBAType = baType;
            this.Value = value;
        }

        
    }
}
