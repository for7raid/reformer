using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace formPrinter
{
    public class ImageEditor : Xceed.Wpf.Toolkit.PropertyGrid.Editors.ITypeEditor
    {

        public ImageEditor()
        {

        }
        public System.Windows.FrameworkElement ResolveEditor(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                Margin = new System.Windows.Thickness(1,2,1,2)
            };

            Button btnEdit = new Button();
            btnEdit.Content = "P";
            btnEdit.ToolTip = "Редактировать изображение";
            btnEdit.Width = 20.0;
            btnEdit.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            btnEdit.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;

            btnEdit.Click += new System.Windows.RoutedEventHandler(btnEdit_Click);

            panel.Children.Add(btnEdit);

            panel.Children.Add(new Separator() { Width = 3 });

            Button btnOpen = new Button();
            btnOpen.Content = "...";
            btnOpen.ToolTip = "Загрузить изображение из файла";
            btnOpen.Width = 20.0;
            btnOpen.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            btnOpen.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;

            btnOpen.Click += new System.Windows.RoutedEventHandler(btnOpen_Click);

            panel.Children.Add(btnOpen);

            //create the binding from the bound property item to the editor
            var _binding = new Binding("Value"); //bind to the Value property of the PropertyItem
            _binding.Source = propertyItem;
            _binding.ValidatesOnExceptions = true;
            _binding.ValidatesOnDataErrors = true;
            _binding.Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            BindingOperations.SetBinding(panel, Button.TagProperty, _binding);

            return panel;
        }

        void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RegistryKey classesRoot = Registry.ClassesRoot;
            var sub = classesRoot.OpenSubKey(@"jpegfile\shell\edit\command") ?? classesRoot.OpenSubKey(@"SystemFileAssociations\image\shell\edit\command");
            var editor = sub.GetValue("").ToString().Replace("\"", "").Replace("%1", "");

            var panel = ((Panel)((Button)sender).Parent);
            var path = Path.GetTempFileName() + ".jpg";
            File.WriteAllBytes(path, formPrinter.Model.Page.ImageToBytes(panel.Tag as BitmapImage));

            var proc = System.Diagnostics.Process.Start(editor, path);
            proc.WaitForExit();
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            panel.Tag = formPrinter.Model.Page.ImageFromBytes(bytes);
            File.Delete(path);

        }

        void btnOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog.Filter = "Графические файлы (.jpg)|*.jpg;*.jpeg|Все файлы (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {

                byte[] bytes = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                ((Panel)((Button)sender).Parent).Tag = formPrinter.Model.Page.ImageFromBytes(bytes);
            }
        }
    }
}
