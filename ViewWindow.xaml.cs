using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestHwndHost.Utils;
using TestHwndHost.Views;

namespace TestHwndHost
{
  /// <summary>
  /// ViewWindow.xaml 的交互逻辑
  /// </summary>
  public partial class ViewWindow : Window
  {
    public Process CaptureProcess { get; set; }
    public ViewWindow(Process process)
    {
      CaptureProcess = process;
      DataContext = process;
      process.Exited += (s, e) => Close();
      InitializeComponent();
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
