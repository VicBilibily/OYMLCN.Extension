using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OYMLCN.Extensions;
using OYMLCN.WPF.Extensions;

namespace OYMLCN.WPF.Heplers
{
    /// <summary>
    /// 界面辅助方法
    /// </summary>
    public static class VisualHeplers
    {
        /// <summary>
        /// 获取元素页面截图
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="saveBmpInDesktop"></param>
        /// <returns></returns>
        public static BitmapImage RenderVisaulToBitmap(Visual visual, bool saveBmpInDesktop = false)
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
            return new Bitmap(stream).ToBytes().ToBitmapImage();
        }
        /// <summary>
        /// 获取元素屏幕截图
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="saveBmpInDesktop"></param>
        /// <returns></returns>
        public static BitmapImage RenderVisaulScreenToBitmap(Visual visual, bool saveBmpInDesktop = false)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(visual);
            var p0 = visual.PointToScreen(bounds.TopLeft);
            var p1 = new System.Drawing.Point((int)p0.X, (int)p0.Y);

            Bitmap image = new Bitmap((int)bounds.Width, (int)bounds.Height);
            Graphics imgGraphics = Graphics.FromImage(image);
            imgGraphics.CopyFromScreen(p1.X, p1.Y, 0, 0, new System.Drawing.Size((int)bounds.Width, (int)bounds.Height));
            if (saveBmpInDesktop)
                image.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Guid.NewGuid() + ".bmp"), ImageFormat.Bmp);
            return image.ToBytes().ToBitmapImage();
        }
    }
}
