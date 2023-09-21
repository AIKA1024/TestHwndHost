using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TestHwndHost
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow : Window
  {
    System.Timers.Timer timer;
    public MainWindow()
    {
      InitializeComponent();
      ProcessList = new ObservableCollection<Process>();
      processListBox.ItemsSource = ProcessList;
      DataContext = this;
      UpdateProcessList();
      timer = new System.Timers.Timer();
      timer.Interval = 3000;
      timer.Elapsed += (s, e) => { UpdateProcessList(); };
      timer.Start();
    }

    public ObservableCollection<Process> ProcessList { get; set; }

    private void UpdateProcessList()
    {
      Application.Current.Dispatcher.Invoke(() =>
      {
        ProcessList.Clear();
        foreach (Process process in Process.GetProcesses().OrderBy(p => p.ProcessName))
        {
          ProcessList.Add(process);
        }
      });
    }

    private void processListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      timer.Stop();
      if (processListBox.SelectedItem == null)
      {
        timer.Start();
        return;
      }

      if (MessageBox.Show("切换监控进程", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
      {
        Process process = (Process)processListBox.SelectedItem;
        if (process.MainWindowHandle == IntPtr.Zero)
        {
          MessageBox.Show("无法捕获该进程,可能该进程并没有窗口或者这个是隐藏的进程","提示");
          processListBox.SelectedItem = null;
          timer.Start();
          return;
        }
        ViewWindow viewWindow = new ViewWindow(process);
        try
        {
          viewWindow.Show();
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message,"错误");
          process.Kill();
          process.Close();
          process.Dispose();
        }
      }
      else
        processListBox.SelectedItem = null;

      timer.Start();
    }
  }
}
