using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace formPrinter
{
    public class EnumCheckComboBoxEditor : TypeEditor<CheckComboBox>
    {
        protected override void SetValueDependencyProperty()
        {
            ValueProperty = Xceed.Wpf.Toolkit.Primitives.Selector.SelectedItemsProperty;
            Editor = new PropertyGridEditorCheckComboBox();
        }

        protected override void ResolveValueBinding(PropertyItem propertyItem)
        {
            SetItemsSource(propertyItem);

            var _binding = new Binding("Value");
            _binding.Source = propertyItem;
            _binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _binding.Mode = BindingMode.TwoWay;
            _binding.Converter = CreateValueConverter();
            BindingOperations.SetBinding(Editor, CheckComboBox.SelectedValueProperty, _binding);

            //var _binding2 = new Binding("Value");
            //_binding2.Source = propertyItem;
            //_binding2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //_binding2.Mode = BindingMode.TwoWay;
            //_binding2.Converter = CreateValueConverter();
            //BindingOperations.SetBinding(Editor, CheckComboBox.SelectedItemProperty, _binding2);

            Editor.ItemsSource = Enum.GetValues(propertyItem.Value.GetType());

            //base.ResolveValueBinding(propertyItem);
        }

        private void SetItemsSource(PropertyItem propertyItem)
        {
            Editor.ItemsSource = CreateItemsSource(propertyItem);
        }

        protected IEnumerable CreateItemsSource(PropertyItem propertyItem)
        {
            return GetValues(propertyItem.PropertyType);
        }

        private static object[] GetValues(Type enumType)
        {
            List<object> values = new List<object>();

            if (enumType != null)
            {
                var fields = enumType.GetFields().Where(x => x.IsLiteral);
                foreach (FieldInfo field in fields)
                {
                    // Get array of BrowsableAttribute attributes
                    object[] attrs = field.GetCustomAttributes(typeof(BrowsableAttribute), false);

                    if (attrs.Length == 1)
                    {
                        // If attribute exists and its value is false continue to the next field...
                        BrowsableAttribute brAttr = (BrowsableAttribute)attrs[0];
                        if (brAttr.Browsable == false)
                        {
                            continue;
                        }
                    }

                    values.Add(field.GetValue(enumType));
                }
            }

            return values.ToArray();
        }
    }

    public class PropertyGridEditorCheckComboBox : CheckComboBox
    {
        static PropertyGridEditorCheckComboBox()
        {

        }
    }
}