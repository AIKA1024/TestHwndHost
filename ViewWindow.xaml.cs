using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Interop;
using TestHwndHost.Utils;
using TestHwndHost.Views;

namespace TestHwndHost
{
  /// <summary>
  ///     ViewWindow.xaml 的交互逻辑
  /// </summary>
  public partial class ViewWindow : Window
  {
    private readonly Timer timer;

    public ViewWindow(Process process)
    {
      CaptureProcess = process;
      var tempClass = new TempClass { Process = process, Option = PageManager.GetPage<SettingPage>().PgOption };
      DataContext = tempClass;
      timer = new Timer();
      timer.Interval = 1000;
      timer.Elapsed += (s, e) =>
      {
        if (process.HasExited)
          Application.Current.Dispatcher.Invoke(Close);
      };
      timer.Start();

      InitializeComponent();
    }

    public Process CaptureProcess { get; set; }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (!CaptureProcess.HasExited)
      {
        myHwndHost.Dispose();
        foreach (var item in PageManager.GetPage<MonitorPage>().ProgramList)
          if (item.Path == CaptureProcess.MainModule.FileName)
          {
            item.IsCapture = false;
            break;
          }
      }
    }

    public class TempClass
    {
      public Option Option { get; set; }
      public Process Process { get; set; }
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
      Win32Native.SetWindowDisplayAffinity(new WindowInteropHelper(this).Handle, Win32Native.WDA_EXCLUDEFROMCAPTURE);
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
      Win32Native.SetWindowDisplayAffinity(new WindowInteropHelper(this).Handle, Win32Native.WDA_NONE);
    }
  }
}