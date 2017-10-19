using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser {
    public class Span {

        #region 初始化

        /// <summary>
        /// 基本解析
        /// </summary>
        internal BParser PBParser;

        /// <summary>
        /// 当前元素
        /// </summary>
        internal BEleme NEleme;


        public Span(BParser parser, BEleme ele) {
            this.PBParser = parser;
            this.NEleme = ele;
        }

        public Span(Span span)
            : this(span.PBParser, span.NEleme) {

        }

        public virtual void Init() { }

        #endregion

        #region 错误

        /// <summary>
        /// 输出错误
        /// </summary>
        /// <param name="msg"></param>
        public void Error(string msg = null) {
            this.PBParser.Error(msg ?? "无效字符");
        }

        #endregion

        #region 坐标

        /// <summary>
        /// 上个坐标
        /// </summary>
        public CPoint UpPoint { get; set; }

        /// <summary>
        /// 返回的坐标
        /// </summary>
        public CPoint ResutPoint { get; set; }


        /// <summary>
        /// 添加坐标
        /// </summary>
        public void SetUpPoint() {
            this.UpPoint = new CPoint(this.PBParser.Point);
        }

        /// <summary>
        /// 重置到上个坐标
        /// </summary>
        public void ResetUpPoint() {
            this.PBParser.Point.X = this.UpPoint.X;
            this.PBParser.Point.Y = this.UpPoint.Y;
            if (this.ResutPoint != null) {
                this.ResutPoint.X = this.UpPoint.X;
                this.ResutPoint.Y = this.UpPoint.Y;
            }            
        }

        /// <summary>
        /// 设置返回的坐标
        /// </summary>
        /// <param name="point"></param>
        public void GetResultPoint(CPoint point) {
            this.ResutPoint = point;
        }

        #endregion

        #region 获取字符
        /// <summary>
        /// 获取的当前字符
        /// </summary>
        protected char _Char;
        /// <summary>
        /// 获取的当前标签
        /// </summary>
        protected string _Tag;

        /// <summary>
        /// 获取下一个字符串， 允许出现空格或者回车字符
        /// </summary>
        /// <param name="isEnter"></param>
        public void GetChar(bool isEnter = true) {
            this.GetChar(isEnter, Tools.SpaceChars);
        }

        /// <summary>
        /// 获取下一个字符串
        /// </summary>
        /// <param name="isEnter"></param>
        /// <param name="allowChar"></param>
        public void GetChar(bool isEnter, params char[] allowChar) {
            this._Char = this.PBParser.GetNextChar(isEnter, allowChar);
        }

        /// <summary>
        /// 获取标签 获取之前往前退一格
        /// </summary>
        /// <param name="isEnter"></param>
        public void GetTagForBack(bool isEnter = true) {
            this._Tag = this.PBParser.GetTag(true, isEnter, Tools.SpaceChars);
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="isEnter"></param>
        public void GetTag(bool isEnter = true) {
            this._Tag = this.PBParser.GetTag(false, isEnter, Tools.SpaceChars);
        }

        public void GetTagNotNull(bool isEnter = true) {
            this.GetTag();
            if (string.IsNullOrEmpty(this._Tag))
                this.Error();
        }


        #endregion

        #region 验证

        protected bool IsAddOrReduce {
            get {
                return this._Char == '+' || this._Char == '-';
            }
        }

        /// <summary>
        /// 是否换行
        /// </summary>
        /// <returns></returns>
        protected bool IsEnter {
            get { return this.UpPoint.Y != this.PBParser.Point.Y; }
        }

        /// <summary>
        /// 验证变量是否有效
        /// </summary>
        /// <param name="tag"></param>
        protected void ValidVar(string tag) {
            this.PBParser.ValidVar(tag);
        }

        /// <summary>
        /// 验证变量是否有效
        /// </summary>
        protected void ValidVar() {
            this.PBParser.ValidVar(this._Tag);
        }

        /// <summary>
        /// 验证字符
        /// </summary>
        /// <param name="vc"></param>
        public void ValidChar(char vc) {
            this.GetChar();
            if (this._Char != vc)
                this.Error();
        }

        public bool VaildatePChar(string chars) {
            return this.PBParser.POperate.VaildatePChar(chars);
        }

        #endregion

        #region 节点
        /// <summary>
        /// 验证名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BEleme ValidateNameExists(string name, bool isFatherFind = true) {
            return this.PBParser.POperate.ValidateNameExists(this.NEleme, name, isFatherFind);
        }

        /// <summary>
        /// 将节点添加到父类节点中
        /// </summary>
        /// <param name="node"></param>
        public void AddFather(BEleme node) {
            this.AddFather(this.NEleme, node);
        }

        /// <summary>
        /// 将节点添加到父类节点中
        /// </summary>
        /// <param name="father"></param>
        /// <param name="node"></param>
        public void AddFather(BEleme father, BEleme node) {
            this.PBParser.POperate.AddChild(father, node);
        }

        /// <summary>
        /// 添加包含文件
        /// </summary>
        /// <param name="url"></param>
        public void AddIncludeFile(string url) {
            this.PBParser.POperate.AddIncludeFile(url);
        }

        

        /// <summary>
        /// 添加被引用的变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        public void AddIncludeVar(string name, BEleme eleme, string fileName) {
            this.PBParser.POperate.AddIncludeVar(name, eleme, fileName);
        }
        #endregion

        #region 注释
        /// <summary>
        /// 清空行注释
        /// </summary>
        public void ClearLineAnnotation() {
            this.PBParser.POperate.ClearLineAnnotation();
        }

        /// <summary>
        /// 分析注释
        /// </summary>
        /// <param name="eleme"></param>
        public  void AnalysisAnnotation(BEleme eleme) {
            this.PBParser.POperate.AnalysisAnnotation(eleme);
        }

        #endregion
    }
}
