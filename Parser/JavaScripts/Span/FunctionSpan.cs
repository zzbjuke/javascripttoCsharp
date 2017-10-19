using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class FunctionSpan : Span {
        #region 构造
        public FunctionSpan(Span span)
            : base(span) { }

        public FunctionSpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }
        #endregion

        /// <summary>
        /// 是否验证名称
        /// </summary>
        public bool IsValidName = true;

        private JFunEleme pFunEleme;

        public override void Init() {
            pFunEleme = new JFunEleme();
            pFunEleme.PVariableType = EVariableType.SetValue;
            pFunEleme.SetPoint(this.PBParser.Point);
            if (this.IsValidName) {
                this.GetTag();
                if (this._Tag == null && this.IsValidName)
                    this.Error();
                pFunEleme.PVariableType = EVariableType.Definition;
                this.pFunEleme.Name = this._Tag;
                var exists = this.ValidateNameExists(this._Tag);
                if (exists != null) {
                    pFunEleme.PVariableType = EVariableType.SetValue;
                    this.pFunEleme.Name = exists.GetAliasName();
                }
            }
            pFunEleme.Sign = (this.PBParser.VarIndexCount++).ToString();
            this.AddFather(pFunEleme);
            this.ValidChar('(');
            this.Args();
            this.SetArgs();
            this.ValidChar('{');
            this.AnalysisAnnotation(pFunEleme);
            var jsSpan = new JavaSciptSpan(this.PBParser, pFunEleme);
            jsSpan.Init();
            this.ValidChar('}');

            pFunEleme.AddEndPoint(this.PBParser.Point);

        }

        /// <summary>
        /// 设置参数
        /// </summary>
        private void SetArgs() {
            if (this.pFunEleme.Arguments.IsNull())
                return;
            int index = 0;
            foreach (var a in this.pFunEleme.Arguments) {
                var jdef = new JDefinitionEleme();
                jdef.SetPoint(this.PBParser.Point);
                jdef.Name = a;
                jdef.IsFunArg = true;
                jdef.Childs = new List<BEleme>();
                jdef.Childs.Add(new JConstEleme(string.Format("__paramers{0}__.__Get__({1})", this.pFunEleme.Sign, index), EValueType.Const));
                this.AddFather(this.pFunEleme, jdef);
                index++;
            }
        }

        /// <summary>
        /// 参数
        /// </summary>
        private void Args() {
            this.SetUpPoint();
            this.GetChar();
            if (this._Char == ')')
                return;

            this.ResetUpPoint();
            this.pFunEleme.Arguments = new List<string>();
        Start:
            this.GetTag();
            this.ValidVar(this._Tag);
            this.pFunEleme.Arguments.Add(this._Tag);
            this.GetChar();
            switch (this._Char) {
                case ',':
                    goto Start;
                case ')':
                    break;
                default:
                    this.Error();
                    break;
            }
        }

    }
}
