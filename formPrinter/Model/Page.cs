using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Controls;
using System.IO.Compression;
using System.ComponentModel;
using System.Collections.Specialized;

namespace formPrinter.Model
{
    public class Page : DependencyObject, INotifyPropertyChanged
    {
        [Category("Общие")]
        [DisplayName("Название страницы")]
        [Description("Название страницы формы для отображения в дизайнере. На печать не выводится.")]
        [XmlAttribute]
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Page), new UIPropertyMetadata(""));

        [Category("Подложка")]
        [DisplayName("Изображение")]
        [Description("Фоновый рисунок для отображения в дизайнере и на предварительной печати. На печать не выводится.")]
        [Editor(typeof(ImageEditor), typeof(ImageEditor))]
        [XmlIgnore]
        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(BitmapImage), typeof(Page), new UIPropertyMetadata(null));

        public byte[] Picture { get { return ImageToBytes(Image); } set { Image = ImageFromBytes(value); } }


        
        [Category("Подложка")]
        [DisplayName("Смещение по вертикали")]
        [Description("Смещает верхний край изображения")]
        public double ImageTop
        {
            get { return (double)GetValue(ImageTopProperty); }
            set { SetValue(ImageTopProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageTop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageTopProperty =
            DependencyProperty.Register("ImageTop", typeof(double), typeof(Page), new UIPropertyMetadata(0.0));


        [Category("Подложка")]
        [DisplayName("Смещение по горизонтали")]
        [Description("Смещает левый край изображения")]
        public double ImageLeft
        {
            get { return (double)GetValue(ImageLeftProperty); }
            set { SetValue(ImageLeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageLeft.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageLeftProperty =
            DependencyProperty.Register("ImageLeft", typeof(double), typeof(Page), new UIPropertyMetadata(0.0));

        ObservableCollection<Element> _elements;
        public ObservableCollection<Element> Elements
        {
            get { return _elements; }
            set { _elements= value; }
        }

        //public ObservableCollection<Element> Elements
        //{
        //    get { return (ObservableCollection<Element>)GetValue(ElementsProperty); }
        //    set { SetValue(ElementsProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Elements.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ElementsProperty =
        //    DependencyProperty.Register("Elements", typeof(ObservableCollection<Element>), typeof(Page), new UIPropertyMetadata(null));

        [Category("Размер")]
        [DisplayName("Высота страницы")]
        [Description("Размер странциы в см. Влияет на результат печати")]
        [XmlAttribute]
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Height.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(double), typeof(Page), new UIPropertyMetadata(0.0));

        [Category("Размер")]
        [DisplayName("Ширина страницы")]
        [Description("Размер странциы в см. Влияет на результат печати")]
        [XmlAttribute]
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Width.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double), typeof(Page), new UIPropertyMetadata(0.0));



        public Page()
        {
            Elements = new ObservableCollection<Element>();
            Elements.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Elements_CollectionChanged);
            
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.OwnerType == this.GetType())
                NotifyPropertyChanged(e.Property.Name);

            base.OnPropertyChanged(e);
        }

        void Elements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (Element item in e.NewItems)
                {
                    item.PropertyChanged += new PropertyChangedEventHandler(element_PropertyChanged);
                }
                this.NotifyPropertyChanged("Elements");
            }

            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (Element item in e.OldItems)
                {
                    item.PropertyChanged -= new PropertyChangedEventHandler(element_PropertyChanged);
                }
                this.NotifyPropertyChanged("Elements");
            }

            
        }

        void element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        public static byte[] ImageToBytes(BitmapImage imageSource)
        {


            try
            {
                MemoryStream memStream = new MemoryStream();
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(imageSource));
                encoder.Save(memStream);

                memStream.Capacity = (int)memStream.Length;

                var bytes = memStream.GetBuffer();

                return bytes;

            }
            catch (Exception)
            {

                return null;
            }
        }

        //when deserializing, convert the string back to an image
        public static BitmapImage ImageFromBytes(byte[] imageBytes)
        {

            if (imageBytes.Length == 0)
                return null;

            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();


            return bi;
        }

        public Page Clone()
        {
            var page = new Page()
            {
                Name = Name,
                Height = Height,
                Width = Width,
                Picture = Picture
            };

            foreach (var item in Elements)
            {
                page.Elements.Add(item.Clone());
            }

            return page;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged( String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
