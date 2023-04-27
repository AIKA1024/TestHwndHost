using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestHwndHost
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }
  }

  public class MyHwndHost : HwndHost
  {
    int hostHeight, hostWidth;
    private Process _process;

    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
      Process process =new Process();
      process.StartInfo.FileName = "notepad.exe";
      process.StartInfo.UseShellExecute = false;
      process.Start();
      process.WaitForInputIdle();

      Win32Native.SetWindowLong(process.MainWindowHandle, Win32Native.GWL_STYLE,
       new IntPtr(Win32Native.GetWindowLong(process.Handle, Win32Native.GWL_STYLE) | Win32Native.WS_CHILD));

      Win32Native.SetParent(process.MainWindowHandle, hwndParent.Handle);

      return new HandleRef(this, process.MainWindowHandle);
    }
    protected override void DestroyWindowCore(HandleRef hwnd)
    {
      Win32Native.DestroyWindow(hwnd.Handle);
    }
  }
}
