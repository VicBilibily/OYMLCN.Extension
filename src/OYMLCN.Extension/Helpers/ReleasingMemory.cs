using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 内存释放辅助方法
    /// </summary>
    public static class ReleasingMemoryHelper
    {
        [DllImport("kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetProcessWorkingSetSize(IntPtr process, UIntPtr minimumWorkingSetSize, UIntPtr maximumWorkingSetSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="removePages">强迫症选项，将内存页大小设置为0</param>
        public static void ReleaseMemory(bool removePages = false)
        {
            // release any unused pages
            // making the numbers look good in task manager
            // this is totally nonsense in programming
            // but good for those users who care
            // making them happier with their everyday life
            // which is part of user experience
            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();
            if (removePages)
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)0xFFFFFFFF, (UIntPtr)0xFFFFFFFF);
        }
        static bool ReleasingMemoryTimingStarted = false;
        static int ReleasingMemoryTimingSeconds = 60;
        static Thread _ramThread;
        /// <summary>
        /// 开启定时释放内存
        /// </summary>
        public static void StartTimingReleasing(int seconds = 60)
        {
            ReleasingMemoryTimingSeconds = seconds;
            if (ReleasingMemoryTimingStarted)
                return;

            _ramThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    ReleaseMemory(false);
                    Thread.Sleep(ReleasingMemoryTimingSeconds * 1000);
                }
            }))
            {
                IsBackground = true
            };
            _ramThread.Start();
            ReleasingMemoryTimingStarted = true;
        }
        /// <summary>
        /// 停止定时释放内存
        /// </summary>
        public static void StopTimingReleasing()
            => _ramThread?.Abort();
    }
}
