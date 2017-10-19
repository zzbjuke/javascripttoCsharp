using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class ForSpan : Span {
        #region 构造
        public ForSpan(Span span)
            : base(span) { }

        public ForSpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }

        #endregion
        private BEleme pFEleme;
        public override void Init() {
            this.ValidChar('(');
            if (!this.Foreach())
                this.For();

            this.ValidChar(')');
            this.ValidChar('{');
            this.AddFather(pFEleme);
            new JavaSciptSpan(this.PBParser, pFEleme).Init();
            this.ValidChar('}');

        }

        private JForEleme pForEleme;
        public void For() {
            pFEleme = new JForEleme();
            pForEleme = pFEleme as JForEleme;

            #region 定义参数
            this.SetUpPoint();
            this.GetChar();
            if (this._Char != ';') {
                this.ResetUpPoint();
                this.GetTag();
                var valueS = new VarSpan(this);
                if (this._Tag != "var") {
                    var exists = this.ValidateNameExists(this._Tag);
                    if (exists != null) {
                        valueS.IsDef = true;
                        valueS.VarName = exists.GetAliasName();
                    }
                    else
                        valueS.VarName = this._Tag;
                }
                valueS.Init();
            }

            #endregion

            #region Statement2
            this.SetUpPoint();
            this.GetChar();
            if (this._Char != ';') {
                this.ResetUpPoint();
                pForEleme.Statement2 = new ValueEleme();
                pForEleme.Statement2.Father = this.NEleme;
                var valueE = new ValueEleme();
                this.AddFather(pForEleme.Statement2, valueE);
                var valueS = new ValueSpan(this.PBParser, valueE);
                valueS.Init();
            }
            #endregion

            #region where
            this.SetUpPoint();
            this.GetChar();
            if (this._Char != ';') {
                this.ResetUpPoint();
                pForEleme.Where = new ValueEleme();
                pForEleme.Where.Father = this.NEleme;
                var valueE = new ValueEleme();
                this.AddFather(pForEleme.Where, valueE);
                var valueS = new ValueSpan(this.PBParser, valueE);
                valueS.Init();
            }
            #endregion


        }

        private JForeachEleme pForeachEleme;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Foreach() {
            this.SetUpPoint();
            string varName = string.Empty;
            this.GetTag();
            if (this._Tag == "var") {
                this.GetTagNotNull();
                varName = this._Tag;
            }
            else
                varName = this._Tag;

            this.GetTag();
            if (this._Tag != "in") {
                this.ResetUpPoint();
                return false;
            }

            pFEleme = new JForeachEleme();
            pFEleme.Father = this.NEleme;
            pForeachEleme = pFEleme as JForeachEleme;
            pForeachEleme.Name = varName + "_" + this.PBParser.VarIndexCount++ + "_";

            var def = new JDefinitionEleme();
            def.SetPoint(this.PBParser.Point);
            def.Name = varName;
            var exists = this.ValidateNameExists(varName);
            if (exists != null) {
                def.Name = exists.GetAliasName();
                def.PVariableType = Entity.EVariableType.SetValue;
            }
            this.AddFather(def, new JConstEleme(pForeachEleme.Name, Entity.EValueType.Const));
            this.AddFather(pForeachEleme, def);

            var vE = new ValueEleme();
            vE.Father = this.NEleme;
            var vS = new ValueSpan(this.PBParser, vE);
            vS.Init();
            pForeachEleme.Where = vE;
            return true;
        }
    }
}
