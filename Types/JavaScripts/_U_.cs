using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class _U_ {
        public _U_(BaseType value) {
            this.Value = value;
        }

        #region 新建变量

        /// <summary>
        /// 值
        /// </summary>
        public BaseType Value { get; set; }

        public static _U_ New(BaseType value) {
            return new _U_(value);
        }

        public static _U_ New<T>(T v) where T : BaseType {
            return new _U_(v);
        }

        public static _U_ New<T>() where T : BaseType, new() {
            return new _U_(new T());
        }

        public static _U_ NewNumber(double value) {
            return _U_.New<_Number>(value);
        }

        public static _U_ NewBoolean(bool value) {
            return _U_.New<_Bool>(value);
        }

        public static _U_ NewString(string value) {
            return _U_.New<_String>(value);
        }

        #region Undefined Null NaN 没有必要每次创建

        //private static _U_ _initNull = Null();
        private static _U_ _Undefined = new _U_(new Undefined());
        private static _U_ _null = new _U_(new Null());
        private static _U_ _nan = new _U_(new NaN());
        //private static BaseType _undefinedValue = new Undefined();
        //private static BaseType _null = new Null();
        //private static BaseType _nan = new NaN();

        public static _U_ GetNull {
            get {
                return _null;
            }
        }


        /// <summary>
        /// 未定义数据
        /// </summary>
        /// <returns></returns>
        public static _U_ Undefined() {
            return _Undefined;
        }

        /// <summary>
        /// Null 数据
        /// </summary>
        /// <returns></returns>
        public static _U_ Null() {
            //return new _U_(_null);
            return _null;
        }

        /// <summary>
        /// NaN
        /// </summary>
        /// <returns></returns>
        public static _U_ NaN() {
            return _nan;
        }

        #endregion


        #endregion

        #region 	比较运算符

        public static _U_ operator <=(_U_ t1, _U_ t2) {
            var tup = Get_U_Number(t1, t2);
            if (tup == null)
                return _U_.New<_Bool>(false);
            else
                return _U_.New<_Bool>(tup.Item1 <= tup.Item2);
        }

        public static _U_ operator >=(_U_ t1, _U_ t2) {
            var tup = Get_U_Number(t1, t2);
            if (tup == null)
                return _U_.New<_Bool>(false);
            else
                return _U_.New<_Bool>(tup.Item1 >= tup.Item2);
        }

        public static _U_ operator >(_U_ t1, _U_ t2) {
            var tup = Get_U_Number(t1, t2);
            if (tup == null)
                return _U_.New<_Bool>(false);
            else
                return _U_.New<_Bool>(tup.Item1 > tup.Item2);
        }

        public static _U_ operator <(_U_ t1, _U_ t2) {
            var tup = Get_U_Number(t1, t2);
            if (tup == null)
                return _U_.New<_Bool>(false);
            else
                return _U_.New<_Bool>(tup.Item1 < tup.Item2);
        }

        public static _U_ operator ==(_U_ t1, _U_ t2) {
            return _U_.New<_Bool>(t1.Value.Equals(t2.Value));
        }

        public static _U_ operator !=(_U_ t1, _U_ t2) {
            return _U_.New<_Bool>(!t1.Value.Equals(t2.Value));
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            return base.Equals(obj);
        }

        private static Tuple<_Number, _Number> Get_U_Number(_U_ v1, _U_ v2) {
            _Number _number1 = null;
            _Number _number2 = null;
            if (v1.Value is _Number)
                _number1 = (_Number)v1.Value;
            else if (v1.Value is _Bool) {
                if ((_Bool)v1.Value == true)
                    _number1 = new _Number(1);
                else
                    _number1 = new _Number(0);
            }
            if (v2.Value is _Number)
                _number2 = (_Number)v2.Value;
            else if (v2.Value is _Bool) {
                if ((_Bool)v2.Value == true)
                    _number2 = new _Number(1);
                else
                    _number2 = new _Number(0);
            }

            if (_number1 != null && _number2 != null)
                return Tuple.Create(_number1, _number2);
            else
                return null;
        }


        #endregion

        #region 一元运算符

        public static bool operator true(_U_ bt) {
            return bt.Value._Exists;
        }
        public static bool operator false(_U_ bt) {
            return !bt.Value._Exists;
        }

        public static _U_ operator ~(_U_ t) {
            var l = getLong(t, _U_.Undefined());
            return _U_.New<_Number>(~l.Item1);
        }

        public static _U_ operator !(_U_ bt) {
            return _U_.New<_Bool>(!bt.Value._Exists);
        }



        /// <summary>
        /// 是否能转换为double
        /// </summary>
        /// <returns></returns>
        private bool TypeParseDouble() {
            double v = 0;
            if (double.TryParse(this.Value.ToString(), out v))
                return true;
            else
                return false;

        }




        #endregion

        #region 二元运算符

        private static Tuple<long, long> getLong(_U_ t1, _U_ t2) {
            long long1 = 0;
            long long2 = 0;
            if (t1.Value is _Number) {
                double v1 = (_Number)t1.Value;
                if (v1 > long.MaxValue)
                    long1 = long.MaxValue;
                else
                    long1 = (long)Math.Round(v1);
            }
            else if (t1.Value is _Bool) {
                if ((_Bool)t1.Value == true)
                    long1 = 1;
                else
                    long1 = 0;
            }
            if (t2.Value is _Number) {
                double v2 = (_Number)t2.Value;
                if (v2 > long.MaxValue)
                    long2 = long.MaxValue;
                else
                    long2 = (long)Math.Round(v2);
            }
            else if (t2.Value is _Bool) {
                if ((_Bool)t2.Value == true)
                    long2 = 1;
                else
                    long2 = 0;
            }

            return Tuple.Create(long1, long2);
        }

        private static Tuple<int, int> getInt(_U_ t1, _U_ t2) {
            int int1 = 0;
            int int2 = 0;
            if (t1.Value is _Number) {
                double v1 = (_Number)t1.Value;
                if (v1 > int.MaxValue)
                    int1 = int.MaxValue;
            }
            if (t2.Value is _Number) {
                double v2 = (_Number)t2.Value;
                if (v2 > int.MaxValue)
                    int2 = int.MaxValue;
            }

            return Tuple.Create(int1, int2);
        }

        public static _U_ operator |(_U_ t1, _U_ t2) {
            //if (t1.Value is _Bool && t2.Value is _Bool)
            //    return _U_.New<_Bool>((_Bool)t1.Value | (_Bool)t1.Value);

            var l = getLong(t1, t2);
            return _U_.New<_Number>(l.Item1 | l.Item2);
        }
        public static _U_ operator &(_U_ t1, _U_ t2) {
            //if (t1.Value is _Bool && t2.Value is _Bool)
            //    return _U_.New<_Bool>((_Bool)t1.Value & (_Bool)t1.Value);

            var l = getLong(t1, t2);
            return _U_.New<_Number>(l.Item1 & l.Item2);
        }
        public static _U_ operator ^(_U_ t1, _U_ t2) {
            var l = getLong(t1, t2);
            return _U_.New<_Number>(l.Item1 ^ l.Item2);
        }

        //public static _U_ operator ++(_U_ value) {
        //    if (value.Value is _Number) {
        //        double v = (value.Value as _Number);
        //        //return _U_.NewNumber(v + 1);
        //        value.Value = new _Number(v + 1);
        //        return value;
        //    }
        //    else if (value.Value is _String) {
        //        double outValue = 0;
        //        if (double.TryParse(value.ToString(), out outValue))
        //            return _U_.NewNumber(outValue + 1);
        //    }
        //    else if (value.Value is _Bool) {
        //        bool v = value.Value as _Bool;
        //        return v == true ? _U_.NewNumber(2) : _U_.NewNumber(1);
        //    }
        //    value.Value = new NaN();
        //    return _U_.NaN();
        //}

        //public static _U_ operator --(_U_ value) {
        //    if (value.Value is _Number) {
        //        double v = (value.Value as _Number);
        //        return _U_.NewNumber(v + 1);
        //    }
        //    else if (value.Value is _String) {
        //        double outValue = 0;
        //        if (double.TryParse(value.ToString(), out outValue))
        //            return _U_.NewNumber(outValue - 1);
        //    }
        //    else if (value.Value is _Bool) {
        //        bool v = value.Value as _Bool;
        //        return v == true ? _U_.NewNumber(-0) : _U_.NewNumber(-1);
        //    }
        //    value.Value = new NaN();
        //    return _U_.NaN();
        //}

        public static _U_ operator +(_U_ t1, _U_ t2) {
            if (t1.Value is _Number && t2.Value is _Number)
                return _U_.New<_Number>((_Number)t1.Value + (_Number)t2.Value);
            //if (t1.Value is _Number || t2.Value is _Number)
            //    return _U_.NaN();

            return _U_.New<_String>(t1.Value.ToString() + t2.Value.ToString());
        }
        public static _U_ operator -(_U_ t1, _U_ t2) {
            double? v1 = GetDoubleFor_U_Value(t1);
            double? v2 = GetDoubleFor_U_Value(t2);
            if (v1 != null && v2 != null)
                return _U_.New<_Number>(v1 - v2);

            return _U_.New<NaN>();
        }

        //public static _U_ operator -(_U_ value) {
        //    if (value.Value is _Number) {
        //        (value.Value as _Number).SetAction((v) => {
        //            return v * -1;
        //        });
        //        return value;
        //    }
        //    else if (value.Value is _Bool) {
        //        bool v = value.Value as _Bool;
        //        return v == true ? _U_.NewNumber(-1) : _U_.NewNumber(-0);
        //    }
        //    else if (value.Value is _String) {
        //        double outValue = 0;
        //        if (double.TryParse(value.ToString(), out outValue))
        //            return _U_.NewNumber(-outValue);
        //    }
        //    return _U_.NaN();
        //}

        //public static _U_ operator +(_U_ value) {
        //    if (value.Value is _Number)
        //        return value;
        //    else if (value.Value is _Bool) {
        //        bool v = value.Value as _Bool;
        //        return v == true ? _U_.NewNumber(1) : _U_.NewNumber(0);
        //    }
        //    else if (value.Value is _String) {
        //        double outValue = 0;
        //        if (double.TryParse(value.ToString(), out outValue))
        //            return _U_.NewNumber(outValue);
        //    }

        //    return _U_.NaN();


        //}

        public static _U_ operator *(_U_ t1, _U_ t2) {
            double? v1 = GetDoubleFor_U_Value(t1);
            double? v2 = GetDoubleFor_U_Value(t2);
            if (v1 != null && v2 != null)
                return _U_.New<_Number>(v1 * v2);

            return _U_.New<NaN>();
        }
        public static _U_ operator /(_U_ t1, _U_ t2) {
            double? v1 = GetDoubleFor_U_Value(t1);
            double? v2 = GetDoubleFor_U_Value(t2);
            if (v1 != null && v2 != null)
                return _U_.New<_Number>(v1.Value / v2.Value);

            return _U_.New<NaN>();
        }
        public static _U_ operator %(_U_ t1, _U_ t2) {
            double? v1 = GetDoubleFor_U_Value(t1);
            double? v2 = GetDoubleFor_U_Value(t2);
            if (v1 != null && v2 != null)
                return _U_.New<_Number>(v1 % v2);

            return _U_.New<NaN>();
        }

        private static double? GetDoubleFor_U_Value(_U_ u) {
            if (u.Value is _Number)
                return (_Number)u.Value;
            else if (u.Value is _String) {
                double v = 0;
                if (double.TryParse(u.Value.ToString(), out v))
                    return v;
            }
            return null;
        }

        #endregion

        #region 重写
        public override string ToString() {
            return this.Value.ToString();
        }
        #endregion

        #region 调用方法

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public _U_ __Call__(string name, params _U_[] args) {
            //if (name.StartsWith("__C__")) {
            //    if (!(this.Value is _Function)) {
            //        string funName = name.Substring(5);
            //        if (char.IsNumber(name[5])) {
            //            string[] arrayHL = funName.Split('_');
            //            if (arrayHL.Length == 2)
            //                funName = "在行" + arrayHL[0] + "， 列" + arrayHL[1];
            //            else
            //                funName = string.Empty;

            //        }

            //        throw new Exception(funName + " 不是一个有效的函数");
            //    }
            //    return this.Value.__Call__("", this, args);
            //}
            //else
            //    return this.Value.__Call__(name, this, args);

            return __CallThis__(name, this, args);
        }

        public _U_ __CallThis__(string name, _U_ _this, params _U_[] args) {
            if (name.StartsWith("__C__")) {
                if (!(this.Value is _Function)) {
                    string funName = name.Substring(5);
                    if (char.IsNumber(name[5])) {
                        string[] arrayHL = funName.Split('_');
                        if (arrayHL.Length == 2)
                            funName = "在行" + arrayHL[0] + "， 列" + arrayHL[1];
                        else
                            funName = string.Empty;
                    }
                    throw new Exception(funName + " 不是一个有效的函数");
                }
                return this.Value.__Call__("", _this, args);
            }
            else
                return this.Value.__Call__(name, _this, args);
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void __Set__(string name, _U_ value) {
            this.Value.__Set__(name, value);
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public _U_ __Get__(string name) {
            return this.Value.__Get__(name);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public _U_ __New__(string name, params _U_[] args) {
            if (!(this.Value is _Function))
                throw new Exception(name + " 不是构造函数");

            (this.Value as _Function).__Init__(this, args);
            return this;
        }


        #endregion

        #region 索引

        public _U_ this[int index] {
            get {
                return this.Value.GetIndexValue(index);
            }
            set {
                this.Value.SetIndexValue(index, value);
            }
        }

        public _U_ this[string index] {
            get {
                return this.Value.GetIndexValue(index);
            }
            set {
                this.Value.SetIndexValue(index, value);
            }
        }

        public _U_ this[_U_ index] {
            get {
                return this[index.ToString()];
            }
            set { this[index.ToString()] = value; }
        }

        public _U_ this[double index] {
            get {
                return this[index.ToString()];
            }
            set { this[index.ToString()] = value; }
        }

        #endregion
    }
}
