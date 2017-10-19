using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    public class _String : BaseType {
        private string _value = string.Empty;
        public _String(string value) {
            this._value = value;
        }

        

        public static implicit operator _String(string v) {
            return new _String(v);
        }

        public static implicit operator string(_String v) {
            return v._value;
        }

        public override _U_ __Call__(string name, _U_ _this, params _U_[] args) {
            switch (name) {
                case "concat":
                    return this.Concat(args);
                case "toLowerCase":
                    return this.ToLowerCase();
                case "toUpperCase":
                    return this.ToUpperCase();
                case "charAt":
                    return this.CharAt(args.__Get__());
                case "indexOf":
                    return this.IndexOf(args.__Get__(), args.__Get__(1));
                case "replace":
                    return this.Replace(args.__Get__(), args.__Get__(1));
                case "lastIndexOf":
                    return this.LastIndexOf(args.__Get__(), args.__Get__(1));
                case "slice":
                    return this.Slice(args.__Get__(), args.__Get__(1));
                case "substr":
                    return this.Substr(args.__Get__(), args.__Get__(1));
                case "substring":
                    return this.Substring(args.__Get__(), args.__Get__(1));
                case "split":
                    return this.Split(args.__Get__(), args.__Get__(1));
                case "trim":
                    return Trim(args);
                case "contains":
                    return Contains(args.__Get__());
                case "startsWith":
                    return StartsWith(args.__Get__(), args.__Get__(1));
                case "endsWith":
                    return EndsWith(args.__Get__(), args.__Get__(1));
                case "trimStart":
                    return TrimStart(args);
                case "trimEnd":
                    return TrimEnd(args);
                default:
                    return base.__Call__(name, _this, args);
            }
            
        }

        public override _U_ __Get__(string name) {
            switch (name) {
                case "length":
                    return _U_.New<_Number>(this._value == null ? 0 : this._value.Length);
                default:
                    return _U_.Undefined();
            }
        }

        public override void __Set__(string name, _U_ value) {
            //base.__Set__(name, value);
        }
        #region 调用方法

        private _U_ TrimStart(_U_[] uvs) {
            return _U_.NewString(this._value.TrimStart(uvs.ToArrayForUseVar()));
        }
        private _U_ TrimEnd(_U_[] uvs) {
            return _U_.NewString(this._value.TrimEnd(uvs.ToArrayForUseVar()));
        }

        private _U_ Trim(_U_[] uvs) {
            return _U_.NewString(this._value.Trim(uvs.ToArrayForUseVar()));
        }

        private _U_ EndsWith(_U_ uv, _U_ ignoreCase) {
            if (uv.IsNullNaNUndefined())
                return _U_.NewBoolean(false);
            return _U_.NewBoolean(this._value.EndsWith(uv.ToString(), ignoreCase.Value._Exists, null));
        }

        private _U_ StartsWith(_U_ uv, _U_ ignoreCase) {
            if (uv.IsNullNaNUndefined())
                return _U_.NewBoolean(false);
            return _U_.NewBoolean(this._value.StartsWith(uv.ToString(), ignoreCase.Value._Exists, null));
        }

        private _U_ Contains(_U_ uv) {
            if (uv.IsNullNaNUndefined())
                return _U_.NewBoolean(false);

            return _U_.NewBoolean(this._value.Contains(uv.ToString()));
        }

        private _U_ Split(_U_ separator, _U_ howmany) {
            var _array = new _Array();
            if (separator.IsNullNaNUndefined()) {
                _array.__Add__(_U_.NewString(this._value));
                return _U_.New<_Array>(_array);
            }
            string[] array = this._value.ToSplitStrings(separator.ToString());
            string[] newArray = array;
            if (!howmany.IsNullNaNUndefined()) {
                int length = Math.Min(Math.Max(0, howmany.ToInt(0)), array.Length);
                if (length == 0)
                    return _U_.New<_Array>();
                if (length < array.Length) {
                    newArray = new string[length];
                    Array.Copy(array, 0, newArray, 0, length);
                }
            }

            foreach (var a in newArray)
                _array.__Add__(_U_.NewString(a));

            return _U_.New<_Array>(_array);
        }

        private _U_ Substring(_U_ startUV, _U_ endUV) {
            int start = Math.Max(0, startUV.ToInt());
            int end = 0;
            if (endUV.Value is Undefined)
                end = this._value.Length;
            else
                end = endUV.ToInt();

            end = Math.Max(0, end);

            int startValue = Math.Min(start, end);
            int endValue = Math.Max(start, end);
            if (startValue >= this._value.Length)
                return _U_.NewString("");

            endValue = Math.Min(this._value.Length, endValue);

            return _U_.NewString(this._value.Substring(startValue, endValue - startValue));
        }

        private _U_ Substr(_U_ startUV, _U_ endUV) {
            int start = startUV.ToInt();
            if (start < 0)
                start = this._value.Length - Math.Abs(start);

            int length = Math.Max(0, endUV.ToInt(0));
            if (start >= this._value.Length || length < 0)
                return _U_.NewString("");
            if (length == 0)
                length = this._value.Length - start;
            else
                length = Math.Min(length, this._value.Length - start);
            return _U_.NewString(this._value.Substring(start, length));
        }

        private _U_ Slice(_U_ startUV, _U_ endUV) {
            int start = startUV.ToInt();
            int end = 0;
            if (endUV.IsNullNaNUndefined())
                end = this._value.Length;
            else
                end = endUV.ToInt();

            if (start < 0)
                start = this._value.Length - Math.Abs(start);
            if (end < 0)
                end = this._value.Length - Math.Abs(end);
            if (start >= this._value.Length || start > end)
                return _U_.NewString("");

            end = Math.Min(end, this._value.Length);
            return _U_.NewString(this._value.Substring(start, end - start));

        }

        private _U_ LastIndexOf(_U_ searchvalue, _U_ fromindex) {
            if (searchvalue.IsNullNaNUndefined())
                return _U_.NewNumber(-1);

            int index = -1;
            if (!fromindex.IsNullNaNUndefined())
                index = fromindex.ToInt(-1);

            string value = searchvalue.ToString();
            //if (index > this._value.Length - 1)
            //    return _U_.NewNumber(-1);

            if (index >= 0 && index < this._value.Length)
                return _U_.NewNumber(this._value.LastIndexOf(value, index));
            else
                return _U_.NewNumber(this._value.LastIndexOf(value));

        }

        private _U_ Replace(_U_ substr, _U_ replacement) {
            if (!(substr.Value is _String))
                return _U_.NewString(this._value);

            if (replacement.IsNullNaNUndefined())
                return _U_.NewString(this._value);

            return _U_.NewString(this._value.Replace(substr.ToString(), replacement.ToString()));
        }

        private _U_ IndexOf(_U_ searchvalue, _U_ fromindex) {
            if (searchvalue.IsNullNaNUndefined())
                return _U_.NewNumber(-1);

            int index = -1;
            if (!fromindex.IsNullNaNUndefined())
                index = fromindex.ToInt(-1);

            string value = searchvalue.ToString();
            if (index > this._value.Length - 1)
                return _U_.NewNumber(-1);

            return _U_.NewNumber(this._value.IndexOf(value, Math.Max(0, index)));

        }

        private _U_ CharAt(_U_ index) {
            var i = index.ToInt(0);
            if (i < this._value.Length)
                return _U_.NewString(this._value[i].ToString());

            return _U_.NewString("");
        }

        private _U_ Concat(params _U_[] args) {
            StringBuilder sb = new StringBuilder();
            sb.Append(this._value);
            foreach (var a in args)
                sb.Append(a.Value.ToString());

            return _U_.New<_String>(sb.ToString());
        }

        public _U_ ToLowerCase() {
            return _U_.New<_String>(this._value.ToLower());
        }

        public _U_ ToUpperCase() {
            return _U_.New<_String>(this._value.ToUpper());
        }

        #endregion

        public override bool _Exists {
            get {
                return !string.IsNullOrEmpty(this._value);
            }
        }

        public override string GetTypeName {
            get {
                return "string";
            }
        }

        public override string ToString() {
            return _value;
        }

        public override bool Equals(object obj) {
            if (obj is _String) {
                if ((obj as _String)._value == _value)
                    return true;
            }
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
