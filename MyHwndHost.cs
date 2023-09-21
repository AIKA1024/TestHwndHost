using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using static TestHwndHost.ViewWindow;

namespace TestHwndHost
{
  public class MyHwndHost : HwndHost
  {
    public int OriginalWinStyle { get; set; }
    private Process process;
    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
      process = ((TempClass)DataContext).Process;

      OriginalWinStyle = Win32Native.GetWindowLong(process.MainWindowHandle, Win32Native.GWL_STYLE);
      Win32Native.SetWindowLong(process.MainWindowHandle, Win32Native.GWL_STYLE,
       new IntPtr((Win32Native.GetWindowLong(hwndParent.Handle, Win32Native.GWL_STYLE) | Win32Native.WS_CHILD) & ~Win32Native.WS_CAPTION));

      Win32Native.SetParent(process.MainWindowHandle, hwndParent.Handle);


      return new HandleRef(this, process.MainWindowHandle);
    }
    protected override void DestroyWindowCore(HandleRef hwnd)
    {
      Win32Native.SetWindowLong(process.MainWindowHandle, Win32Native.GWL_STYLE, new IntPtr(OriginalWinStyle));
      Win32Native.SetParent(process.MainWindowHandle, IntPtr.Zero);
      //Win32Native.DestroyWindow(hwnd.Handle);
    }
  }
}
