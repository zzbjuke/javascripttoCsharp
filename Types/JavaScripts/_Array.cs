using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class _Array : BaseType {
        private static object _lockIndex = new object();
        private static object _lockvalid = new object();
        private List<_U_> arrays = new List<_U_>();
        private SortedSet<int> validIndex = new SortedSet<int>();
        //private SortedList<>
        //private SortedSet
        //private Dictionary<int, int> indexs = new Dictionary<int, int>();

        /// <summary>
        /// 内部使用
        /// </summary>
        /// <returns></returns>
        public List<_U_> GetValue() {
            return arrays;
        }

        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public _Array __Add__(_U_ value) {
            this.AddValidIndex(this.arrays.Count);
            this.arrays.Add(value);
            return this;
        }

        /// <summary>
        /// 添加有效的索引值
        /// </summary>
        /// <param name="length"></param>
        private void AddValidIndex(int length) {
            lock (_lockvalid) {
                if (!validIndex.Contains(length)) {
                    this.validIndex.Add(length);
                    //validIndex.OrderBy();
                }
            }
        }

        /// <summary>
        /// 设置数组的长度
        /// </summary>
        /// <param name="length"></param>
        public void SetArrayLength(int length) {
            if (length == this.arrays.Count)
                return;

            if (length < this.arrays.Count) {
                this.arrays.RemoveRange(length, this.arrays.Count - length);
                //validIndex.Where(V => V > length);
                validIndex.RemoveWhere(V => V >= length);
            }
            else {
                while (length - this.arrays.Count > 0)
                    this.arrays.Add(_U_.Undefined());
            }
        }


        public override string ToString() {
            return "[" + string.Join(",", arrays) + "]";
        }

        public override bool _Exists {
            get {
                return this.arrays.Count > 0;
            }
        }


        public override void __Set__(string name, _U_ value) {
            switch (name) {
                case "length":
                    int length = 0;
                    if (int.TryParse(value.ToString(), out length))
                        this.SetArrayLength(length);
                    else
                        throw new Exception(value.ToString() + " 不是有效的数组长度");
                    break;
                default:
                    base.__Set__(name, value);
                    break;
            }
        }

        public override string GetTypeName {
            get {
                return "array";
            }
        }

        /// <summary>
        /// 遍历
        /// </summary>
        /// <returns></returns>
        public override System.Collections.IEnumerator GetEnumerator() {
            foreach (var i in validIndex)
                yield return _U_.NewNumber(i);

            foreach (var kv in MethodAndPropery.Keys)
                yield return _U_.NewString(kv);
        }

        protected override _U_ GetInvoke(string name) {
            if (name == "length")
                return _U_.NewNumber(this.arrays.Count);
            return _U_.Undefined();
        }

        #region 索引器

        /// <summary>
        /// 获取索引值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override _U_ GetIndexValue(int index) {
            if (index >= 0 && index < arrays.Count)
                return arrays[index];
            else
                return _U_.Undefined();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public override void SetIndexValue(int index, _U_ value) {
            lock (_lockIndex) {
                AddValidIndex(index);
                if (index < this.arrays.Count)
                    this.arrays[index] = value;
                else {
                    while (index - this.arrays.Count > 0)
                        this.arrays.Add(_U_.Undefined());

                    this.arrays.Add(value);
                }
            }
        }

        public override _U_ GetIndexValue(string key) {
            int index = 0;
            if (int.TryParse(key, out index))
                return GetIndexValue(index);
            return base.GetIndexValue(key);
        }

        public override void SetIndexValue(string key, _U_ value) {
            int index = 0;
            if (int.TryParse(key, out index))
                SetIndexValue(index, value);
            else
                base.SetIndexValue(key, value);
        }

        #endregion
    }
}
