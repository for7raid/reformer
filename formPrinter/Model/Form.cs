using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.Specialized;

namespace formPrinter.Model
{
    public class Form : DependencyObject
    {
        [Category("Общие")]
        [DisplayName("Название формы")]
        [XmlAttribute]
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Form), new UIPropertyMetadata(""));

        ObservableCollection<Page> _pages;
        public ObservableCollection<Page> Pages
        {
            get { return _pages; }
            set { _pages = value; }
        }

        //public ObservableCollection<Page> Pages
        //{
        //    get { return (ObservableCollection<Page>)GetValue(PagesProperty); }
        //    set { SetValue(PagesProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Pages.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PagesProperty =
        //    DependencyProperty.Register("Pages", typeof(ObservableCollection<Page>), typeof(Form), new UIPropertyMetadata(null));


        [XmlIgnore]
        public bool HasChanges
        {
            get { return (bool)GetValue(HasChangesProperty); }
            set { SetValue(HasChangesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasChanges.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasChangesProperty =
            DependencyProperty.Register("HasChanges", typeof(bool), typeof(Form), new UIPropertyMetadata(false));


        public List<Element> AllElements
        {
            get { return Pages.SelectMany(p => p.Elements).ToList(); }
        }

        public Form()
        {
            this.Pages = new ObservableCollection<Page>();
            Pages.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Pages_CollectionChanged);

        }

        void Pages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (Page item in e.NewItems)
                {
                    item.PropertyChanged += new PropertyChangedEventHandler(page_PropertyChanged);
                }
                HasChanges = true;
            }

            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (Page item in e.OldItems)
                {
                    item.PropertyChanged -= new PropertyChangedEventHandler(page_PropertyChanged);
                }
                HasChanges = true;
            }


        }

        void page_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            HasChanges = true;
        }


    }
}
