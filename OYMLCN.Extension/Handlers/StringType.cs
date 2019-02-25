using OYMLCN.Extensions;
using System;
using System.Net;

namespace OYMLCN.Handlers
{
    /// <summary>
    /// 字符串数据转换为对应类型
    /// </summary>
    public class StringTypeHandler
    {
        private string Str;
        private StringFormatHandler stringFormatHandler;
        internal StringTypeHandler(string str)
        {
            Str = str;
            stringFormatHandler = new StringFormatHandler(str);
        }

        private string Numeric
            => stringFormatHandler.Numeric;
        private string IntegerNumeric
            => stringFormatHandler.IntegerNumeric;

        #region 数字的转换
        /// <summary>
        /// 转换数字字符串为可空SByte类型
        /// </summary>
        public sbyte? NullableSByte
        {
            get
            {
                var str = IntegerNumeric;
                if (str.IsNullOrEmpty())
                    return null;
                return Convert.ToSByte(str);
            }
        }

        /// <summary>
        /// 转换数字字符串为可空Byte类型
        /// </summary>
        public byte? NullableByte
        {
            get
            {
                string str = IntegerNumeric;
                if (str.IsNullOrEmpty())
                    return null;
                return Convert.ToByte(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Int16类型
        /// </summary>
        public short? NullableShort
        {
            get
            {
                string str = IntegerNumeric;
                if (str.IsNullOrEmpty())
                    return null;
                return Convert.ToInt16(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Int32类型
        /// </summary>
        public int? NullableInt
        {
            get
            {
                string str = IntegerNumeric;
                if (str.IsNullOrEmpty())
                    return null;
                return Convert.ToInt32(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Int64类型
        /// </summary>
        public long? NullableLong
        {
            get
            {
                string str = IntegerNumeric;
                if (str.IsNullOrEmpty())
                    return null;
                return Convert.ToInt64(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空BigInteger类型
        /// </summary>
        public System.Numerics.BigInteger? NullableBigInteger
        {
            get
            {
                string str = IntegerNumeric;
                if (str.IsNullOrEmpty())
                    return null;
                return System.Numerics.BigInteger.Parse(str);
            }
        }

        /// <summary>
        /// 转换字符串为可空Single/float类型
        /// </summary>
        public float? NullableFloat
        {
            get
            {
                string str = IntegerNumeric;
                if (str.IsNullOrEmpty())
                    return null;
                return Convert.ToSingle(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Double类型
        /// </summary>
        public double? NullableDouble
        {
            get
            {
                string str = IntegerNumeric;
                if (str.IsNullOrEmpty())
                    return null;
                return Convert.ToDouble(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Decimal类型
        /// </summary>
        public decimal? NullableDecimal
        {
            get
            {
                string str = IntegerNumeric;
                if (str.IsNullOrEmpty())
                    return null;
                return Convert.ToDecimal(str);
            }
        }


        /// <summary>
        /// 转换数字字符串为SByte类型
        /// </summary>
        public sbyte SByte
            => NullableSByte ?? 0;
        /// <summary>
        /// 转换数字字符串为Byte类型
        /// </summary>
        public byte Byte
            => NullableByte ?? 0;
        /// <summary>
        /// 转换字符串为Int16类型
        /// </summary>
        public short Short
            => NullableShort ?? 0;
        /// <summary>
        /// 转换字符串为Int32类型
        /// </summary>
        public int Int
            => NullableInt ?? 0;
        /// <summary>
        /// 转换字符串为Int64类型
        /// </summary>
        public long Long
            => NullableLong ?? 0;
        /// <summary>
        /// 转换字符串为可空BigInteger类型
        /// </summary>
        public System.Numerics.BigInteger BigInteger => NullableBigInteger ?? 0;

        /// <summary>
        /// 转换字符串为Single/float类型
        /// </summary>
        public float Float
            => NullableFloat ?? 0;
        /// <summary>
        /// 转换字符串为Double类型
        /// </summary>
        public double Double
            => NullableDouble ?? 0;
        /// <summary>
        /// 转换字符串为Decimal类型
        /// </summary>
        public decimal Decimal
            => NullableDecimal ?? 0;
        #endregion

        /// <summary>
        /// 转换字符串为Datetime类型
        /// </summary>
        public DateTime Datetime
            => Convert.ToDateTime(Str);
        /// <summary>
        /// 转换字符串为可空Datetime类型，转换失败返回Null
        /// </summary>
        public DateTime? NullableDatetime
        {
            get
            {
                if (Str.IsNullOrWhiteSpace())
                    return null;
                try
                {
                    return Convert.ToDateTime(Str);
                }
                catch
                {
                    return null;
                }
            }
        }

        ///// <summary>
        ///// 转换网页地址为Uri
        ///// </summary>
        //public Uri Uri => new Uri(Str);
        /// <summary>
        /// 转换网页地址为Uri(失败返回null)
        /// </summary>
        public Uri Uri
        {
            get
            {
                try
                {
                    return new Uri(Str);
                }
                catch
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 将字符串转换为Boolean类型（当字符串是“true/1/yes/checked/是/对”(不区分大小写)时返回true）
        /// </summary>
        public bool Boolean
            => Str.IsEqualIgnoreCase(new string[] { "1", "true", "yes", "checked", "是", "对" });
        /// <summary>
        /// 将字符串转换为可空Boolean类型（当字符串中包含“是/对”时返回true）
        /// </summary>
        public bool? NullableBoolean
        {
            get
            {
                if (Str.IsNullOrEmpty())
                    return null;
                return Boolean;
            }
        }


        /// <summary>
        /// 将单个Cookie字符串转换为Cookie类型
        /// </summary>
        public Cookie Cookie
        {
            get
            {
                if (Str.IsNullOrEmpty())
                    return null;
                var index = Str.IndexOf('=');
                return new Cookie(Str.SubString(0, index).Trim(), Str.SubString(++index, int.MaxValue));
            }
        }
        /// <summary>
        /// 将Cookies字符串转换为CookieCollection集合
        /// </summary>
        public CookieCollection ToCookieCollection
        {
            get
            {
                var result = new CookieCollection();
                foreach (var cookie in Str?.SplitBySign(";") ?? new string[0])
                    result.Add(cookie.AsType().Cookie);
                return result;
            }
        }

    }
}
