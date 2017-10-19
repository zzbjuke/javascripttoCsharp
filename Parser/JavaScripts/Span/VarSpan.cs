using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class VarSpan : Span {
        #region 构造
        public VarSpan(Span span)
            : base(span) {
        }

        public VarSpan(BParser parser, BEleme ele)
            : base(parser, ele) {
        }
        #endregion

        public bool IsFollowUp { get; set; }

        /// <summary>
        /// 是否已经定义 前提是 VarName 不能为null
        /// </summary>
        public bool IsDef { get; set; }
        /// <summary>
        /// 变量名称，是否在执行之前已经获取了变量。这种情况 针对 _var_ = 10;没有写var标签
        /// </summary>
        public string VarName { get; set; }

        public string OperationChar { get; set; }

        public override void Init() {
            if (string.IsNullOrEmpty(VarName)) {
                this.GetTag();
                if (string.IsNullOrEmpty(this._Tag))
                    this.Error("无法找到定义的变量名称");
            }
            else
                this._Tag = this.VarName;

            this.ValidVar();
            var definition = new JDefinitionEleme();
            definition.SetPoint(this.PBParser.Point);
            definition.Name = this._Tag;
            definition.OperationChar = this.OperationChar;
            this.AnalysisAnnotation(definition);
            BEleme vdef = null;
            if (string.IsNullOrEmpty(this.VarName))
                vdef = this.ValidateNameExists(this._Tag, false);
            else if (this.IsDef)
                definition.PVariableType = Entity.EVariableType.SetValue;

            if (vdef != null) {
                definition.PVariableType = Entity.EVariableType.SetValue;
                definition.Name = definition.GetAliasName();
            }
            this.SetUpPoint();
            this.GetChar();
            if (this._Char == '=') {
                this.ResetUpPoint();
                this.ValidChar('=');
                this.AddFather(definition);
                var valueSpan = new ValueSpan(this.PBParser, definition);
                valueSpan.Init();
            }
            else if (this._Char == ',') {
                this.AddFather(definition, new JConstEleme("", Entity.EValueType.Undefined));
                this.AddFather(definition);
                this.ResetUpPoint();
            }
            else {
                if (this._Char != ';') {
                    this.ResetUpPoint();
                    this.GetChar(false);
                    if (this._Char != Tools.LineEndChar)
                        this.Error();

                }
                if (!(string.IsNullOrEmpty(VarName) || this.IsFollowUp))
                    this.Error();

                this.AddFather(definition, new JConstEleme("", Entity.EValueType.Undefined));
                this.AddFather(definition);
            }
            this.SetUpPoint();
            this.GetChar();
            if (this._Char == ',')
                FollowUp();
            else
                this.ResetUpPoint();


        }

        private void FollowUp() {
            this.GetTag();
            if (this._Tag == "var")
                this.Error("意外的标记 var");

            var exists = this.ValidateNameExists(this._Tag);
            var varSpan = new VarSpan(this);
            varSpan.IsFollowUp = true;
            if (exists != null) {
                varSpan.IsDef = true;
                varSpan.VarName = exists.GetAliasName();
            }
            else
                varSpan.VarName = this._Tag;
            varSpan.Init();
        }
    }
}
