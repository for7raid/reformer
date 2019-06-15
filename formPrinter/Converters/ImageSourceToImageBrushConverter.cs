using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace formPrinter.Converters
{
    public class ImageSourceToImageBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                ImageBrush ib = new ImageBrush();
                ib.Stretch = Stretch.None;

                ib.ImageSource = value as BitmapSource;
                return ib;

            }
            catch (Exception)
            {
                return null;
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
