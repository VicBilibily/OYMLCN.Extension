namespace OYMLCN.Extensions
{
    public static partial class SystemTypeExtension
    {
        /// <summary>
        /// CharToInt32
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static int ToInt32(this char ch) => ch;
        /// <summary>
        /// Int32ToChar
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>s
        public static char ToChar(this int i) => (char)i;

    }
}
