using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;

using System.Globalization;
using formPrinter.Model;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;

namespace formPrinter.Converters
{
    public class FontStyleValueConverter : IValueConverter
    {
        static List<Item> items;

        static FontStyleValueConverter()
        {
            items = GetValues();
        }
       
    
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            var v = (FontStyle)value;
            var dd = items.Where(i => v.HasFlag((FontStyle)i.Value)).ToList();
            return dd;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var vals = value as IList<string>;
            if (vals == null || !vals.Any())
                return FontStyle.Regular;
            else
            {
                return Enum.Parse(typeof(FontStyle), string.Join(",", vals));
            }
        }

        public static List<Item> GetItems()
        {
            //return Enum.GetNames(typeof (FontStyle));
            return items;
        }

        public static List<Item> GetValues()
        {

            return Enum.GetValues(typeof(FontStyle)).OfType<FontStyle>().Select(val => new Item { DisplayName = Enum.GetName(typeof (FontStyle), val), Value = val }).ToList();

        }

       
    }
}
