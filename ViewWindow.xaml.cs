﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace TestHwndHost
{
  /// <summary>
  /// ViewWindow.xaml 的交互逻辑
  /// </summary>
  public partial class ViewWindow : Window
  {
    public Process MonoitorProcess { get; set; }
    public ViewWindow(Process process)
    {
      MonoitorProcess = process;
      DataContext = process;
      InitializeComponent();
    }
  }
}
