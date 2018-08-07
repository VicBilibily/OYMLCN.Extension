#if NET461
using OYMLCN.Handlers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// BitmapExtensions
    /// </summary>
    public static class BitmapExtensions
    {
        /// <summary>
        /// 将图片Image转换成Byte[]
        /// </summary>
        /// <param name="Image">image对象</param>
        /// <param name="imageFormat">后缀名</param>
        /// <returns></returns>
        public static byte[] ToBytes(this Image Image, ImageFormat imageFormat)
        {
            if (Image.IsNull())
                return null;
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap Bitmap = new Bitmap(Image))
                {
                    Bitmap.Save(ms, imageFormat);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            return data;
        }

        /// <summary>
        /// byte[]转换成Image
        /// </summary>
        /// <param name="byteArrayIn">二进制图片流</param>
        /// <returns>Image</returns>
        public static Image ToImage(this byte[] byteArrayIn)
        {
            if (byteArrayIn.IsNull())
                return null;
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                ms.Flush();
                return returnImage;
            }
        }

        /// <summary>
        /// byte[] 转换 Bitmap
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(this byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        /// 处理Bitmap图像
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapHandler AsBitmapHandler(this Bitmap bitmap) => new BitmapHandler(bitmap);

        /// <summary>
        /// Bitmap转byte[]  
        /// </summary>
        /// <param name="Bitmap"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Bitmap Bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap.Save(ms, Bitmap.RawFormat);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }
    }
}
#endif