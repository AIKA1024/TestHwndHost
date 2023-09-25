using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using TestHwndHost.Utils;
using TestHwndHost.Views;

namespace TestHwndHost
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow : Window
  {
    System.Timers.Timer timer;
    ProcessPage processPage;
    SettingPage settingPage;
    MonitorPage monitorPage;
    public MainWindow()
    {
      InitializeComponent();
      processPage = PageManager.GetPage<ProcessPage>();
      monitorPage = PageManager.GetPage<MonitorPage>();
      settingPage = PageManager.GetPage<SettingPage>();
      ProcessRadBtn.IsChecked = true;
      processPage.UpdateProcessList();
      timer = new System.Timers.Timer();
      timer.Interval = 3000;
      timer.Elapsed += (s, e) =>
      {
        processPage.UpdateProcessList();
        //无法使用processPage.ProcessList因为这个不是ui线程拥有的
        monitorPage.CheckMonitorProgram(Process.GetProcesses().Where(p => p.MainWindowHandle != IntPtr.Zero).OrderBy(p => p.ProcessName));
      };
      timer.Start();
    }

    private void ProcessRadBtn_Checked(object sender, RoutedEventArgs e)
    {
      frame.Content = processPage;
    }

    private void SettingRadBtn_Checked(object sender, RoutedEventArgs e)
    {
      frame.Content = settingPage;
    }

    private void MonitorRadBtn_Checked(object sender, RoutedEventArgs e)
    {
      frame.Content = monitorPage;
    }
  }
}
