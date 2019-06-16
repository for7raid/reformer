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
using System.Windows.Input;

namespace formPrinter.Interactivity
{
    public class KeyboardMoveElement : Behavior<ItemsControl>
    {


        public DependencyObject Page
        {
            get { return (DependencyObject)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Page.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Page", typeof(DependencyObject), typeof(KeyboardMoveElement), new UIPropertyMetadata(null));

        


        public DependencyObject SelectedItem
        {
            get { return (DependencyObject)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(DependencyObject), typeof(KeyboardMoveElement), new UIPropertyMetadata(null));

        
        

        protected override void OnAttached()
        {
            
            base.OnAttached();

            AssociatedObject.PreviewKeyDown += AssociatedObject_KeyUp;

        }

        private void AssociatedObject_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var element = SelectedItem as Element;
            if (element == null) return;

            switch (e.Key)
            {
                case Key.Up:
                    element.Y -= 0.1;
                    break;
                case Key.Down:
                    element.Y += 0.1;
                    break;

            }
        }
    }
}
