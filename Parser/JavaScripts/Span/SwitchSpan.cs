using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class SwitchSpan : Span {
        #region 构造
        public SwitchSpan(Span span)
            : base(span) { }

        #endregion
        private JSwitchEleme pSwwitchEleme;
        public void Init(string tag) {
            this._Tag = tag;
            switch (tag) {
                case "switch":
                    this.Switch();
                    break;
                case "case":
                    this.Case();
                    break;
                case "default":
                    this.Default();
                    break;
            }
        }

        private void Switch() {
            this.ValidChar('(');
            //this.GetTagNotNull();
            var switchE = new JSwitchEleme();
            this.AddFather(switchE);
            switchE.Sign = (this.PBParser.VarIndexCount++).ToString();

            this.SetUpPoint();
            this.GetChar();
            if (this._Char == ')')
                this.Error();

            this.ResetUpPoint();

            switchE.Where = new ValueEleme();
            switchE.Where.Father = this.NEleme;
            var valueSpan = new ValueSpan(this.PBParser, switchE.Where);
            valueSpan.Init();
            this.ValidChar(')');
            //var exists = this.ValidateNameExists(this._Tag);
            //if (exists == null)
            //    this.Error(this._Tag + " 未定义");
            //else
            //    this._Tag = exists.GetAliasName();
            //switchE.Name = "SWITCH_NAME_"+ SW +"_";
            
            this.ValidChar('{');
            new JavaSciptSpan(this.PBParser, switchE).Init();
            this.ValidChar('}');

        }

        private void Case() {
            if (!(this.NEleme is JSwitchEleme))
                this.Error();
            this.pSwwitchEleme = this.NEleme as JSwitchEleme;
            var caseE = new JCaseEleme();
            caseE.Father = this.NEleme;
            if (this.pSwwitchEleme.Case == null)
                this.pSwwitchEleme.Case = new List<JCaseEleme>();
            this.pSwwitchEleme.Case.Add(caseE);

            caseE.Value = new ValueEleme();
            caseE.Value.Father = this.NEleme;
            var valueSpan = new ValueSpan(this.PBParser, caseE.Value);
            valueSpan.Init();
            this.ValidChar(':');
            var jSpan = new JavaSciptSpan(this.PBParser, caseE);
            jSpan.IsCase = true;
            jSpan.Init();

        }

        private void Default() {
            if (!(this.NEleme is JSwitchEleme))
                this.Error();
            this.pSwwitchEleme = this.NEleme as JSwitchEleme;
            if (this.pSwwitchEleme.Default != null)
                this.Error("在switch语句中多个 default");
            this.pSwwitchEleme.Default = new JCaseEleme();
            this.pSwwitchEleme.Default.Father = this.NEleme;
            this.ValidChar(':');
            new JavaSciptSpan(this.PBParser, this.pSwwitchEleme.Default).Init();


        }
    }
}
