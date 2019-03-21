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
        internal StringTypeHandler(string str)
        {
            Str = str;
        }

        #region 数字的转换
        /// <summary>
        /// 转换数字字符串为可空SByte类型
        /// </summary>
        public sbyte? NullableSByte => Str.ConvertToNullableSByte();

        /// <summary>
        /// 转换数字字符串为可空Byte类型
        /// </summary>
        public byte? NullableByte => Str.ConvertToNullableByte();
        /// <summary>
        /// 转换字符串为可空Int16类型
        /// </summary>
        public short? NullableShort => Str.ConvertToNullableShort();
        /// <summary>
        /// 转换字符串为可空Int32类型
        /// </summary>
        public int? NullableInt => Str.ConvertToNullableInt();
        /// <summary>
        /// 转换字符串为可空Int64类型
        /// </summary>
        public long? NullableLong => Str.ConvertToNullableLong();
        /// <summary>
        /// 转换字符串为可空BigInteger类型
        /// </summary>
        public System.Numerics.BigInteger? NullableBigInteger => Str.ConvertToNullableBigInteger();

        /// <summary>
        /// 转换字符串为可空Single/float类型
        /// </summary>
        public float? NullableFloat => Str.ConvertToNullableFloat();
        /// <summary>
        /// 转换字符串为可空Double类型
        /// </summary>
        public double? NullableDouble => Str.ConvertToNullableDouble();
        /// <summary>
        /// 转换字符串为可空Decimal类型
        /// </summary>
        public decimal? NullableDecimal => Str.ConvertToNullableDecimal();


        /// <summary>
        /// 转换数字字符串为SByte类型
        /// </summary>
        public sbyte SByte => Str.ConvertToSByte();
        /// <summary>
        /// 转换数字字符串为Byte类型
        /// </summary>
        public byte Byte => Str.ConvertToByte();
        /// <summary>
        /// 转换字符串为Int16类型
        /// </summary>
        public short Short => Str.ConvertToShort();
        /// <summary>
        /// 转换字符串为Int32类型
        /// </summary>
        public int Int => Str.ConvertToInt();
        /// <summary>
        /// 转换字符串为Int64类型
        /// </summary>
        public long Long => Str.ConvertToLong();
        /// <summary>
        /// 转换字符串为可空BigInteger类型
        /// </summary>
        public System.Numerics.BigInteger BigInteger => Str.ConvertToBigInteger();

        /// <summary>
        /// 转换字符串为Single/float类型
        /// </summary>
        public float Float => Str.ConvertToFloat();
        /// <summary>
        /// 转换字符串为Double类型
        /// </summary>
        public double Double => Str.ConvertToDouble();
        /// <summary>
        /// 转换字符串为Decimal类型
        /// </summary>
        public decimal Decimal => Str.ConvertToDecimal();
        #endregion

        /// <summary>
        /// 转换字符串为Datetime类型
        /// </summary>
        public DateTime Datetime => Str.ConvertToDatetime();
        /// <summary>
        /// 转换字符串为可空Datetime类型，转换失败返回Null
        /// </summary>
        public DateTime? NullableDatetime => Str.ConvertToNullableDatetime();

        ///// <summary>
        ///// 转换网页地址为Uri
        ///// </summary>
        //public Uri Uri => new Uri(Str);
        /// <summary>
        /// 转换网页地址为Uri(失败返回null)
        /// </summary>
        public Uri Uri => Str.ConvertToUri();


        /// <summary>
        /// 将字符串转换为Boolean类型（当字符串是“true/1/yes/checked/是/对”(不区分大小写)时返回true）
        /// </summary>
        public bool Boolean => Str.ConvertToBoolean();
        /// <summary>
        /// 将字符串转换为可空Boolean类型（当字符串中包含“是/对”时返回true）
        /// </summary>
        public bool? NullableBoolean => Str.ConvertToNullableBoolean();


        /// <summary>
        /// 将单个Cookie字符串转换为Cookie类型
        /// </summary>
        public Cookie Cookie => Str.ConvertToCookie();
        /// <summary>
        /// 将Cookies字符串转换为CookieCollection集合
        /// </summary>
        public CookieCollection ToCookieCollection => Str.ConvertToCookieCollection();

    }
}
