#if NET461
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OYMLCN.WPF
{
    /// <summary>
    /// 界面辅助方法
    /// </summary>
    public static class VisualHepler
    {

        /// <summary>
        /// 获取元素页面截图
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="saveBmpInDesktop"></param>
        /// <returns></returns>
        public static Bitmap RenderVisaulToBitmap(Visual visual, bool saveBmpInDesktop = false)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(visual);
            var p0 = visual.PointToScreen(bounds.TopLeft);
            var p1 = new System.Drawing.Point((int)p0.X, (int)p0.Y);

            var rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Default);
            rtb.Render(visual);
            var stream = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);
            if (saveBmpInDesktop)
                stream.WriteToFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Guid.NewGuid() + ".bmp"));
            return new Bitmap(stream);
        }
        /// <summary>
        /// 获取元素屏幕截图
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="saveBmpInDesktop"></param>
        /// <returns></returns>
        public static Bitmap RenderVisaulScreenToBitmap(Visual visual, bool saveBmpInDesktop = false)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(visual);
            var p0 = visual.PointToScreen(bounds.TopLeft);
            var p1 = new System.Drawing.Point((int)p0.X, (int)p0.Y);

            Bitmap image = new Bitmap((int)bounds.Width, (int)bounds.Height);
            Graphics imgGraphics = Graphics.FromImage(image);
            imgGraphics.CopyFromScreen(p1.X, p1.Y, 0, 0, new System.Drawing.Size((int)bounds.Width, (int)bounds.Height));
            if (saveBmpInDesktop)
                image.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Guid.NewGuid() + ".bmp"), ImageFormat.Bmp);
            return image;
        }

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