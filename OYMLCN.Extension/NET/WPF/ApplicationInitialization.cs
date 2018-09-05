#if NET461
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace OYMLCN.WPF
{
    /// <summary>
    /// ApplicationInitialization
    /// </summary>
    public static class ApplicationInitialization
    {
        /// <summary>
        /// NativeMethods
        /// </summary>
        protected class NativeMethods
        {
            [DllImport("user32.dll")]
            internal static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll")]
            internal static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);
        }
        internal enum WindowShowStyle : uint
        {
            Hide = 0,
            ShowNormal = 1,
            ShowMinimized = 2,
            ShowMaximized = 3,
            Maximize = 3,
            ShowNormalNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActivate = 7,
            ShowNoActivate = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimized = 11
        }

        private static Semaphore singleInstanceWatcher;

        /// <summary>
        /// 只允许启动一个程序实例
        /// </summary>
        /// <param name="runAction">默认调用方法</param>
        /// <param name="nextAction">若已存在实例的调用方法</param>
        public static void OneInstanceStartup(Action runAction, Action nextAction = null)
        {
            singleInstanceWatcher = new Semaphore(0, 1, Process.GetCurrentProcess().MainModule.ModuleName, out bool createdNew);
            if (createdNew)
                runAction.Invoke();
            else
            {
                var current = Process.GetCurrentProcess();
                foreach (var process in Process.GetProcessesByName(current.ProcessName))
                    if (process.Id != current.Id)
                    {
                        if (nextAction != null)
                            nextAction.Invoke();
                        NativeMethods.SetForegroundWindow(process.MainWindowHandle);
                        NativeMethods.ShowWindow(process.MainWindowHandle, WindowShowStyle.Restore);
                        break;
                    }
                Application.Current.Shutdown();
            }
        }
        /// <summary>
        /// 杀掉程序主线程
        /// </summary>
        public static void KillMainProcess() => Process.GetCurrentProcess().Kill();


        /// <summary>
        /// 程序异常提示标题
        /// </summary>
        public static string UnhandledExceptionTipTitle = "程序异常";
        /// <summary>
        /// 致命异常提示标题
        /// </summary>
        public static string UnhandledExceptionFatalTitle = "致命异常";
        /// <summary>
        /// 无法恢复的异常提示文本
        /// </summary>
        public static string UnhandledExceptionTipFatalText = "发生不可恢复异常，程序即将退出。";
        /// <summary>
        /// 捕获程序异常
        /// Systgem.AppDomain.CurrentDomain.UnhandledException += <see cref="UnhandledException"/>;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.ExceptionObject is Exception exception)
                {
                    var list = new List<Exception>();
                    do
                    {
                        list.Add(exception);
                        exception = exception?.InnerException;
                    }
                    while (exception != null);
                    StringBuilder str = new StringBuilder();
                    //str.AppendLine();
                    list.Reverse();
                    foreach (var item in list)
                        str.AppendLine(item?.Message);

                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        MessageBox.Show(str.ToString(), UnhandledExceptionTipTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            }
            catch (Exception err)
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    MessageBox.Show($"{UnhandledExceptionTipFatalText}/r/n{err.Message}", UnhandledExceptionFatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    KillMainProcess();
                });
            }
        }
        /// <summary>
        /// 注册未处理异常处理
        /// 勿重复注册处理事件
        /// 更改提示信息可直接更改 <see cref="UnhandledExceptionTipTitle"/> 和 <see cref="UnhandledExceptionTipFatalText"/>
        /// </summary>
        /// <param name="showEx">默认弹出错误提示</param>
        public static void RegisterUnhandledException(bool showEx = true)
        {
            if (showEx)
                AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            else
                AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e) { };
        }

        /// <summary>
        /// 捕获所有未处理异常
        /// this.DispatcherUnhandledException += ApplicationInitialization.DispatcherUnhandledException;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            try
            {
                var list = new List<Exception>();
                var exception = e.Exception;
                do
                {
                    list.Add(exception);
                    exception = exception?.InnerException;
                }
                while (exception != null);
                StringBuilder str = new StringBuilder();
                //str.AppendLine();
                list.Reverse();
                foreach (var item in list)
                    str.AppendLine(item?.Message);
                if (list.Any(d => d.GetType() == typeof(TypeInitializationException)))
                {
                    str.AppendLine("发生初始化异常，程序即将退出！");
                    throw new Exception(str.ToString());
                }
                if (e.Exception != null)
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        MessageBox.Show(str.ToString(), UnhandledExceptionTipTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    });
            }
            catch (Exception err)
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    MessageBox.Show($"{err.Message}\r\n{UnhandledExceptionTipFatalText}", UnhandledExceptionFatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    KillMainProcess();
                });
            }
        }

        /// <summary>
        /// 注册未处理异常处理
        /// 勿重复注册处理事件
        /// 更改提示信息可直接更改 <see cref="UnhandledExceptionTipTitle"/> 和 <see cref="UnhandledExceptionTipFatalText"/>
        /// </summary>
        /// <param name="app">WPF主程序</param>
        /// <param name="showEx">默认弹出错误提示</param>
        public static void RegisterDispatcherUnhandledException(this Application app, bool showEx = true)
        {
            if (showEx)
                app.DispatcherUnhandledException += DispatcherUnhandledException;
            else
                app.DispatcherUnhandledException += delegate (object sender, DispatcherUnhandledExceptionEventArgs e)
                {
                    e.Handled = true;
                };
        }

    }
}
#endif