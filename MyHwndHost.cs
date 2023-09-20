using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace TestHwndHost
{
  public class MyHwndHost : HwndHost
  {
    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
      Process process = (Process)DataContext;

      Win32Native.SetWindowLong(process.MainWindowHandle, Win32Native.GWL_STYLE,
       new IntPtr((Win32Native.GetWindowLong(process.MainWindowHandle, Win32Native.GWL_STYLE)
        | Win32Native.WS_CHILD) & ~Win32Native.WS_CAPTION));

      Win32Native.SetParent(process.MainWindowHandle, hwndParent.Handle);

      return new HandleRef(this, process.MainWindowHandle);
    }
    protected override void DestroyWindowCore(HandleRef hwnd)
    {
      Win32Native.DestroyWindow(hwnd.Handle);
    }
  }
}
