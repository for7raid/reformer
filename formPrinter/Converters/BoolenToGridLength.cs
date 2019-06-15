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

namespace formPrinter.Converters
{
    public class BoolenToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool show = (bool)value;

            if (!show)
                return new GridLength(0, GridUnitType.Pixel);

            var str = parameter.ToString();
            double length;
            GridUnitType type = GridUnitType.Pixel;
            if (str.IndexOf("*") > -1)
            {
                var sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                str = str.Replace("*", "").Replace(",", sep).Replace(".", sep);
                length = double.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint);
                type = GridUnitType.Star;
            }
            else if(str.ToLower() == "auto")
            {
                return GridLength.Auto;
            }
            else
            {
                length = double.Parse(str);
                type = GridUnitType.Pixel;
            }

            return new GridLength(length, type);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
