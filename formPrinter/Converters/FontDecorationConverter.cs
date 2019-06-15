

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace formPrinter.Converters
{
    public class FontDecorationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var underline = values[0] != DependencyProperty.UnsetValue ? (bool)values[0] : false;
            var strikeout = values[1] != DependencyProperty.UnsetValue ? (bool)values[1] : false;

            var decors = new TextDecorationCollection();
            if (underline)
            {
                foreach (var item in TextDecorations.Underline)
                {
                    decors.Add(item);
                }
            }

            if (strikeout)
            {
                foreach (var item in TextDecorations.Strikethrough)
                {
                    decors.Add(item);
                }
            }
            return decors;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
