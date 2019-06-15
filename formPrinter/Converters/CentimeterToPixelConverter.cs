using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Reflection;

namespace formPrinter.Converters
{
    public class CentimeterToPixelConverter : IValueConverter
    {
        public static int DpiX { get; private set; }
        public static int DpiY { get; private set; }



        static CentimeterToPixelConverter()
        {
            PropertyInfo dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            PropertyInfo dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
            DpiX = (int)dpiXProperty.GetValue(null, null);
            DpiY = (int)dpiYProperty.GetValue(null, null);

        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DPIConverterFactor factor = (DPIConverterFactor)Enum.Parse(typeof(DPIConverterFactor), parameter.ToString());

            double pixel = -1;
            var Centimeter = System.Convert.ToDouble(value);

            if (Centimeter == 0)
                return -1;

            switch (factor)
            {
                case DPIConverterFactor.Y:
                    pixel = Centimeter * DpiX / 2.54d;
                    break;
                case DPIConverterFactor.X:
                    pixel = Centimeter * DpiX / 2.54d;
                    break;

            }
            return (int)pixel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
            
        }

        public static double ConvertX(double value)
        {
            return value * DpiX / 2.54d;
        }

        public static double ConvertY(double value)
        {
            return value * DpiY / 2.54d;
        }

        public static double ConvertToCmX(double value)
        {
            return value / DpiX * 2.54d;
        }

        public static double ConvertToCmY(double value)
        {
            return value / DpiY * 2.54d;
        }
    }

    public enum DPIConverterFactor
    {
        Y = 1,
        X = 2
    }
}
