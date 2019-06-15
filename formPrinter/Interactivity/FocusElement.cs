using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Expression.Interactivity;
using System.Windows.Controls;
using formPrinter.Converters;
using System.Diagnostics;
using System.Windows;

namespace formPrinter.Interactivity
{
    public class FocusElement : Behavior<ContentControl>
    {

        public DependencyObject Element
        {
            get { return (DependencyObject)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Page.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Element", typeof(DependencyObject), typeof(FocusElement), new UIPropertyMetadata(null));




        public DependencyObject SelectedItem
        {
            get { return (DependencyObject)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(DependencyObject), typeof(FocusElement), new UIPropertyMetadata(null, (s, e) =>
            {

                (s as FocusElement).CheckAndFocus(e.NewValue);

            }));

        private void CheckAndFocus(object p)
        {
            if (p == Element)
            {
                AssociatedObject.BringIntoView();
                AssociatedObject.Focus();
            }
        }




        protected override void OnAttached()
        {

            base.OnAttached();

            AssociatedObject.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(AssociatedObject_GotKeyboardFocus);


        }

        void AssociatedObject_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            //SelectedItem = Element;
        }

        void AssociatedObject_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
           
        }




    }
}
