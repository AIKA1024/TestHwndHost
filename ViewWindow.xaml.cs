using System.Diagnostics;
using System.Windows;
using TestHwndHost.Utils;
using TestHwndHost.Views;

namespace TestHwndHost
{
  /// <summary>
  /// ViewWindow.xaml 的交互逻辑
  /// </summary>
  public partial class ViewWindow : Window
  {
    System.Timers.Timer timer;
    public Process CaptureProcess { get; set; }
    public ViewWindow(Process process)
    {
      CaptureProcess = process;
      DataContext = new TempClass() { Process = process, Option = PageManager.GetPage<SettingPage>().PgOption };
      timer = new System.Timers.Timer();
      timer.Interval = 1000;
      timer.Elapsed += (s, e) =>
      {
        if (process.HasExited)
          Application.Current.Dispatcher.Invoke(Close);
      };
      timer.Start();
      InitializeComponent();
    }

    public class TempClass
    {
      public Option Option { get; set; }
      public Process Process { get; set; }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!CaptureProcess.HasExited)
      {
        myHwndHost.Dispose();
        foreach (var item in PageManager.GetPage<MonitorPage>().ProgramList)
        {
          if (item.Path == CaptureProcess.MainModule.FileName)
          {
            item.IsCapture = false;
            break;
          }
        }
      }
    }
  }
}
