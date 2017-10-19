using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class DictionarySpan : Span {
        public DictionarySpan(Span span)
            : base(span) { }
        public DictionarySpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }

        private JDictionaryEleme pDictionaryEleme;
        public override void Init() {
            this.pDictionaryEleme = this.NEleme as JDictionaryEleme;
            this.GetChar();
            if (this._Char == '}') { }
            else if (Tools.StrStartChars.Contains(this._Char) || Tools.HasVarHead(this._Char) || char.IsNumber(this._Char)) {
                if (this.pDictionaryEleme.Dict == null)
                    this.pDictionaryEleme.Dict = new Dictionary<string, BEleme>();
                AddKeyValue();
            }
            else
                this.Error();
        }

        private void AddKeyValue() {
            if (Tools.StrStartChars.Contains(this._Char))
                this._Tag = this.GetString(this._Char);
            else
                this._Tag = this.PBParser.GetTag(true, false);

            this.ValidChar(':');
            var vElem = new JValueEleme();
            vElem.Father = this.pDictionaryEleme;
            var vSpan = new ValueSpan(this.PBParser, vElem);
            vSpan.Init();
            this.pDictionaryEleme.Dict.Add(this._Tag, vElem);
            this.GetChar();
            switch (this._Char) { 
                case ',':
                    this.GetChar();
                    this.AddKeyValue();
                    break;
                case '}':
                    break;
                default:
                    this.Error();
                    break;
            }


        }
    }
}
