using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Reflection;
using Xceed.Wpf.Toolkit.PropertyGrid;
using formPrinter.Model;

namespace formPrinter.Converters
{
    public class PropertyDefinitionCollectionConverter : IValueConverter
    {
        

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PropertyDefinitionCollection col = new PropertyDefinitionCollection();

            col.Add(new PropertyDefinition() { Name = "Name" });

            if (value is Page)
            {
                col.Add(new PropertyDefinition() { Name = "Width" });
                col.Add(new PropertyDefinition() { Name = "Height" });
                col.Add(new PropertyDefinition() { Name = "Image" });
                col.Add(new PropertyDefinition() { Name = "ImageTop" });
                col.Add(new PropertyDefinition() { Name = "ImageLeft" });
            }

            else if (value is Element)
            {
                col.Add(new PropertyDefinition() { Name = "Width" });
                col.Add(new PropertyDefinition() { Name = "Height" });
                col.Add(new PropertyDefinition() { Name = "X" });
                col.Add(new PropertyDefinition() { Name = "Y" });
                col.Add(new PropertyDefinition() { Name = "Value" });
                col.Add(new PropertyDefinition() { Name = "FontSize" });
                col.Add(new PropertyDefinition() { Name = "FontName" });
                col.Add(new PropertyDefinition() { Name = "Bold" });
                col.Add(new PropertyDefinition() { Name = "Italic" });
                col.Add(new PropertyDefinition() { Name = "Underline" });
                col.Add(new PropertyDefinition() { Name = "Strikeout" });
                col.Add(new PropertyDefinition() { Name = "ElementType" });
                col.Add(new PropertyDefinition() { Name = "ListChoises" });
                col.Add(new PropertyDefinition() { Name = "Font" });
                col.Add(new PropertyDefinition() { Name = "Style" });
                //col.Add(new PropertyDefinition() { Name = "GroupName" });
            }

            return col;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
            
        }

        
    }

}
