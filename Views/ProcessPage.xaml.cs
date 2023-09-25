using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestHwndHost.Views
{
  /// <summary>
  /// ProcessView.xaml 的交互逻辑
  /// </summary>
  public partial class ProcessPage : Page
  {
    public ProcessPage()
    {
      InitializeComponent();
      ProcessList = new ObservableCollection<Process>();
      processListBox.ItemsSource = ProcessList;
    }
    public ObservableCollection<Process> ProcessList { get; set; }
    public async void UpdateProcessList()
    {
      ProcessList = new ObservableCollection<Process>();
      IOrderedEnumerable<Process> AllProcessList = null;
      await Task.Run(() =>
      {
        AllProcessList = Process.GetProcesses().Where(p => p.MainWindowHandle != IntPtr.Zero).OrderBy(p => p.ProcessName);
        foreach (Process process in AllProcessList)
        {
          ProcessList.Add(process);
        }
      });

      Application.Current.Dispatcher.Invoke(() =>
      {
        processListBox.ItemsSource = ProcessList;
      });
    }
    private void processListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      if (processListBox.SelectedItem == null)
        return;

      if (MessageBox.Show("切换监控进程", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
      {
        Process process = (Process)processListBox.SelectedItem;
        if (process.MainWindowHandle == IntPtr.Zero)
        {
          MessageBox.Show("无法捕获该进程,可能该进程并没有窗口或者这个是隐藏的进程", "提示");
          processListBox.SelectedItem = null;
          return;
        }
        ViewWindow viewWindow = new ViewWindow(process);
        try
        {
          viewWindow.Show();
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "错误");
          viewWindow.myHwndHost.Dispose();
        }
      }
      else
        processListBox.SelectedItem = null;

    }
  }
}
