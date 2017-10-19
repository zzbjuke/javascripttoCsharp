using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.Parser;
using XZ.ParseLanguage.Parser.JavaScripts;
using XZ.ParseLanguage.Utils;
using XZ.ParseLanguage.Element.JavaScirpts;

namespace XZ.ParseLanguage {
    /// <summary>
    /// 分析
    /// </summary>
    public class JavaSciprtAnalysis : AnalysisBase {

        public JavaSciprtAnalysis(string root) : base(root) { }



        public override string FileExtension {
            get {
                return ".js";
            }
        }


        /// <summary>
        /// 执行
        /// </summary>        
        /// <param name="fileName"></param>
        public override void Eexc(string fileName) {
            var bParser = this.CreateParser();
            BEleme b = this.ExecBefore(fileName);
            if (b == null) {
                bParser.ReplaceCharLinePass = this.ReplaceCharLinePass;
                bParser.AnalysisFile((f) => {
                    this.Eexc(f);
                });
                var filePath = Path.Combine(this.Root, fileName);
                b = bParser.Execute(new ParserText() {
                    RootPath = this.Root,
                    Path = filePath,
                    Content = this.GetFileContent(filePath),
                    Type = this.EType
                });
                if (b == null)
                    return;
            }
            AddData(fileName, b, bParser);
        }



        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<string> GetFileContent(string path) {
            //using (FileStream fs = File.OpenRead(path)) {
            //    using (StreamReader sr = new StreamReader(fs, this.ecoding)) {
            //        var list = new List<string>();
            //        while (sr.Peek() > -1) 
            //                list.Add(sr.ReadLine());                    
            //        return list;
            //    }
            //}

            return Tools.ReadFileLins(path, this.ecoding);
        }


        protected override BEleme FileAnalysis(string file) {
            BEleme outEleme;
            if (Datas.TryGetValue(file, out outEleme))
                return outEleme;
            return null;
        }


        protected override void WriteFile(BParser bParser, BEleme b, string fileName) {
            var parser = bParser as JavaScriptCodeParser;
            var csClassName = this.GetFileNameForCs(fileName);
            var fs = new FormatString(b);
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("using System;");
            sbCode.AppendLine("using Sys.JavaScript;");
            sbCode.AppendLine("using XZ.ParseLanguage.Types.JavaScripts;");
            sbCode.AppendLine("namespace " + this.CompileNameSpan + " {");
            sbCode.AppendLine("    public class " + csClassName + " : JsBase {");

            #region 包含文件
            string classVarName = string.Empty;
            if (parser.IncludeFile != null)
                foreach (var inFile in parser.IncludeFile) {
                    classVarName = this.GetFileNameForCs(inFile);
                    sbCode.AppendLine(string.Format("    private {0} __{1}__ = new {0}();", classVarName, classVarName.SetFirstLower()));
                }

            #endregion

            #region 定义全局变量
            var gvars = (b as RangeEleme).Variables;
            if (gvars != null)
                foreach (var g in gvars.Values) {
                    if (g is JDefinitionEleme)
                        (g as JDefinitionEleme).PVariableType = EVariableType.SetValue;
                    else if (g is JFunEleme)
                        (g as JFunEleme).PVariableType = EVariableType.SetValue;

                    sbCode.AppendLine(string.Format("        public _U_ {0} = _U_.Undefined(); ", g.Name));
                }
            #endregion

            sbCode.AppendLine("        public " + csClassName + "() { ");
            sbCode.Append(fs.ToString());
            sbCode.AppendLine("        }");
            sbCode.AppendLine("    }");
            sbCode.AppendLine("}");
            if (this.GetCompileText(fileName, sbCode.ToString()))
                return;
            string path = Path.Combine(this.WriteRoot, csClassName + ".cs");
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (FileStream f = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                StreamWriter sw = new StreamWriter(f);
                sw.Write(sbCode.ToString());
                sw.Dispose();
                sw.Close();
                f.Dispose();
                f.Close();
            }
        }
    }
}
