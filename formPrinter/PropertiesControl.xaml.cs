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
using System.Windows.Navigation;
using System.Windows.Shapes;
using formPrinter.Model;

namespace formPrinter
{
    /// <summary>
    /// Interaction logic for PropertiesControl.xaml
    /// </summary>
    public partial class PropertiesControl : UserControl
    {

        public Form Form
        {
            get { return (Form)GetValue(FormProperty); }
            set { SetValue(FormProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Form.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormProperty =
            DependencyProperty.Register("Form", typeof(Form), typeof(PropertiesControl), new UIPropertyMetadata(null, (s, e) =>
            {

                (s as PropertiesControl).OnFormChanged(e.NewValue as Form);
            }));




        public DependencyObject SelectedItem
        {
            get { return (DependencyObject)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(DependencyObject), typeof(PropertiesControl), new UIPropertyMetadata(null, (s, e) => 
            {

                (s as PropertiesControl).OnSelectedChanged(e.NewValue);
            }));

        public PropertiesControl()
        {
            InitializeComponent();
            this.DataContext = this;
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

        private void OnSelectedChanged(object p)
        {
            SetSelected(trv, p);
        }

        static private bool SetSelected(ItemsControl parent, object child)
        {

            if (parent == null || child == null)
            {
                return false;
            }

            TreeViewItem childNode = parent.ItemContainerGenerator
                .ContainerFromItem(child) as TreeViewItem;

            if (childNode != null)
            {
                childNode.Focus();
                return childNode.IsSelected = true;
            }

            if (parent.Items.Count > 0)
            {
                foreach (object childItem in parent.Items)
                {
                    ItemsControl childControl = parent
                        .ItemContainerGenerator
                        .ContainerFromItem(childItem)
                        as ItemsControl;

                    if (SetSelected(childControl, child))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
