using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Reflection;

namespace formPrinter.Converters
{
    public class CentimeterToPixelConverterForAnchor : IValueConverter
    {
        public static int DpiX { get; private set; }
        public static int DpiY { get; private set; }



        static CentimeterToPixelConverterForAnchor()
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

           
            switch (factor)
            {
                case DPIConverterFactor.Y:
                    pixel = (Centimeter - 0.5) * DpiX / 2.54d;
                    break;
                case DPIConverterFactor.X:
                    pixel = (Centimeter - 0.5) * DpiX / 2.54d;
                    break;

            }
            return (int)pixel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
            
        }

      
    }

    
}
