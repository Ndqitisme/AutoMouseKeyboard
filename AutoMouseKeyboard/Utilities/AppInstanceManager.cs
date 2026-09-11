using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoMouseKeyboard.Utilities
{
    internal static class AppInstanceManager
    {
        private const string MutexName = "Global\\AutoMouseKeyboard_SingleInstance";
        private const string MessageName = "AutoMouseKeyboard_ShowMainWindow";
        private static Mutex? _mutex;

        public static readonly int ShowWindowMessage = NativeMethods.RegisterWindowMessage(MessageName);

        public static bool TryLockInstance()
        {
            try
            {
                _mutex = new Mutex(true, MutexName, out var createdNew);
                if (!createdNew)
                {
                    NotifyExistingInstance();
                    return false;
                }

                return true;
            }
            catch
            {
                // Nếu không chắc chắn, cho phép chạy để người dùng vẫn mở được ứng dụng
                return true;
            }
        }

        public static void NotifyExistingInstance()
        {
            try
            {
                var message = ShowWindowMessage;
                if (message != 0)
                {
                    NativeMethods.PostMessage(new IntPtr(NativeMethods.HWND_BROADCAST), message, IntPtr.Zero, IntPtr.Zero);
                }

                // Gửi trực tiếp tới cửa sổ thuộc tiến trình hiện có để chắc chắn
                NativeMethods.EnumWindows((hWnd, lParam) =>
                {
                    NativeMethods.GetWindowThreadProcessId(hWnd, out var pid);
                    if (pid == Process.GetCurrentProcess().Id)
                    {
                        return true;
                    }

                    if (message != 0)
                    {
                        NativeMethods.PostMessage(hWnd, message, IntPtr.Zero, IntPtr.Zero);
                    }

                    return true;
                }, IntPtr.Zero);
            }
            catch
            {
            }
        }

        public static void Release()
        {
            try
            {
                _mutex?.ReleaseMutex();
                _mutex?.Dispose();
            }
            catch
            {
            }
            finally
            {
                _mutex = null;
            }
        }

        private static class NativeMethods
        {
            internal const int HWND_BROADCAST = 0xffff;

            internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern int RegisterWindowMessage(string lpString);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        }
    }
}

