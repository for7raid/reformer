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
using formPrinter.Converters;

namespace formPrinter
{

    /// <summary>
    /// Interaction logic for InputElement.xaml
    /// </summary>
    public partial class InputElement : UserControl
    {



        public Element Item
        {
            get { return (Element)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Item.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(Element), typeof(InputElement), new UIPropertyMetadata(null, (s, e) =>
            {

                var c = s as InputElement;
                //c.grid.DataContext = e.NewValue;
                c.ShowMoveGrid();
            }));





        public DependencyObject SelectedItem
        {
            get { return (DependencyObject)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(DependencyObject), typeof(InputElement), new UIPropertyMetadata(null, (s, e) =>
            {
                var c = s as InputElement;
                c.ShowMoveGrid();
            }));



        Point mouseStartPosition;

        public InputElement()
        {
            InitializeComponent();
            grid.DataContext = this;
        }

        private void ShowMoveGrid()
        {
            if (SelectedItem == Item)
            {
                moveGrid.Visibility = Visibility.Visible;
                this.BringIntoView();
            }
            else
            {
                moveGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedItem = Item;

            var rec = sender as Rectangle;
            mouseStartPosition = e.GetPosition(rec);
            rec.CaptureMouse();

        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            var rec = sender as Rectangle;
            if (rec.IsMouseCaptureWithin)
            {
                Vector diff = e.GetPosition(rec) - mouseStartPosition;
                Item.Y = Math.Max(0, Item.Y + CentimeterToPixelConverter.ConvertToCmY(diff.Y));
                Item.X = Math.Max(0, Item.X + CentimeterToPixelConverter.ConvertToCmX(diff.X));
            }
        }

        private void rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var rec = sender as Rectangle;
            rec.ReleaseMouseCapture();
        }


        private void letfBottom_MouseMove(object sender, MouseEventArgs e)
        {
            var rec = sender as Rectangle;
            if (rec.IsMouseCaptureWithin)
            {
                Vector diff = e.GetPosition(rec) - mouseStartPosition;
                Item.Height = Math.Max(0, Item.Height + CentimeterToPixelConverter.ConvertToCmY(diff.Y));
                Item.Width = Math.Max(0, Item.Width - CentimeterToPixelConverter.ConvertToCmX(diff.X));
                if (Item.Width > 0)
                    Item.X = Math.Max(0, Item.X + CentimeterToPixelConverter.ConvertToCmX(diff.X));
            }
        }

        private void rightBottom_MouseMove(object sender, MouseEventArgs e)
        {
            var rec = sender as Rectangle;
            if (rec.IsMouseCaptureWithin)
            {
                Vector diff = e.GetPosition(rec) - mouseStartPosition;
                Item.Height = Math.Max(0, Item.Height + CentimeterToPixelConverter.ConvertToCmY(diff.Y));
                Item.Width = Math.Max(0, Item.Width + CentimeterToPixelConverter.ConvertToCmX(diff.X));
            }
        }

        private void rightTop_MouseMove(object sender, MouseEventArgs e)
        {
            var rec = sender as Rectangle;
            if (rec.IsMouseCaptureWithin)
            {
                Vector diff = e.GetPosition(rec) - mouseStartPosition;
                Item.Height = Math.Max(0, Item.Height - CentimeterToPixelConverter.ConvertToCmY(diff.Y));
                Item.Width = Math.Max(0, Item.Width + CentimeterToPixelConverter.ConvertToCmX(diff.X));
                if (Item.Height > 0)
                    Item.Y = Math.Max(0, Item.Y + CentimeterToPixelConverter.ConvertToCmY(diff.Y));

            }
        }

        private void leftTop_MouseMove(object sender, MouseEventArgs e)
        {
            var rec = sender as Rectangle;
            if (rec.IsMouseCaptureWithin)
            {
                Vector diff = e.GetPosition(rec) - mouseStartPosition;
                Item.Height -= CentimeterToPixelConverter.ConvertToCmY(diff.Y);
                Item.Width -= CentimeterToPixelConverter.ConvertToCmX(diff.X);
                if (Item.Height > 0)
                    Item.Y = Math.Max(0, Item.Y + CentimeterToPixelConverter.ConvertToCmY(diff.Y));
                if (Item.Width > 0)
                    Item.X = Math.Max(0, Item.X + CentimeterToPixelConverter.ConvertToCmX(diff.X));
            }
        }




    }
}
