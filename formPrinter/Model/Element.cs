using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using System.Drawing;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Reflection;
using FontStyle = System.Drawing.FontStyle;

namespace formPrinter.Model
{
    public class Element : DependencyObject, INotifyPropertyChanged
    {
        [Category("Общие")]
        [DisplayName("Название поля")]
        [Description("Название поля на форме для отображения в дизайнере. На печать не выводится.")]
        [XmlAttribute]
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Element), new PropertyMetadata(""));


        [Category("Общие")]
        [DisplayName("Значение поля")]
        [Description("Text, содержащийся в поле. Печатается.")]
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(Element), new UIPropertyMetadata(""));

        [Category("Общие")]
        [DisplayName("Тип поля")]
        [Description("Задает какой тип поля используется при заполнении бланка. На печать не влияет.")]
        //[ItemsSource(typeof(ElementTypeItemsSource))]
        [XmlAttribute]
        public ElementType ElementType
        {
            get { return (ElementType)GetValue(ElementTypeProperty); }
            set { SetValue(ElementTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElementType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementTypeProperty =
            DependencyProperty.Register("ElementType", typeof(ElementType), typeof(Element), new UIPropertyMetadata(ElementType.Text, (s,e)=>
            {
                s.CoerceValue(Element.HeightProperty);
                s.CoerceValue(Element.WidthProperty);
            }));

        [Category("Общие")]
        [DisplayName("Список вариантов")]
        [Description("Варианты выбора для поля типа List. На другие поля не влияет.")]
        [XmlIgnore]
        public ObservableCollection<string> ListChoises
        {
            get { return (ObservableCollection<string>)GetValue(ListChoisesProperty); }
            set { SetValue(ListChoisesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ListChoises.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ListChoisesProperty =
            DependencyProperty.Register("ListChoises", typeof(ObservableCollection<string>), typeof(Element), new UIPropertyMetadata(null));

        [XmlElement("ListChoises")]
        public string ListChoisesSeparetedText
        {
            get { return String.Join(";", ListChoises); ; }
            set { ListChoises = new ObservableCollection<string>(value.Split(';')); }
        }



        [Category("Общие")]
        [DisplayName("Группа для поля \"Галочка \"Один из варинатов\"")]
        [Description("Объединяет поля в группу и дает возможность выбрать только один из вариантов, например поле Пол, выбор только М или Ж")]
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(Element), new UIPropertyMetadata(""));

        

        [Category("Шрифт")]
        [DisplayName("Размер")]
        [Description("Размер шрифта, которым прозиводится заполнение. Влияет на печать.")]
        [XmlAttribute]
        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(int), typeof(Element), new UIPropertyMetadata(12), ValidateSizePointValue);


        [Category("Шрифт")]
        [DisplayName("Шрифт")]
        [Description("Шрифт, которым прозиводится заполнение. Влияет на печать.")]
        [XmlAttribute]
        public string FontName
        {
            get { return (string)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Font.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontProperty =
            DependencyProperty.Register("FontName", typeof(string), typeof(Element), new PropertyMetadata("Arial"));

        [Category("Шрифт")]
        [DisplayName("Жирный")]
        [Description("Жирное начертание. Влияет на печать.")]
        [XmlAttribute]
        public bool Bold
        {
            get { return (bool)GetValue(BoldProperty); }
            set { SetValue(BoldProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Bold.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoldProperty =
            DependencyProperty.Register("Bold", typeof(bool), typeof(Element), new PropertyMetadata(false));

        [Category("Шрифт")]
        [DisplayName("Курсив")]
        [Description("Курсивное начертание. Влияет на печать.")]
        [XmlAttribute]
        public bool Italic
        {
            get { return (bool)GetValue(ItalicProperty); }
            set { SetValue(ItalicProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Italic.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItalicProperty =
            DependencyProperty.Register("Italic", typeof(bool), typeof(Element), new PropertyMetadata(false));

        [Category("Шрифт")]
        [DisplayName("Подчеркнутый")]
        [Description("Подчеркнуто нижней чертой. Влияет на печать.")]
        [XmlAttribute]
        public bool Underline
        {
            get { return (bool)GetValue(UnderlineProperty); }
            set { SetValue(UnderlineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Underline.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnderlineProperty =
            DependencyProperty.Register("Underline", typeof(bool), typeof(Element), new PropertyMetadata(false));


        [Category("Шрифт")]
        [DisplayName("Перечеркнутый")]
        [Description("Перечеркнутое начертание. Влияет на печать.")]
        [XmlAttribute]
        public bool Strikeout
        {
            get { return (bool)GetValue(StrikeoutProperty); }
            set { SetValue(StrikeoutProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Strikeout.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrikeoutProperty =
            DependencyProperty.Register("Strikeout", typeof(bool), typeof(Element), new PropertyMetadata(false));

        public FontStyle FontStyle
        {
            get
            {
                return FontStyle.Regular |
                    (Bold ? FontStyle.Bold : FontStyle.Regular) |
                    (Italic ? FontStyle.Italic : FontStyle.Regular) |
                    (Underline ? FontStyle.Underline : FontStyle.Regular) |
                    (Strikeout ? FontStyle.Strikeout : FontStyle.Regular);
            }
        }

        [Category("Положение")]
        [DisplayName("Позиция Y (по вертикали) поля")]
        [Description("Расстояние поля от края лист в см. Исчисляется от верхнего левого угла листа!")]
        [XmlAttribute]
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Element), new UIPropertyMetadata(1.0), ValidateSizePointValue);

        [Category("Положение")]
        [DisplayName("Позиция X (по горизонтали) поля")]
        [Description("Расстояние поля от края лист в см. Исчисляется от верхнего левого угла листа!")]
        [XmlAttribute]
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Element), new UIPropertyMetadata(1.0), ValidateSizePointValue);



        [Category("Размер")]
        [DisplayName("Высота поля")]
        [Description("Размер поля в см. Влияет на результат печати")]
        [XmlAttribute]
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Height.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(double), typeof(Element), new UIPropertyMetadata(1.0, (s, e) => { }, CoerceSize), ValidateSizePointValue);

        [Category("Размер")]
        [DisplayName("Ширина поля")]
        [Description("Размер поля в см. Влияет на результат печати")]
        [XmlAttribute]
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double), typeof(Element), new UIPropertyMetadata(5.0, (s, e) => { }, CoerceSize), ValidateSizePointValue);


        public static bool ValidateSizePointValue(object value)
        {
            return double.Parse(value.ToString()) >= 0;
        }

        private static object CoerceSize(DependencyObject obj, object value)
        {
            var element = obj as Element;
            //if (element.ElementType == ElementType.Check)
            //    return 0.50;
            //else
            if (element.ElementType == ElementType.Anchor)
                return 1.0;
            else
                return value;
        }
        
        public Element()
        {
            ListChoises = new ObservableCollection<string>();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            
            if (e.Property.OwnerType == this.GetType())
                NotifyPropertyChanged(e.Property.Name);
            
            base.OnPropertyChanged(e);
        }

        public override string ToString()
        {
            return Name;
        }

        public Element Clone()
        {
            return new Element()
            {
                Name = Name,
                Height = Height,
                Width = Width,
                X = X,
                Y = Y,
                ElementType = ElementType,
                ListChoisesSeparetedText = ListChoisesSeparetedText,
                Value = Value,
                FontSize = FontSize,
                FontName = FontName

            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public enum ElementType
    {
        [DescriptionAttribute("Якорь")]
        Anchor=0,
        [DescriptionAttribute("Текст")]
        Text = 1,
        [DescriptionAttribute("Календарь")]
        Calendar = 2,
        [DescriptionAttribute("Выпадающий список")]
        List = 3,
        [DescriptionAttribute("Галочка")]
        Check = 4
        //[Description("Галочка \"Один из вариантов\"")]
        //RadioButton=4

    }

    public class ElementTypeItemsSource
    {
        public List<Item> GetValues()
        {

            var DataSource = Enum.GetValues(typeof(ElementType)).OfType<ElementType>().Select(val => new Item { DisplayName = GetDescription(val), Value = val });
            return DataSource.ToList();

        }

        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            object[] attribs = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attribs.Length > 0)
            {
                string message = ((DescriptionAttribute)attribs[0]).Description;
                return message;
            }
            return string.Empty;
        }
    }

}
