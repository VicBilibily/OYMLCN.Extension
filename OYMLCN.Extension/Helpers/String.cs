using System;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 字符串相关辅助
    /// </summary>
    public static class StringHelpers
    {
        #region 生成随机字符
        /// <summary>
        /// 生成随机字符
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="blur">是否包含特殊符号</param>
        /// <param name="onlyNumber">只生成数字字符</param>
        /// <returns></returns>
        public static string RandCode(int length = 6, bool blur = false, bool onlyNumber = false)
        {
            if (length <= 0)
                return "";
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string symbols = "~!@#$%^&*()_-+=[{]}|><,.?/";
            var hash = string.Empty;
            var rand = new Random();

            for (var cont = 0; cont < length; cont++)
                switch (onlyNumber ? 1 : rand.Next(0, 3))
                {
                    case 1:
                        hash = hash + numbers[rand.Next(0, 10)];
                        break;
                    case 2:
                        hash = hash + (blur ? symbols[rand.Next(0, 26)] : letters[rand.Next(0, 26)]);
                        break;
                    default:
                        hash = hash + ((cont % 3 == 0)
                            ? letters[rand.Next(0, 26)].ToString()
                            : (letters[rand.Next(0, 26)]).ToString().ToLower());
                        break;
                }
            return hash;
        }
        /// <summary>
        /// 生成带特殊符号的随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public static string RandBlurCode(int length)
            => RandCode(length, true);
        #endregion

    }
}
