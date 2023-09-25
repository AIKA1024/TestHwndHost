using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace TestHwndHost.Resources.Converters
{
  internal class ProcessPathToIconConvert : MarkupExtension, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var icon = Icon.ExtractAssociatedIcon(value.ToString());
      var image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
        icon.Handle,
        Int32Rect.Empty,
        System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions()
      );
      return image;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }
  }
}
