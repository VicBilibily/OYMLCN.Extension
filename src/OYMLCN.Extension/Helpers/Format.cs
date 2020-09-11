using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using OYMLCN.Extensions;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 格式处理辅助
    /// </summary>
    public static class FormatHelper
    {
        /// <summary>
        /// 将枚举类型转换为Key/Value数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumClass"></param>
        /// <returns></returns>
        //[Obsolete("请使用EnumToDictionary<TEnum>(typeof(Enum))，方法将于下一主要版本移除")]
        public static Dictionary<string, T> EnumToKeyValues<T>(T enumClass)
            => EnumToDictionary<T>(typeof(T));

        /// <summary>
        /// 将枚举类型转换为Name/Enum字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string, TEnum> EnumToDictionary<TEnum>(Type enumType)
        {
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("参数基类型并非类型System.Enum");
            var reuslt = new Dictionary<string, TEnum>();
            foreach (TEnum value in Enum.GetValues(enumType))
                reuslt[value.ToString()] = value;
            return reuslt;
        }
        /// <summary>
        /// 将枚举类型转换为Int/Description字典
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<int, string> EnumToIntDescription(Type enumType)
        {
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("参数基类型并非类型System.Enum");
            Array values = Enum.GetValues(enumType);
            string[] names = Enum.GetNames(enumType);
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < values.Length; i++)
            {
                FieldInfo field = enumType.GetField(names[i]);
                DescriptionAttribute attribute = field.GetAttribute<DescriptionAttribute>();
                if (attribute == null)
                    dictionary.Add((int)values.GetValue(i), names[i]);
                else
                    dictionary.Add((int)values.GetValue(i), attribute.Description);
            }
            return dictionary;
        }
        

        #region FormatCapacity 字节容量格式化
        /// <summary>
        /// 格式化字节数到更大单位
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesCapacity(ulong bytes)
        {
            const long K = 1024L;
            const long M = K * 1024L;
            const long G = M * 1024L;
            const long T = G * 1024L;
            const long P = T * 1024L;
            const long E = P * 1024L;

            if (bytes >= P * 990)
                return (bytes / (double)E).ToString("F5") + "EiB";
            if (bytes >= T * 990)
                return (bytes / (double)P).ToString("F5") + "PiB";
            if (bytes >= G * 990)
                return (bytes / (double)T).ToString("F5") + "TiB";
            if (bytes >= M * 990)
                return (bytes / (double)G).ToString("F4") + "GiB";
            if (bytes >= M * 100)
                return (bytes / (double)M).ToString("F1") + "MiB";
            if (bytes >= M * 10)
                return (bytes / (double)M).ToString("F2") + "MiB";
            if (bytes >= K * 990)
                return (bytes / (double)M).ToString("F3") + "MiB";
            if (bytes > K * 2)
                return (bytes / (double)K).ToString("F1") + "KiB";
            return bytes + "B";
        }

        /// <summary>
        /// 字节总量转换为KB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToKB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1000), 2, MidpointRounding.AwayFromZero);
        /// <summary>
        /// 字节总量转换为MB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToMB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1000 * 1000), 2, MidpointRounding.AwayFromZero);
        /// <summary>
        /// 字节总量转换为GB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToGB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1000 * 1000 * 1000), 2, MidpointRounding.AwayFromZero);

        /// <summary>
        /// 字节总量转换为KiB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToKiB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1024), 2, MidpointRounding.AwayFromZero);
        /// <summary>
        /// 字节总量转换为MiB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToMiB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1024 * 1024), 2, MidpointRounding.AwayFromZero);
        /// <summary>
        /// 字节总量转换为GiB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToGiB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1024 * 1024 * 1024), 2, MidpointRounding.AwayFromZero);
        #endregion

    }
}
