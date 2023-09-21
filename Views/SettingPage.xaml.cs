using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestHwndHost.Views
{
  /// <summary>
  /// SettingPage.xaml 的交互逻辑
  /// </summary>
  public partial class SettingPage : Page
  {
    public Option PgOption { get; set; } = new Option();
    public SettingPage()
    {
      InitializeComponent();

      DataContext = PgOption;
    }
  }
  public class Option : INotifyPropertyChanged
  {
    private bool alwaysOnTop;

    public bool AlwaysOnTop
    {
      get { return alwaysOnTop; }
      set
      {
        alwaysOnTop = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlwaysOnTop)));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
