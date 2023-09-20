using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

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
      ProcessList = new ObservableCollection<Process>();
      processListBox.ItemsSource = ProcessList;
      DataContext = this;
      UpdateProcessList();
    }
    public ObservableCollection<Process> ProcessList { get; set; }

    private void UpdateProcessList()
    {
      ProcessList.Clear();
      foreach (Process process in Process.GetProcesses().OrderBy(p=>p.ProcessName)) 
      {
        ProcessList.Add(process);
      }
    }

    private void processListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      if (MessageBox.Show("切换监控进程", "提示",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
      {
        Process process = (Process)processListBox.SelectedItem;
        if (process.MainWindowHandle == IntPtr.Zero)
        {
          MessageBox.Show("无法捕获该进程,可能该进程并没有窗口或者这个是隐藏的进程");
          return;
        }
        ViewWindow viewWindow = new ViewWindow(process);
        viewWindow.Show();
      }
    }
  }
}
