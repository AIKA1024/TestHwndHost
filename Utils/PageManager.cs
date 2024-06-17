using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TestHwndHost.Utils
{
    internal static class PageManager
    {
        private static readonly Dictionary<Type, Page> PageDic = new Dictionary<Type, Page>();

        public static T GetPage<T>() where T : Page, new()
        {
            if (PageDic.ContainsKey(typeof(T)))
            {
                return (T)PageDic[typeof(T)];
            }

            var page = new T();
            PageDic.Add(typeof(T), page);
            return page;
        }
    }
}