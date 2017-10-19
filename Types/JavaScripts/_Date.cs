using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Types.JavaScripts {
    /// <summary>
    /// 时间类型
    /// </summary>
    public class _Date : BaseType {
        private DateTime? _value;

        public _Date(DateTime dt) {
            _value = dt;
        }

        public _Date(bool v) { }

        public _Date(params _U_[] args) {
            switch (args.Length) {
                case 0:
                    _value = DateTime.Now;
                    break;
                case 1:
                    this.ChangeDateForStringOrLong(args[0]);
                    break;
                default:
                    this.SetTime(args);
                    break;
            }
        }

        private void SetTime(_U_[] args) {
            int[] arrays = new int[7];
            for (var i = 0; i < args.Length; i++) {
                if (i >= arrays.Length)
                    break;

                var v = args[i].Value;
                if (v is _Number) {
                    var dv = (double)(v as _Number);
                    if (dv > int.MaxValue)
                        return;
                    arrays[i] = (int)dv;
                }
                else
                    return;
            }
            if (arrays[2] == 0)
                arrays[2] = 1;

            string timeStr = string.Format("{0}-{1}-{2} {3}:{4}:{5}:{6}", arrays);

            DateTime outTime;
            if (DateTime.TryParse(timeStr, out outTime))
                this._value = outTime;
        }

        private void ChangeDateForStringOrLong(_U_ par) {
            if (par.Value is _String) {
                bool success = false;
                var dt = par.ToDateTimeForUseVar(out success);
                if (success)
                    this._value = dt;
            }
            else if (par.Value is _Number) {
                var d = (double)(par.Value as _Number);
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                this._value = startTime.AddMilliseconds(d);
            }
        }

        public static implicit operator _Date(DateTime v) {
            return new _Date(v);
        }

        public static implicit operator DateTime(_Date v) {
            return v._value.Value;
        }

        #region 获取值

        /// <summary>
        /// 从 Date 对象返回一个月中的某一天 (1 ~ 31)。
        /// </summary>
        /// <returns></returns>
        public _U_ GetDate() {
            if (this._value == null)
                return _U_.NaN();

            return _U_.NewNumber(this._value.Value.Day);
        }

        /// <summary>
        /// 从 Date 对象返回一周中的某一天 (0 ~ 6)。
        /// </summary>
        /// <returns></returns>
        public _U_ GetDay() {
            if (this._value == null)
                return _U_.NaN();
            return _U_.NewNumber((int)this._value.Value.DayOfWeek);
        }

        /// <summary>
        /// 从 Date 对象返回月份 (1 ~ 12)
        /// </summary>
        /// <returns></returns>
        public _U_ GetMonth() {
            if (this._value == null)
                return _U_.NaN();
            return _U_.NewNumber(this._value.Value.Month);
        }

        /// <summary>
        /// 从 Date 对象以四位数字返回年份
        /// </summary>
        /// <returns></returns>
        public _U_ GetYear() {
            if (this._value == null)
                return _U_.NaN();
            return _U_.NewNumber(this._value.Value.Year);
        }

        /// <summary>
        /// 返回 Date 对象的小时 (0 ~ 23)。
        /// </summary>
        /// <returns></returns>
        public _U_ GetHours() {
            if (this._value == null)
                return _U_.NaN();
            return _U_.NewNumber(this._value.Value.Hour);
        }

        /// <summary>
        /// 返回 Date 对象的分钟 (0 ~ 59)。
        /// </summary>
        /// <returns></returns>
        public _U_ GetMinutes() {
            if (this._value == null)
                return _U_.NaN();
            return _U_.NewNumber(this._value.Value.Minute);
        }
        /// <summary>
        /// 返回 Date 对象的秒数 (0 ~ 59)。
        /// </summary>
        /// <returns></returns>
        public _U_ GetSeconds() {
            if (this._value == null)
                return _U_.NaN();
            return _U_.NewNumber(this._value.Value.Second);
        }
        /// <summary>
        /// 返回 Date 对象的毫秒(0 ~ 999)。
        /// </summary>
        /// <returns></returns>
        public _U_ GetMilliseconds() {
            if (this._value == null)
                return _U_.NaN();
            return _U_.NewNumber(this._value.Value.Millisecond);
        }
        /// <summary>
        /// 返回 1970 年 1 月 1 日至今的毫秒数。
        /// </summary>
        /// <returns></returns>
        public _U_ GetTime() {
            if (this._value == null)
                return _U_.NaN();

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (this._value.Value.Ticks - startTime.Ticks) / 10000;//除10000调整为13位
            return _U_.NewNumber(t);
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public _U_ GetFormat(string format) {
            if (this._value == null)
                return _U_.NewString("");

            return _U_.NewString(_value.Value.ToString(format));
        }

        #endregion


        public _U_ ResetTime(_U_ value, int index) {
            return SetValue<int>(value, (d, array) => {
                array[index] = d;
            });
        }

        private _U_ SetValue<T>(_U_ value, Action<T, int[]> callBack) {
            if (this._value == null)
                return _U_.Undefined();
            try {
                if (value.IsNullNaNUndefined()) {
                    this._value = null;
                    return _U_.Undefined();
                }
                var t = ToObject.ConvertTo<T>(value.ToString());
                int[] array = new int[7];
                array[0] = this._value.Value.Year;
                array[1] = this._value.Value.Month;
                array[2] = this._value.Value.Day;
                array[3] = this._value.Value.Hour;
                array[4] = this._value.Value.Minute;
                array[5] = this._value.Value.Second;
                array[6] = this._value.Value.Millisecond;
                this._value = null;
                callBack(t, array);
                this._value = new DateTime(array[0], array[1], array[2], array[3], array[4], array[5], array[6]);
            }
            catch {
                this._value = null;
            }

            return _U_.Undefined();

        }

        protected override _U_ CallInvoke(string name, _U_ _this, params _U_[] args) {
            switch (name) {
                case "getDate":
                    return GetDate();
                case "getDay":
                    return this.GetDay();
                case "getMonth":
                    return this.GetMonth();
                case "getFullYear":
                case "getYear":
                    return this.GetYear();
                case "getHours":
                    return this.GetHours();
                case "getMinutes":
                    return this.GetMinutes();
                case "getSeconds":
                    return this.GetSeconds();
                case "getMilliseconds":
                    return this.GetMilliseconds();
                case "getTime":
                    return this.GetTime();
                case "getFormat":
                    return this.GetFormat(args.__Get__().ToString());
                case "setFullYear":
                case "setYear":
                    return this.ResetTime(args.__Get__(), 0);
                case "setMonth":
                    return this.ResetTime(args.__Get__(), 1);
                case "setDate":
                    return this.ResetTime(args.__Get__(), 2);
                case "setHours":
                    return this.ResetTime(args.__Get__(), 3);
                case "setMinutes":
                    return this.ResetTime(args.__Get__(), 4);
                case "setSeconds":
                    return this.ResetTime(args.__Get__(), 5);
                case "toString":
                    return _U_.NewString(this.ToString());
                default:
                    return base.CallInvoke(name, _this, args);

            }

        }



        public override string ToString() {
            if (_value == null)
                return "Invalid Date";
            return _value.ToString();
        }


        public override string GetTypeName {
            get {
                return "Date";
            }
        }

    }
}
