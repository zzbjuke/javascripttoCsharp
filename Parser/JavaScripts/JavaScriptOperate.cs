using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public class JavaScriptOperate : Operate {

        public JavaScriptOperate(BParser parser) : base(parser) { }

        private Dictionary<string, string> includeVars = new Dictionary<string, string>();

        /// <summary>
        /// 将该节点添加到父节点中
        /// </summary>
        /// <param name="faher"></param>
        /// <param name="node"></param>
        public override void AddChild(BEleme faher, BEleme node) {
            if ((node is JDefinitionEleme && (node as JDefinitionEleme).PVariableType == EVariableType.Definition)
                || (node is JFunEleme && (node as JFunEleme).PVariableType == EVariableType.Definition)
                ) {
                BEleme elme = faher;
                while (elme != null) {
                    if (IsFunOrPage(elme)) {
                        var rangeEleme = elme as RangeEleme;
                        if (rangeEleme.Variables == null)
                            rangeEleme.Variables = new SortedDictionary<string, BEleme>();

                        if (elme is JFunEleme)
                            node.AliasName = node.Name + "_" + this.PBParser.VarIndexCount++ + "_";
                        rangeEleme.Variables.Add(node.Name, node);
                        break;
                    }
                    elme = elme.Father;
                }
            }
            if (faher.Childs == null)
                faher.Childs = new List<BEleme>();

            faher.Childs.Add(node);
            node.Father = faher;
        }

        /// <summary>
        /// 获取名称是否存在
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private BEleme GetNameExists(BEleme node, string name, bool isFatherFind) {
            if (node == null)
                return null;
            BEleme elme = node;
            while (elme != null) {
                if (IsFunOrPage(elme)) {
                    var range = elme as RangeEleme;
                    if (range.Variables != null && range.Variables.ContainsKey(name))
                        return range.Variables[name];
                    if (!isFatherFind)
                        return null;
                }
                elme = elme.Father;
            }

            return null;
        }


        /// <summary>
        /// 验证变量是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override BEleme ValidateNameExists(BEleme node, string name, bool isFatherFind) {
            if (SystemVar.Contains(name))
                return new NoEleme() { Name = name };
            var tag = this.GetNameExists(node, name, isFatherFind);
            if (tag == null) {
                string fileValue = GetIncludeVar(name);
                if (fileValue != null)
                    return new BEleme() {
                        AliasName = fileValue
                    };
            }
            return tag;
        }

        public static bool IsFunOrPage(BEleme e) {
            return e is JFunEleme || e is JPageEleme;
        }

        public override void AddIncludeFile(string file) {
            var bp = this.PBParser as JavaScriptCodeParser;
            if (bp.IncludeFile == null)
                bp.IncludeFile = new HashSet<string>();

            if (!bp.IncludeFile.Contains(file))
                bp.IncludeFile.Add(file);
        }

        public override void AddIncludeVar(string name, BEleme eleme, string fileName) {
            includeVars[name] = fileName;
            var pageEleme = this.PBParser.LumpElem as JPageEleme;
            if (pageEleme.IncludeVar == null)
                pageEleme.IncludeVar = new SortedDictionary<string, BEleme>();


            pageEleme.IncludeVar[name] = eleme;
            includeVars[name] = fileName;
        }

        /// <summary>
        /// 重包含文件中获取变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string GetIncludeVar(string name) {
            string outFile;
            if (includeVars.TryGetValue(name, out outFile))
                return "__" + Tools.GetFileClass(outFile).SetFirstLower() + "__." + name;
            else
                return null;
        }

        #region 解析行


        public override void ReadLineAnnotation(LineAnnInfo info) {
            if (info.Eleme is JDefinitionEleme)
                this.ReadLADefin(info);
            else
                this.ReadLAFun(info.Eleme as JFunEleme, info.Desc);
        }

        /// <summary>
        /// 变量
        /// </summary>
        /// <param name="info"></param>
        private void ReadLADefin(LineAnnInfo info) {
            string[] array = info.Desc.Trim().ToSplitStrings("\r\n");
            var sbDesc = new StringBuilder();
            for (var i = 0; i < array.Length; i++) {
                array[i] = array[i].TrimStart().TrimStart('/');
                if (i == array.Length - 1) {
                    var regex = new Regex(@"^\{(\w+)\}");
                    var m = regex.Match(array[i]);
                    if (m != null && m.Success) {
                        info.Eleme.Type = m.Groups[1].Value;
                        continue;
                    }
                }
                sbDesc.AppendLine(array[i]);
            }
            info.Eleme.Desc = "摘要:\r\n    " + sbDesc.ToString().TrimEnd();
        }

        /// <summary>
        /// 函数
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="desc"></param>
        private void ReadLAFun(JFunEleme fun, string desc) {
            string[] array = desc.Trim().ToSplitStrings("\r\n");
            var sbDesc = new StringBuilder();
            Regex regex;
            Match m;
            for (var i = 0; i < array.Length; i++) {
                array[i] = array[i].TrimStart('/', ' ', '\t');
                regex = new Regex(@"^(\{([_a-zA-Z0-9]+)\})?\s*([_a-zA-Z0-9]+)");
                m = regex.Match(array[i]);
                if (m != null && m.Success) {
                    if (fun.Variables == null)
                        continue;
                    var type = m.Groups[2].Value;
                    var name = m.Groups[3].Value;
                    var pdesc = array[i].Substring(m.Length).TrimStart();

                    BEleme pEleme;
                    if (fun.Variables.TryGetValue(name, out pEleme)) {
                        if ((pEleme as JDefinitionEleme).IsFunArg) {
                            pEleme.Type = type;
                            pEleme.Desc = pdesc;
                        }
                    }

                }
                else if (i == array.Length - 1) {
                    regex = new Regex(@"^\{(\w+)\}");
                    m = regex.Match(array[i]);
                    if (m != null) {
                        fun.Type = m.Groups[1].Value;
                        continue;
                    }
                }
                else
                    sbDesc.AppendLine(array[i]);

            }

            fun.Desc = sbDesc.ToString();
        }

        #endregion

        #region 系统变量

        /// <summary>
        /// 系统变量
        /// </summary>
        public static HashSet<string> SystemVar {
            get {
                return new HashSet<string>() { 
                    "console",
                    "window",
                    "Date"
                };
            }
        }

        #endregion
    }
}
