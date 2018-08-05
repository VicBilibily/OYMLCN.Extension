#if NET461
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace OYMLCN.WPFExtensions
{
    /// <summary>
    /// ImageExtension
    /// </summary>
    public static class ImageExtension
    {
        /// <summary>
        /// 转换为BitmapImage
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static BitmapImage ToBitmapImage(this byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArray);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }
            return bmp;
        }

        /// <summary>
        /// 转换为Byte[]
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this BitmapImage bmp)
        {
            byte[] byteArray = null;
            try
            {
                Stream sMarket = bmp.StreamSource;
                if (sMarket != null && sMarket.Length > 0)
                {
                    //很重要，因为Position经常位于Stream的末尾，导致下面读取到的长度为0。 
                    sMarket.Position = 0;
                    using (BinaryReader br = new BinaryReader(sMarket))
                        byteArray = br.ReadBytes((int)sMarket.Length);
                }
            }
            catch { }
            return byteArray;
        }
    }
}
#endif