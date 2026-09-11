#nullable disable
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AutoMouseKeyboard.Services
{
    public static class MouseCaptureService
    {
    public static Task<Point> CaptureAsync()
    {
        var session = new CaptureSession();
        return session.Task;
    }

    private sealed class CaptureSession
    {
        private readonly TaskCompletionSource<Point> _tcs = new TaskCompletionSource<Point>(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly NativeMethods.LowLevelMouseProc _proc;
        private IntPtr _hookHandle;

        internal CaptureSession()
        {
            _proc = HookCallback;
            var moduleHandle = NativeMethods.GetModuleHandle(null);
            _hookHandle = NativeMethods.SetWindowsHookEx(NativeMethods.WH_MOUSE_LL, _proc, moduleHandle, 0);
            if (_hookHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Không thể khởi tạo hook chuột.");
            }

            _tcs.Task.ContinueWith(_ => Dispose(), TaskScheduler.Default);
        }

        internal Task<Point> Task => _tcs.Task;

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && NativeMethods.IsMouseDownMessage(wParam))
            {
                var info = Marshal.PtrToStructure<NativeMethods.MSLLHOOKSTRUCT>(lParam);
                var point = new Point(info.pt.x, info.pt.y);
                _tcs.TrySetResult(point);
                Dispose();
                return new IntPtr(1);
            }

            return NativeMethods.CallNextHookEx(_hookHandle, nCode, wParam, lParam);
        }

        private void Dispose()
        {
            if (_hookHandle != IntPtr.Zero)
            {
                NativeMethods.UnhookWindowsHookEx(_hookHandle);
                _hookHandle = IntPtr.Zero;
            }
        }
    }

    private static class NativeMethods
    {
        internal const int WH_MOUSE_LL = 14;
        internal const int WM_LBUTTONDOWN = 0x0201;
        internal const int WM_RBUTTONDOWN = 0x0204;
        internal const int WM_MBUTTONDOWN = 0x0207;

        internal static bool IsMouseDownMessage(IntPtr message) =>
            message == (IntPtr)WM_LBUTTONDOWN ||
            message == (IntPtr)WM_RBUTTONDOWN ||
            message == (IntPtr)WM_MBUTTONDOWN;

        internal delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
    }
    }
}

