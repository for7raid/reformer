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
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using formPrinter.Converters;

namespace formPrinter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Form Form
        {
            get { return (Form)GetValue(FormProperty); }
            set { SetValue(FormProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Form.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormProperty =
            DependencyProperty.Register("Form", typeof(Form), typeof(MainWindow), new UIPropertyMetadata(null));


        public MainWindow()
        {

            InitializeComponent();


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new MainWindowViewModel();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var canvas = sender as Canvas;
            var pos = e.GetPosition(canvas);
            Debug.WriteLine("{0:N2}:{1:N2}", CentimeterToPixelConverter.ConvertToCmX(pos.X), CentimeterToPixelConverter.ConvertToCmY(pos.Y));


        }



    }
}
