using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Expression.Interactivity;
using System.Windows.Controls;
using formPrinter.Converters;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace formPrinter.Interactivity
{
    public class ScrollViewerMouseZoom : Behavior<ScrollViewer>
    {



        public ICommand ZoomIn
        {
            get { return (ICommand)GetValue(ZoomInProperty); }
            set { SetValue(ZoomInProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoomIn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomInProperty =
            DependencyProperty.Register("ZoomIn", typeof(ICommand), typeof(ScrollViewerMouseZoom), new UIPropertyMetadata(null));


        public ICommand ZoomOut
        {
            get { return (ICommand)GetValue(ZoomOutProperty); }
            set { SetValue(ZoomOutProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoomOut.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomOutProperty =
            DependencyProperty.Register("ZoomOut", typeof(ICommand), typeof(ScrollViewerMouseZoom), new UIPropertyMetadata(null));




        public DependencyObject Form
        {
            get { return (DependencyObject)GetValue(FormProperty); }
            set { SetValue(FormProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Form.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormProperty =
            DependencyProperty.Register("Form", typeof(DependencyObject), typeof(ScrollViewerMouseZoom), new UIPropertyMetadata(null));

        

        protected override void OnAttached()
        {
            
            base.OnAttached();

            AssociatedObject.PreviewMouseWheel += new System.Windows.Input.MouseWheelEventHandler(AssociatedObject_PreviewMouseWheel);
            AssociatedObject.PreviewMouseUp += new MouseButtonEventHandler(AssociatedObject_PreviewMouseUp);
        }

        void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Form != null)
            {
                MainWindowViewModel.SetScale(Form, 1.0);
            }
        }

        void AssociatedObject_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Form != null)
            {

                if (e.Delta > 0 && ZoomIn != null)
                {
                    ZoomIn.Execute(Form);
                    e.Handled = true;
                }
                else if (e.Delta < 0 && ZoomOut != null)
                {
                    ZoomOut.Execute(Form);
                    e.Handled = true;
                }
            }
        }

        
    }
}
