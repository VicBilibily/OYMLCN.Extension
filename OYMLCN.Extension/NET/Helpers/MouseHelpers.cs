#if NET461
using System.Runtime.InteropServices;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 鼠标操作
    /// </summary>
    public static class MouseHelpers
    {
        /// <summary>
        /// mouse_event
        /// </summary>
        /// <param name="dwFlags">dwFlags常数</param>
        /// <param name="dx">根据MOUSEEVENTF_ABSOLUTE标志，指定x方向的绝对位置或相对位置</param>
        /// <param name="dy">根据MOUSEEVENTF_ABSOLUTE标志，指定y方向的绝对位置或相对位置</param>
        /// <param name="cButtons"></param>
        /// <param name="dwExtraInfo"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        /// <summary>
        /// 移动鼠标
        /// </summary>
        public const int MOUSEEVENTF_MOVE = 0x0001;
        /// <summary>
        /// 模拟鼠标左键按下
        /// </summary>
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        /// <summary>
        /// 模拟鼠标左键抬起
        /// </summary>
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        /// <summary>
        /// 模拟鼠标右键按下
        /// </summary>
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        /// <summary>
        /// 模拟鼠标右键抬起
        /// </summary>
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;
        /// <summary>
        /// 模拟鼠标中键按下
        /// </summary>
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        /// <summary>
        /// 模拟鼠标中键抬起
        /// </summary>
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        /// <summary>
        /// 标示是否采用绝对坐标
        /// </summary>
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;


        /// <summary>
        /// 移动鼠标
        /// </summary>
        /// <param name="x">X轴像素偏移</param>
        /// <param name="y">Y轴像素偏移</param>
        /// <param name="screenX">屏幕X轴像素量</param>
        /// <param name="screenY">屏幕Y轴像素量</param>
        /// <param name="absolute">屏幕绝对位置</param>
        public static void MouseMove(int x, int y, int screenX = 1024, int screenY = 768, bool absolute = true) =>
            mouse_event(absolute ? (MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE) : MOUSEEVENTF_MOVE, x * 65536 / screenX, y * 65536 / screenY, 0, 0);

        /// <summary>
        /// 鼠标单击
        /// </summary>
        /// <param name="x">X轴像素绝对位置</param>
        /// <param name="y">Y轴像素绝对位置</param>
        /// <param name="screenX">屏幕X轴像素量</param>
        /// <param name="screenY">屏幕Y轴像素量</param> 
        public static void MouseClick(int x = 0, int y = 0, int screenX = 1024, int screenY = 768) =>
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x * 65536 / screenX, y * 65536 / screenY, 0, 0);
        /// <summary>
        /// 鼠标双击
        /// </summary>
        /// <param name="x">X轴像素绝对位置</param>
        /// <param name="y">Y轴像素绝对位置</param>
        /// <param name="screenX">屏幕X轴像素量</param>
        /// <param name="screenY">屏幕Y轴像素量</param>
        public static void MouseDoubleClick(int x = 0, int y = 0, int screenX = 1024, int screenY = 768)
        {
            MouseClick(x, y);
            MouseClick(x, y);
        }

        /// <summary>
        /// 鼠标右键
        /// </summary>
        /// <param name="x">X轴像素绝对位置</param>
        /// <param name="y">Y轴像素绝对位置</param>
        /// <param name="screenX">屏幕X轴像素量</param>
        /// <param name="screenY">屏幕Y轴像素量</param>
        public static void MouseRightClick(int x = 0, int y = 0, int screenX = 1024, int screenY = 768) =>
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, x * 65536 / screenX, y * 65536 / screenY, 0, 0);


    }
}
#endif
