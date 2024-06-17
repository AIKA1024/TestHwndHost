using System.ComponentModel;
using System.Windows.Controls;

namespace TestHwndHost.Views
{
  /// <summary>
  ///     SettingPage.xaml 的交互逻辑
  /// </summary>
  public partial class SettingPage : Page
  {
    public SettingPage()
    {
      InitializeComponent();

      DataContext = PgOption;
    }

    public Option PgOption { get; set; } = new Option();
  }

  public class Option : INotifyPropertyChanged
  {
    private bool alwaysOnTop;

    public bool AlwaysOnTop
    {
      get => alwaysOnTop;
      set
      {
        alwaysOnTop = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlwaysOnTop)));
      }
    }

    private bool isPreventScreenshots;

    public bool IsPreventScreenshots
    {
      get { return isPreventScreenshots; }
      set 
      {
        isPreventScreenshots = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPreventScreenshots)));
      }
    }


    public event PropertyChangedEventHandler PropertyChanged;
  }
}