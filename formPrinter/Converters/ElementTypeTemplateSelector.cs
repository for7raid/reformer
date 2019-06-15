using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using formPrinter.Model;

namespace formPrinter.Converters
{
    public class ElementTypeTemplateSelectorpublic : DataTemplateSelector
    {
        public ResourceDictionary Resources { get; set; }

        public ElementTypeTemplateSelectorpublic()
        {
            Resources = new ResourceDictionary();
        }

        public override DataTemplate SelectTemplate(object item,
          DependencyObject container)
        {
            var element = (Element)item;
            var fe = container as FrameworkElement;
            if (fe == null) return null;

            var template = (DataTemplate)Resources[element.ElementType.ToString()];

            return template;
        }
    }
}
