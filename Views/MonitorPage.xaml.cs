using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TestHwndHost.Views
{
  /// <summary>
  /// MonoitorPage.xaml 的交互逻辑
  /// </summary>
  public partial class MonitorPage : Page
  {
    public MonitorPage()
    {
      InitializeComponent();
      ProgramListBox.ItemsSource = ProgramList;
    }
    public class ProgramInfo : INotifyPropertyChanged
    {
      public string Path { get; set; }
      public BitmapSource Icon { get; set; }

      private bool isCapture;
      public bool IsCapture
      {
        get => isCapture;
        set
        {
          isCapture = value;
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCapture)));
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;
    }
    public ObservableCollection<ProgramInfo> ProgramList { get; set; } = new ObservableCollection<ProgramInfo>();
    private void Button_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "可执行文件(*.exe)|*.exe|所有文件(*.*)|*.*";
      if (dialog.ShowDialog() == true)
      {
        if (ProgramList.Any(p => p.Path == dialog.FileName))
          return;

        var icon = Icon.ExtractAssociatedIcon(dialog.FileName);
        var image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
          icon.Handle,
          Int32Rect.Empty,
          BitmapSizeOptions.FromEmptyOptions()
        );
        var programInfo = new ProgramInfo();
        programInfo.Path = dialog.FileName;
        programInfo.Icon = image;
        ProgramList.Add(programInfo);
      }
    }

    public async void CheckMonitorProgram(IEnumerable<Process> processesList)
    {
      foreach (var process in processesList)
      {
        foreach (var MonitorProgarm in ProgramList)
        {
          if (MonitorProgarm.IsCapture)
            continue;

          try
          {
            if (process.MainModule.FileName == MonitorProgarm.Path)
            {
              try
              {
                await Dispatcher.BeginInvoke(new Action(delegate
                {
                  //await Task.Delay(2000);
                  if (process.MainWindowHandle == IntPtr.Zero)
                    return;
                  ViewWindow viewWindow = new ViewWindow(process);
                  viewWindow.Show();
                  MonitorProgarm.IsCapture = true;
                }));
              }
              catch (Exception ex)
              {
                MessageBox.Show(ex.Message, "错误");
                process.Kill();
                process.Close();
                process.Dispose();
              }
            }
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
          }
        }
      }
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      MenuItem menuItem = (MenuItem)sender;
      ProgramList.Remove((ProgramInfo)menuItem.DataContext);
    }
  }
}
