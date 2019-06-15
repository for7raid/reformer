using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using formPrinter.Model;


namespace formPrinter
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window
    {


        public Form Form
        {
            get { return (Form)GetValue(FormProperty); }
            set { SetValue(FormProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Form.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormProperty =
            DependencyProperty.Register("Form", typeof(Form), typeof(PropertiesWindow), new UIPropertyMetadata(null, (s, e) => 
            {

                (s as PropertiesWindow).OnFormChanged(e.NewValue as Form);
            }));

       


        public DependencyObject SelectedItem
        {
            get { return (DependencyObject)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(DependencyObject), typeof(PropertiesWindow), new UIPropertyMetadata(null));

        

        public PropertiesWindow()
        {
            
            InitializeComponent();
            DataContext = this;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = (DependencyObject)e.NewValue;
        }

        private void OnFormChanged(Model.Form form)
        {
            trv.Items.Clear();
            trv.Items.Add(form);
        }
    }
}
