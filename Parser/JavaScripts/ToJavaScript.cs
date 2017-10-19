using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.ParseLanguage.Element;
using XZ.ParseLanguage.Element.JavaScirpts;
using XZ.ParseLanguage.Entity;
using XZ.ParseLanguage.interfaces;
using XZ.ParseLanguage.Utils;

namespace XZ.ParseLanguage.Parser.JavaScripts {
    public static class ToJavaScript {
        /// <summary>
        /// 获取别名
        /// </summary>
        /// <param name="eleme"></param>
        /// <returns></returns>
        public static string GetAliasName(this BEleme eleme) {
            if (!string.IsNullOrEmpty(eleme.AliasName))
                return eleme.AliasName;
            else
                return eleme.Name;
        }

        public static void ElemeAllToString(this BEleme beleme, StringBuilder sbCode) {
            if (beleme == null || beleme.Childs.IsNull())
                return;

            var charaShow = false;
            if (beleme is JValueEleme) {
                var jv = beleme as JValueEleme;
                if (jv.Characteristic != null) {
                    charaShow = true;
                    sbCode.Append("__" + jv.Characteristic.GetMethodName() + "__(");
                }
            }
            foreach (var child in beleme.Childs) {
                sbCode.Append(child.ToCode());
                if (beleme is RangeEleme || beleme is IHChild || child is IHChild)
                    continue;


                child.ElemeAllToString(sbCode);
            }
            if (charaShow)
                sbCode.Append(")");
        }

        /// <summary>
        /// 获取方法名称
        /// </summary>
        /// <param name="chara"></param>
        /// <returns></returns>
        public static string GetMethodName(this CharacteristicInfo chara) {
            var mName = chara.PBAType.ToString();
            switch (chara.Value) {
                case "++":
                    return mName + "Adds";
                case "--":
                    return mName + "Subs";
                case "+":
                    return mName + "Add";
                case "-":
                    return mName + "Sub";
                default:
                    throw new Exception("无效字符串");
            }
        }

    }
}
