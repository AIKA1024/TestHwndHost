using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestHwndHost.Utils
{
  internal static class PageManager
  {
    private static Dictionary<Type, Page> PageDic = new Dictionary<Type, Page>();
    public static T GetPage<T>() where T : Page, new()
    {
      if (PageDic.ContainsKey(typeof(T))) 
        return (T)PageDic[typeof(T)];
      else
      {
        T page = new T();
        PageDic.Add(typeof(T), page);
        return page;
      }
    }
  }
}
