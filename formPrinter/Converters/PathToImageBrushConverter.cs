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
    public class PathToImageBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageBrush ib = new ImageBrush();
            ib.Stretch = Stretch.None;
            string path = (string)value;

            try
            {
                //ABSOLUTE
                if (path.Length > 0 && path[0] == System.IO.Path.DirectorySeparatorChar
                    || path.Length > 1 && path[1] == System.IO.Path.VolumeSeparatorChar)
                    ib.ImageSource= new BitmapImage(new Uri(path));

                else
                    ib.ImageSource = new BitmapImage(new Uri(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates", path)));

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
