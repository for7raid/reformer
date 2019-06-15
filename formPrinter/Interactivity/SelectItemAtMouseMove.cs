using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Expression.Interactivity;
using System.Windows.Controls;
using formPrinter.Converters;
using System.Diagnostics;
using System.Windows;
using formPrinter.Model;
using System.Windows.Shapes;

namespace formPrinter.Interactivity
{
    public class SelectItemAtMouseMove : Behavior<ItemsControl>
    {


        public DependencyObject Page
        {
            get { return (DependencyObject)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Page.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Page", typeof(DependencyObject), typeof(SelectItemAtMouseMove), new UIPropertyMetadata(null));

        


        public DependencyObject SelectedItem
        {
            get { return (DependencyObject)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(DependencyObject), typeof(SelectItemAtMouseMove), new UIPropertyMetadata(null));

        
        

        protected override void OnAttached()
        {
            
            base.OnAttached();

            //AssociatedObject.MouseMove += new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseMove);
            AssociatedObject.MouseUp += new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_MouseUp);

        }

        void AssociatedObject_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
           if (!(e.OriginalSource is Rectangle))
                SelectedItem = Page;
               
        }

        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!(SelectedItem is Element))
                SelectedItem = Page;
        }
    }
}
