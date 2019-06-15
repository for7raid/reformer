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
    public class MousePosition : Behavior<ItemsControl>
    {


        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(MousePosition), new UIPropertyMetadata(0.0));



        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(MousePosition), new UIPropertyMetadata(0.0));

        

        protected override void OnAttached()
        {
            
            base.OnAttached();

            AssociatedObject.MouseMove += new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseMove);
            //AssociatedObject.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseMove);

        }

        

        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var pos = e.GetPosition(sender as IInputElement);
            X = CentimeterToPixelConverter.ConvertToCmX(pos.X);
            Y = CentimeterToPixelConverter.ConvertToCmY(pos.Y);
            //Debug.WriteLine("{0:N2}:{1:N2}", CentimeterToPixelConverter.ConvertToCmX(pos.X), CentimeterToPixelConverter.ConvertToCmY(pos.Y));
        }
    }
}
