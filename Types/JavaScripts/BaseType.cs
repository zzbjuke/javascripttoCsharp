using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {

    /// <summary>
    /// 基类
    /// </summary>
    public class BaseType : IEnumerable {

        #region 基本属性

        protected Dictionary<string, _U_> MethodAndPropery = new Dictionary<string, _U_>();

        /// <summary>
        /// 变量名称
        /// </summary>
        public virtual string GetTypeName { get; private set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public virtual VarType VarType { get { return VarType.Value; } }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        internal virtual BaseType Clone() {
            return (BaseType)this.MemberwiseClone();
        }

        /// <summary>
        /// 验证数据类型是否存在
        /// </summary>
        public virtual bool _Exists { get; private set; }

        private string _guid = Guid.NewGuid().ToString();

        public string GUID {
            get { return _guid; }
        }

        #endregion

        #region 调用或设置属性方法

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual _U_ __Get__(string name) {
            _U_ outValue;
            if (MethodAndPropery.TryGetValue(name, out outValue))
                return outValue;
            else
                return GetInvoke(name);
        }

        protected virtual _U_ GetInvoke(string name) {
            return _U_.Undefined();
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual _U_ __Call__(string name, _U_ _this, params _U_[] args) {
            _U_ outValue;
            if (MethodAndPropery.TryGetValue(name, out outValue))
                return outValue.__CallThis__("__C__" + name, _this, args);
            else
                return CallInvoke(name, _this, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual _U_ CallInvoke(string name, _U_ _this, params _U_[] args) {
            return _U_.Undefined();
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual void __Set__(string name, _U_ value) {
            this.MethodAndPropery[name] = value;
        }
        #endregion

        #region 遍历
        public virtual IEnumerator GetEnumerator() {
            foreach (var kv in MethodAndPropery.Keys)
                yield return _U_.NewString(kv);
        }
        #endregion

        #region 索引

        /// <summary>
        /// 获取索引值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual _U_ GetIndexValue(int index) {
            return this.GetIndexValue(index.ToString());
        }

        /// <summary>
        /// 获取索引值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual _U_ GetIndexValue(string key) {
            return this.__Get__(key);
        }


        /// <summary>
        /// 设置索引值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual void SetIndexValue(int index, _U_ value) {
            this.SetIndexValue(index.ToString(), value);
        }

        /// <summary>
        /// 设置字符索引
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void SetIndexValue(string key, _U_ value) {
            this.__Set__(key, value);
        }

        #endregion


    }
}
