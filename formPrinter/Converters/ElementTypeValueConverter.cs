using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Globalization;
using formPrinter.Model;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Reflection;
using System.ComponentModel;

namespace formPrinter.Converters
{
    public class ElementTypeValueConverter : IValueConverter
    {
        static List<Item> items;

        static ElementTypeValueConverter()
        {
            items = GetValues();
        }
       
    
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ElementType type = (ElementType)value;
            var item = items.FirstOrDefault(i => (ElementType)i.Value == type);
            if (item != null)
                return item.DisplayName;
            else
                return value;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string displayName = value.ToString();
            var item = items.FirstOrDefault(i => i.DisplayName == displayName);
            if (item != null)
                return item.Value;
            else
                return null;
        }

        public static List<Item> GetValues()
        {

            return Enum.GetValues(typeof(ElementType)).OfType<ElementType>().Select(val => new Item { DisplayName = GetDescription(val), Value = val }).ToList();

        }

        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            object[] attribs = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attribs.Length > 0)
            {
                string message = ((DescriptionAttribute)attribs[0]).Description;
                return message;
            }
            return string.Empty;
        }

        public static List<string> GetNames()
        {

            return Enum.GetValues(typeof(ElementType)).OfType<ElementType>().Select(val => GetDescription(val)).ToList();

        }
    }
}
