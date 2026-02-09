using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using AacV1.Core;

namespace AacV1.Services;

public class PcControlService
{
    private readonly Logging _logging;

    public PcControlService(Logging logging)
    {
        _logging = logging;
    }

    public void MoveCursor(int dx, int dy)
    {
        var point = GetCursorPosition();
        SetCursorPos(point.X + dx, point.Y + dy);
    }

    public void MoveCursorTo(double x, double y)
    {
        SetCursorPos(x, y);
    }

    public void LeftClick() => MouseClick(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP);
    public void RightClick() => MouseClick(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP);
    public void DoubleClick()
    {
        LeftClick();
        LeftClick();
    }

    public void DragStart() => MouseClick(MOUSEEVENTF_LEFTDOWN);
    public void DragEnd() => MouseClick(MOUSEEVENTF_LEFTUP);

    public void Scroll(int delta)
    {
        var input = new INPUT
        {
            type = INPUT_MOUSE,
            U = new InputUnion
            {
                mi = new MOUSEINPUT
                {
                    dwFlags = MOUSEEVENTF_WHEEL,
                    mouseData = delta
                }
            }
        };
        SendInput(1, new[] { input }, Marshal.SizeOf<INPUT>());
    }

    public void SendKey(params Key[] keys)
    {
        try
        {
            foreach (var key in keys)
            {
                SendKeyInternal(key, true);
            }
            foreach (var key in keys.Reverse())
            {
                SendKeyInternal(key, false);
            }
        }
        catch (Exception ex)
        {
            _logging.Error("キー送信に失敗しました", ex);
        }
    }

    public void AltTab() => SendKey(Key.LeftAlt, Key.Tab);
    public void WinD() => SendKey(Key.LWin, Key.D);
    public void CtrlL() => SendKey(Key.LeftCtrl, Key.L);

    private static void MouseClick(uint flags)
    {
        var input = new INPUT
        {
            type = INPUT_MOUSE,
            U = new InputUnion
            {
                mi = new MOUSEINPUT
                {
                    dwFlags = flags
                }
            }
        };
        SendInput(1, new[] { input }, Marshal.SizeOf<INPUT>());
    }

    private static void SendKeyInternal(Key key, bool down)
    {
        var input = new INPUT
        {
            type = INPUT_KEYBOARD,
            U = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    wVk = (ushort)KeyInterop.VirtualKeyFromKey(key),
                    dwFlags = down ? 0u : KEYEVENTF_KEYUP
                }
            }
        };
        SendInput(1, new[] { input }, Marshal.SizeOf<INPUT>());
    }

    private static Point GetCursorPosition()
    {
        GetCursorPos(out var point);
        return new Point(point.X, point.Y);
    }

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(double x, double y);

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT point);

    [DllImport("user32.dll")]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    private const uint INPUT_MOUSE = 0;
    private const uint INPUT_KEYBOARD = 1;
    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;
    private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
    private const uint MOUSEEVENTF_WHEEL = 0x0800;
    private const uint KEYEVENTF_KEYUP = 0x0002;

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        public uint type;
        public InputUnion U;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }
}
