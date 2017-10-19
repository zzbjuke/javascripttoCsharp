using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class TrySpan : Span {
        public TrySpan(Span span) : base(span) { }

        private JTryEleme pTryEleme;
        public override void Init() {
            this.ValidChar('{');

            this.pTryEleme = new JTryEleme();
            this.AddFather(this.pTryEleme);

            new JavaSciptSpan(this.PBParser, this.pTryEleme).Init();
            this.ValidChar('}');
            this.GetTag();
            switch (this._Tag) {
                case "catch":
                    this.Catch();
                    break;
                case "finally":
                    this.Finally();
                    break;
                default:
                    this.Error("Missing catch or finally after try");
                    break;
            }
        }

        private void Catch() {
            var catchE = new JCatchEleme();
            catchE.Father = this.pTryEleme;
            this.pTryEleme.CatchValue = catchE;

            this.GetChar();
            if (this._Char == '(') {
                this.GetTagNotNull();
                catchE.Name = this._Tag + "_" + this.PBParser.VarIndexCount++ + "_";
                var def = new JDefinitionEleme();
                def.Name = this._Tag;
                var exists = this.ValidateNameExists(this._Tag);
                if (exists != null) {
                    def.Name = exists.GetAliasName();
                    def.PVariableType = Entity.EVariableType.SetValue;
                }
                this.AddFather(def, new JConstEleme(catchE.Name + ".Message", Entity.EValueType.SimString));
                this.AddFather(catchE, def);
                this.ValidChar(')');
                this.GetChar();
            }
            if (this._Char != '{')
                this.Error();
            new JavaSciptSpan(this.PBParser, catchE).Init();
            this.ValidChar('}');
            this.SetUpPoint();
            this.GetTag();
            if (this._Tag == "finally")
                this.Finally();
            else
                this.ResetUpPoint();
        }

        private void Finally() {
            var finallyE = new BEleme();
            finallyE.Father = this.pTryEleme;
            this.pTryEleme.FinallyValue = finallyE;
            this.ValidChar('{');
            new JavaSciptSpan(this.PBParser, this.pTryEleme).Init();
            this.ValidChar('}');
        }
    }
}
