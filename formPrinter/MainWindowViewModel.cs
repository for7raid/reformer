using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using formPrinter.Model;
using System.Windows.Input;
using akarov;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using System.Drawing.Printing;
using System.Drawing;
using formPrinter.Converters;
using System.Diagnostics;
using akarov.Controls.MVVM.Commands;
using akarov.Controls.Utils;
using akarov.Controls.Exceptions;
using akarov.Controls.Extensions;


namespace formPrinter
{
    public class MainWindowViewModel : DependencyObject
    {

        private static string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static string templatePath = Path.Combine(appPath, "Templates");

        public static bool GetIsSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedProperty);
        }

        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(MainWindowViewModel), new UIPropertyMetadata(false));




        public double CurrentPositionX
        {
            get { return (double)GetValue(CurrentPositionXProperty); }
            set { SetValue(CurrentPositionXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPositionX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPositionXProperty =
            DependencyProperty.Register("CurrentPositionX", typeof(double), typeof(MainWindowViewModel), new UIPropertyMetadata(0.0));



        public double CurrentPositionY
        {
            get { return (double)GetValue(CurrentPositionYProperty); }
            set { SetValue(CurrentPositionYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPositionY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPositionYProperty =
            DependencyProperty.Register("CurrentPositionY", typeof(double), typeof(MainWindowViewModel), new UIPropertyMetadata(0.0));




        public ObservableCollection<Form> Forms
        {
            get { return (ObservableCollection<Form>)GetValue(FormsProperty); }
            set { SetValue(FormsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Forms.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormsProperty =
            DependencyProperty.Register("Forms", typeof(ObservableCollection<Form>), typeof(MainWindowViewModel), new UIPropertyMetadata(null));



        public Form CurrentForm
        {
            get { return (Form)GetValue(CurrentFormProperty); }
            set { SetValue(CurrentFormProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentForm.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentFormProperty =
            DependencyProperty.Register("CurrentForm", typeof(Form), typeof(MainWindowViewModel), new UIPropertyMetadata(null));



        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(MainWindowViewModel), new UIPropertyMetadata(null, (s, e) =>
            {

                var vm = s as MainWindowViewModel;
                var newVal = e.NewValue as DependencyObject;
                var oldVal = e.OldValue as DependencyObject;

                if (oldVal != null)
                    MainWindowViewModel.SetIsSelected(oldVal, false);
                if (newVal != null)
                    MainWindowViewModel.SetIsSelected(newVal, true);

            }));



        public bool IsInDesign
        {
            get { return (bool)GetValue(IsInDesignProperty); }
            set { SetValue(IsInDesignProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsInDesign.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInDesignProperty =
            DependencyProperty.Register("IsInDesign", typeof(bool), typeof(MainWindowViewModel), new UIPropertyMetadata(false));



        public bool IsTableMode
        {
            get { return (bool)GetValue(IsTableModeProperty); }
            set { SetValue(IsTableModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTableMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTableModeProperty =
            DependencyProperty.Register("IsTableMode", typeof(bool), typeof(MainWindowViewModel), new UIPropertyMetadata(false));




        public static string GetFileName(DependencyObject obj)
        {
            return (string)obj.GetValue(FileNameProperty);
        }

        public static void SetFileName(DependencyObject obj, string value)
        {
            obj.SetValue(FileNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for FileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.RegisterAttached("FileName", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata(""));



        public static double GetScale(DependencyObject obj)
        {
            return (double)obj.GetValue(ScaleProperty);
        }

        public static void SetScale(DependencyObject obj, double value)
        {
            obj.SetValue(ScaleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Scale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.RegisterAttached("Scale", typeof(double), typeof(MainWindowViewModel), new UIPropertyMetadata(1.0, (s, e) => { }, (s, e) => { return Math.Max(0.1, double.Parse(e.ToString())); }));




        public string[] Templates { get { return GetTemplateList(); } }

        #region Commands

        public ICommand ExitCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { Exit(); }
                    );
            }
        }



        public ICommand NewCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { New(); }
                    );
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new SimpleCommand<Form>(
                    p => { Close(p); },
                    p => { return p != null; }
                    );
            }
        }

        public ICommand SaveAsCommand
        {
            get
            {
                return new SimpleCommand<Form>(
                    p => { SaveAs(p); },
                    p => { return p != null; }
                    );
            }
        }

        public ICommand SaveAsTemplateCommand
        {
            get
            {
                return new SimpleCommand<Form>(
                    p => { SaveAsTemplate(p); },
                    p => { return p != null; }
                    );
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new SimpleCommand<Form>(
                    p => { Save(p); },
                    p => { return p != null; }
                    );
            }
        }

        public ICommand OpenCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { Open(); }
                    );
            }
        }

        public ICommand PrintCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { Print(); },
                    p => { return p != null; }
                    );
            }
        }

        public ICommand PrintPreviewCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { PrintPreview(); },
                    p => { return p != null; }
                    );
            }
        }

        public ICommand PrintCalibratePageCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { PrintCalibratePage(); }
                    );
            }
        }



        public ICommand OpenTemplateCommand
        {
            get
            {
                return new SimpleCommand<string>(
                    p => { Open(Path.Combine(templatePath, p)); }
                    );
            }
        }

        public ICommand AddItemCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { AddItem(); },
                    p => IsInDesign
                    );
            }
        }

        public ICommand RemoveItemCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { RemoveItem(); },
                    p => IsInDesign
                    );
            }
        }

        public ICommand CopyCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { Copy(); },
                    p => IsInDesign
                    );
            }
        }

        public ICommand ItemUpCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { ItemUp(); },
                    p => IsInDesign
                    );
            }
        }

        public ICommand ItemDownCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { ItemDown(); },
                    p => IsInDesign
                    );
            }
        }



        public ICommand ZoomInCommand
        {
            get
            {
                return new SimpleCommand<Form>(
                    p => { ZoomIn(p); },
                    p => { return p != null; }
                    );
            }
        }

        public ICommand ZoomOutCommand
        {
            get
            {
                return new SimpleCommand<Form>(
                    p => { ZoomOut(p); },
                    p => { return p != null; }
                    );
            }
        }

        public ICommand ZoomNormalCommand
        {
            get
            {
                return new SimpleCommand<Form>(
                    p => { ZoomNormal(p); },
                    p => { return p != null; }
                    );
            }
        }

        public ICommand FileAssotiateCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { FileAssotiate(); }
                    );
            }
        }


        public ICommand TableModeOnOffCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { IsTableMode = !IsTableMode; }
                    );
            }
        }

        public ICommand DesingModeOnOffCommand
        {
            get
            {
                return new SimpleCommand(
                    p => { IsInDesign = !IsInDesign; }
                    );
            }
        }


        #endregion

        public MainWindowViewModel()
        {
            Application.Current.MainWindow.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

            CheckAndRegisterForSingleInstance();

            Forms = new ObservableCollection<Form>();


            var args = Environment.GetCommandLineArgs();
            foreach (var arg in args.Where(a => a.Contains(".frmx") | a.Contains(".frtx")).Where(f => File.Exists(f)))
            {
                Open(arg);
            }


            if (CurrentForm == null)
                LoadTest();

        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !Exit();
        }

        private void CheckAndRegisterForSingleInstance()
        {
            //Если это первый запуск приложения и другое окно не активировалось - зарегесртировать слушателя сообщений
            if (SingleInstance.IsFirstInstance)
            {
                try
                {
                    SingleInstance.ListenFromOtherInstance(file =>
                    {
                        var form = Forms.FirstOrDefault(f => MainWindowViewModel.GetFileName(f) == file);
                        if (form != null)
                            CurrentForm = form;
                        else
                            Open(file);

                    });
                }
                catch (Exception ex)
                {

                    akarov.Controls.Exceptions.ShowException.Show(ex);
                }
            }
        }

        private void New()
        {
            var form = new Form() { Name = "Новый бланк" };
            var page = new Model.Page() { Name = "Страница 1", Height = 29.7, Width = 21 };
            page.Elements.Add(new Element() { Name = "Новое поле", Value = "Новое поле" });
            form.Pages.Add(page);
            Forms.Add(form);
            CurrentForm = form;
            CurrentForm.HasChanges = false;
        }


        private bool Save(Form form)
        {
            var filePath = MainWindowViewModel.GetFileName(form);
            if (string.IsNullOrEmpty(filePath))
            {
                return SaveAs(form);
            }
            else
            {
                Save(form, filePath);
                return true;
            }
        }

        private bool SaveAs(Form form)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Файлы бланков (*.frmx)|*.frmx|Файлы шаблонов бланков (*.frtx)|*.frtx";
            dialog.FileName = CurrentForm.Name + ".frmx";

            if (dialog.ShowDialog() == true && dialog.FileName.Length > 0)
            {
                Save(form, dialog.FileName);
                return true;
            }
            else
                return false;
        }

        private bool SaveAsTemplate(Form form)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Файлы шаблонов бланков (*.frtx)|*.frtx";
            dialog.FileName = CurrentForm.Name + ".frmx";
            dialog.InitialDirectory = templatePath;

            if (dialog.ShowDialog() == true && dialog.FileName.Length > 0)
            {
                Save(form, dialog.FileName);
                MainWindowViewModel.SetFileName(form, "");
                return true;
            }
            else
                return false;
        }

        private void Save(Form form, string fileName)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);
            CurrentForm.Name = name;

            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Form));

            System.IO.StreamWriter file = new System.IO.StreamWriter(
                fileName);
            writer.Serialize(file, form);
            file.Close();



            MainWindowViewModel.SetFileName(form, fileName);

            form.HasChanges = false;
        }


        private void Open()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Файлы бланков (*.frmx)|*.frmx|Файлы шаблонов бланков (*.frtx)|*.frtx";


            if (dialog.ShowDialog() == true && dialog.FileName.Length > 0)
            {

                Open(dialog.FileName);

            }
        }


        private bool Close(Form form)
        {
            if (form != null)
            {
                if (form.HasChanges)
                {
                    var res = MessageBox.Show("Форма {0} содержит несохраненные данные!\r\nСохранить изменения?".F(form.Name),
                    "Несохраненные изменения на форме " + form.Name, MessageBoxButton.YesNoCancel, MessageBoxImage.Stop, MessageBoxResult.Yes);

                    if ((res == MessageBoxResult.Yes && !Save(form))
                        || res == MessageBoxResult.Cancel)
                        return false;

                }

                Forms.Remove(form);
                CurrentForm = Forms.FirstOrDefault();

                return true;
            }

            return false;
        }

        private void Open(string filePath)
        {
            System.Xml.Serialization.XmlSerializer reader =
                     new System.Xml.Serialization.XmlSerializer(typeof(Form));
            System.IO.StreamReader file = new System.IO.StreamReader(
                filePath);
            var form = (Form)reader.Deserialize(file);
            file.Close();
            var name = Path.GetFileNameWithoutExtension(filePath);
            form.Name = name;
            Forms.Add(form);
            CurrentForm = form;

            CurrentForm.HasChanges = false;

            if (Path.GetExtension(filePath) == ".frmx")
                MainWindowViewModel.SetFileName(form, filePath);
        }

        private string[] GetTemplateList()
        {
            return Directory.GetFiles(templatePath, "*.frtx").Select(f => Path.GetFileName(f)).ToArray();
        }


        private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            try
            {
                System.Drawing.Bitmap bitmap;
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                    enc.Save(outStream);
                    outStream.Position = 0;
                    bitmap = new System.Drawing.Bitmap(outStream);
                }
                return bitmap;
            }
            catch (Exception)
            {

                return null;
            }
        }

        private PrintDocument GeneratePrintDocuemnt(Form form, PrinterSettings settings, bool printPreview = false)
        {
            int pageTo;
            int pageFrom;
            int pageNo;

            //Константа для перевода см в сотые доли дюйма
            float dpiConst = 100F / 2.54F;

            if (settings.PrintRange == PrintRange.SomePages)
            {

                pageNo = pageFrom = Math.Max(settings.FromPage - 1, 0);
                pageTo = Math.Min(settings.ToPage - 1, form.Pages.Count - 1);

            }
            else if (settings.PrintRange == PrintRange.Selection)
            {
                pageNo = pageTo = pageFrom = CurrentForm.Pages.IndexOf(SelectedItem as Model.Page);
            }
            else
            {
                pageNo = pageFrom = 0;
                pageTo = form.Pages.Count - 1;
            }



            PrintDocument printDoc = new PrintDocument();

            printDoc.PrinterSettings = settings;
            printDoc.PrintPage += (s, e) =>
            {


                var page = form.Pages[pageNo];

                if (printPreview)
                {
                    System.Drawing.Image bp = BitmapFromSource(page.Image);

                    if (bp != null)
                        e.Graphics.DrawImage(bp, new RectangleF((float)page.ImageLeft * dpiConst - e.PageSettings.HardMarginX,
                                                                (float)page.ImageTop * dpiConst - e.PageSettings.HardMarginY,
                                                                (float)page.Image.Width / 96f * 100f,
                                                                (float)page.Image.Height / 96F * 100f));

                    //printPreview = false; // после первого предпросмотра больше картинку на лист не выводить, чтобы она не печаталсь когда делашеь печать из окна превью
                }

                var pen = System.Drawing.Pens.Black;
                SolidBrush brush = new SolidBrush(System.Drawing.Color.Black);
                StringFormat format = new StringFormat()
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near,
                    Trimming = StringTrimming.None,
                    FormatFlags = StringFormatFlags.NoClip
                };

                foreach (var element in page.Elements)
                {
                    if (element.ElementType == ElementType.Anchor)
                        continue; //пропускаем и не рисуем якоря

                    //При выовде на принтер необходиме указывать координаты в сотых долях дюйма, при этом нужно отнимать
                    //физические границы принетра
                    float x = (float)element.X * dpiConst - e.PageSettings.HardMarginX;
                    float y = (float)element.Y * dpiConst - e.PageSettings.HardMarginY;
                    float height = (float)element.Height * dpiConst;
                    float width = (float)element.Width * dpiConst;

                    RectangleF rec = new RectangleF(x, y, width, height);



                    //e.Graphics.DrawEllipse(pen, x, y, 1, 1);
                    //e.Graphics.DrawRectangle(Pens.Black, x, y, width, height);

                    if (element.ElementType == ElementType.Check && element.Value.ToLower() == "x")
                    {
                        Font font = new Font(element.FontName, element.FontSize, System.Drawing.FontStyle.Bold);

                        e.Graphics.DrawString("X",
                                            font,
                                            brush, x, y, format);
                    }
                    else
                    {
                        Font font = new Font(element.FontName, element.FontSize, element.FontStyle);
                        e.Graphics.DrawString(element.Value,
                                            font,
                                            brush, x, y, format);
                    }


                    printPreview = false; // после первого предпросмотра больше картинку на лист не выводить, чтобы она не печаталсь когда делашеь печать из окна превью
                }

                e.HasMorePages = (++pageNo <= pageTo);
                if (!e.HasMorePages)
                    pageNo = pageFrom;
            };

            return printDoc;
        }

        private void Print()
        {


            var printDlg = new System.Windows.Forms.PrintDialog();
            printDlg.AllowSomePages = true;
            printDlg.PrinterSettings.FromPage = 1;
            printDlg.PrinterSettings.ToPage = CurrentForm.Pages.Count;
            printDlg.AllowSelection = SelectedItem is Model.Page;
            if (printDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                printDlg.Document = GeneratePrintDocuemnt(CurrentForm, printDlg.PrinterSettings);
                printDlg.Document.Print();
            }
        }



        private void PrintPreview()
        {
            var dialog = new System.Windows.Forms.PrintPreviewDialog();

            dialog.Document = GeneratePrintDocuemnt(CurrentForm, new System.Windows.Forms.PrintDialog().PrinterSettings, true);
            dialog.Show();

        }

        private void PrintCalibratePage()
        {
            var dialog = new System.Windows.Forms.PrintPreviewDialog();

            dialog.Document = new PrintDocument();
            //dialog.Document.PrinterSettings = dialog.PrinterSettings;
            dialog.Document.PrintPage += (s, e) =>
            {
                Font font1 = new Font("Arial", 11, System.Drawing.FontStyle.Regular);

                // Construct a new Rectangle .
                Rectangle displayRectangle =
                    new Rectangle(40, 40, 180, 180);

                // Construct 2 new StringFormat objects
                StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
                StringFormat format2 = new StringFormat(format1);

                // Set the LineAlignment and Alignment properties for
                // both StringFormat objects to different values.
                format1.LineAlignment = StringAlignment.Near;
                format1.Alignment = StringAlignment.Center;

                format2.LineAlignment = StringAlignment.Center;
                format2.Alignment = StringAlignment.Far;

                // Draw the bounding rectangle and a string for each
                // StringFormat object.
                e.Graphics.DrawRectangle(Pens.Black, displayRectangle);
                e.Graphics.DrawString("Showing Format1", font1,
                    System.Drawing.Brushes.Red, (RectangleF)displayRectangle, format1);
                e.Graphics.DrawString("Showing Format2", font1,
                    System.Drawing.Brushes.Red, (RectangleF)displayRectangle, format2);

                return;
                var x = e.PageSettings.HardMarginX;
                var y = e.PageSettings.HardMarginY;

                double lineHeightCm = 1;
                double lineWidthCm = 1;

                float lineHeight = (float)CentimeterToPixelConverter.ConvertY(lineHeightCm);
                float lineWidth = (float)CentimeterToPixelConverter.ConvertX(lineWidthCm);

                float pWidth = e.PageSettings.PaperSize.Width;
                float pHight = e.PageSettings.PaperSize.Height;

                var pen = System.Drawing.Pens.Black;
                SolidBrush brush = new SolidBrush(System.Drawing.Color.Black);
                Font font = new Font("Arial", 11, System.Drawing.FontStyle.Regular);

                e.Graphics.DrawLine(pen, 200 - x, 500 - y, 200 - x, 600 - y);
                e.Graphics.DrawString("2 дюйма = 5,08 см", font, brush, 220F, 550F);

                e.Graphics.DrawLine(pen, 10F / 2.54F * 100F - x, 10F / 2.54F * 100F - y, 10F / 2.54F * 100F - x, 15F / 2.54F * 100F - y);
                e.Graphics.DrawLine(pen, 10F / 2.54F * 100F - x, 10F / 2.54F * 100F - y, 15F / 2.54F * 100F - x, 10F / 2.54F * 100F - y);
                e.Graphics.DrawString("10 см от верхнего и левого краев", font, brush, 10.5F / 2.54F * 100F, 10.5F / 2.54F * 100F);

                e.Graphics.DrawLine(pen, 15F / 2.54F * 100F - x, 15F / 2.54F * 100F - y, 15F / 2.54F * 100F - x, 17F / 2.54F * 100F - y);
                e.Graphics.DrawLine(pen, 15F / 2.54F * 100F - x, 15F / 2.54F * 100F - y, 17F / 2.54F * 100F - x, 15F / 2.54F * 100F - y);
                e.Graphics.DrawString("15 см от верхнего и левого краев", font, brush, 15.5F / 2.54F * 100F, 15.5F / 2.54F * 100F);

                e.Graphics.DrawLine(pen, 19F / 2.54F * 100F - x, 25F / 2.54F * 100F - y, 19F / 2.54F * 100F - x, 28F / 2.54F * 100F - y);
                e.Graphics.DrawLine(pen, 19F / 2.54F * 100F - x, 25F / 2.54F * 100F - y, 21F / 2.54F * 100F - x, 25F / 2.54F * 100F - y);
                e.Graphics.DrawString("25 от верхнего и\r\n 19 см от левого краев", font, brush, 18F / 2.54F * 100F, 26F / 2.54F * 100F);

                ////1. top-left conner
                //e.Graphics.DrawLine(pen, lineWidth * 0.5F, lineHeight, lineWidth * 1.5F, lineHeight);
                //e.Graphics.DrawLine(pen, lineWidth, lineHeight * 0.5F, lineWidth, lineHeight * 1.5F);
                //e.Graphics.DrawString(string.Format("{0:N1}:{1:N1}", lineWidthCm, lineHeightCm), font, brush, lineWidth * 2F, lineHeight * 2f);

                ////2. top-right conner
                //e.Graphics.DrawLine(pen, e.PageSettings.PaperSize.Width - lineWidth * 1.5F, lineHeight, e.PageSettings.PaperSize.Width - lineWidth * 0.5F, lineHeight);
                //e.Graphics.DrawLine(pen, e.PageSettings.PaperSize.Width - lineWidth, lineHeight * 0.5F, e.PageSettings.PaperSize.Width - lineWidth, lineHeight * 1.5F);
                //e.Graphics.DrawString(string.Format("{0:N1}:{1:N1}", CentimeterToPixelConverter.ConvertToCmX(e.PageSettings.PaperSize.Width - lineWidth), lineHeightCm), font, brush, e.PageSettings.PaperSize.Width - lineWidth * 3F, lineHeight * 2f);

                ////3. bottom-left conner
                //e.Graphics.DrawLine(pen, lineWidth * 0.5F, e.PageSettings.PaperSize.Height - lineHeight, lineHeight * 1.5F, e.PageSettings.PaperSize.Height - lineHeight);
                //e.Graphics.DrawLine(pen, lineWidth, e.PageSettings.PaperSize.Height - lineHeight * 1.5F, lineWidth, e.PageSettings.PaperSize.Height - lineHeight * 0.5F);
                //e.Graphics.DrawString(string.Format("{0:N1}:{1:N1}", lineWidthCm, CentimeterToPixelConverter.ConvertToCmY(e.PageSettings.PaperSize.Height - lineHeight)), font, brush, lineWidth * 2F, e.PageSettings.PaperSize.Height - lineHeight * 2f);

                ////4. bottom-right
                //e.Graphics.DrawLine(pen, e.PageSettings.PaperSize.Width - lineWidth * 1.5F, e.PageSettings.PaperSize.Height - lineHeight, e.PageSettings.PaperSize.Width - lineWidth * 0.5F, e.PageSettings.PaperSize.Height - lineHeight);
                //e.Graphics.DrawLine(pen, e.PageSettings.PaperSize.Width - lineWidth, e.PageSettings.PaperSize.Height - lineHeight * 1.5F, e.PageSettings.PaperSize.Width - lineWidth, e.PageSettings.PaperSize.Height - lineHeight * 0.5F);
                //e.Graphics.DrawString(string.Format("{0:N1}:{1:N1}", CentimeterToPixelConverter.ConvertToCmX(e.PageSettings.PaperSize.Width - lineWidth), CentimeterToPixelConverter.ConvertToCmY(e.PageSettings.PaperSize.Height - lineHeight)), font, brush, e.PageSettings.PaperSize.Width - lineWidth * 3F, e.PageSettings.PaperSize.Height - lineHeight * 2f);

                ////5. Center
                //e.Graphics.DrawLine(pen, e.PageSettings.PaperSize.Width / 2 - lineWidth / 2, e.PageSettings.PaperSize.Height / 2, e.PageSettings.PaperSize.Width / 2 + lineWidth / 2, e.PageSettings.PaperSize.Height / 2);
                //e.Graphics.DrawLine(pen, e.PageSettings.PaperSize.Width / 2, e.PageSettings.PaperSize.Height / 2 - lineHeight / 2, e.PageSettings.PaperSize.Width / 2, e.PageSettings.PaperSize.Height / 2 + lineHeight / 2);
                //e.Graphics.DrawString(string.Format("{0:N1}:{1:N1}", CentimeterToPixelConverter.ConvertToCmX(e.PageSettings.PaperSize.Width / 2), CentimeterToPixelConverter.ConvertToCmY(e.PageSettings.PaperSize.Height / 2)), font, brush, e.PageSettings.PaperSize.Width / 2 + lineWidth, e.PageSettings.PaperSize.Height / 2 + lineHeight);


                //6. Page size
                e.Graphics.DrawString(string.Format("Размер страницы {0:N1}х{1:N1} см.", e.PageSettings.PaperSize.Width / 100F * 2.54,
                                               e.PageSettings.PaperSize.Height / 100F * 2.54), font, brush, lineWidth * 2F, lineHeight * 5f);

                e.Graphics.DrawString(string.Format("Размер печатной области {0:N1}х{1:N1} см.", e.PageSettings.PrintableArea.Width / 100F * 2.54,
                                               e.PageSettings.PrintableArea.Height / 100F * 2.54), font, brush, lineWidth * 2F, lineHeight * 6f);


                e.Graphics.DrawString(string.Format("Физический отступ принтера {0:N1}х{1:N1} см.", e.PageSettings.HardMarginX / 100F * 2.54,
                                             e.PageSettings.HardMarginY / 100F * 2.54), font, brush, lineWidth * 2F, lineHeight * 8f);

                //7. Риски по границам
                //e.Graphics.DrawLine(pen, e.PageSettings.PrintableArea.X, e.PageSettings.PrintableArea.Y, e.PageSettings.PrintableArea.X, e.PageSettings.PrintableArea.Y + lineWidth);
                //e.Graphics.DrawLine(pen, e.PageSettings.PrintableArea.X, e.PageSettings.PrintableArea.Y, e.PageSettings.PrintableArea.X + lineHeight, e.PageSettings.PrintableArea.Y);
                //e.Graphics.DrawLine(pen,e.PageSettings.PrintableArea.Right,e.PageSettings.PrintableArea.Bottom,e.PageSettings.PrintableArea.Right, e.PageSettings.PrintableArea.Bottom - lineHeight);
                //e.Graphics.DrawLine(pen,e.PageSettings.PrintableArea.Right,e.PageSettings.PrintableArea.Bottom,e.PageSettings.PrintableArea.Right - lineWidth, e.PageSettings.PrintableArea.Bottom);

            };

            dialog.ShowDialog();

        }


        private void AddItem()
        {
            if (SelectedItem is Model.Page)
            {

                var page = SelectedItem as Model.Page;
                var element = new Element() { Name = "Новое поле " + (page.Elements.Count + 1), Value = "Новое поле " + (page.Elements.Count + 1) };
                page.Elements.Add(element);
                SelectedItem = element;
            }
            else if (SelectedItem is Element)
            {

                var page = CurrentForm.Pages.Single(p => p.Elements.Contains(SelectedItem as Element));
                var element = new Element() { Name = "Новое поле " + (page.Elements.Count + 1), Value = "Новое поле " + (page.Elements.Count + 1) };
                page.Elements.Add(element);
                SelectedItem = element;
            }
            else if (CurrentForm != null)
            {
                var page = new formPrinter.Model.Page() { Name = "Новая страница " + (CurrentForm.Pages.Count + 1) };
                CurrentForm.Pages.Add(page);
                SelectedItem = page;
            }
        }

        private void RemoveItem()
        {
            if (SelectedItem is Model.Page)
            {
                var page = SelectedItem as Model.Page;
                if (MessageBox.Show("Вы действительно хотите удалить странцу " + page.Name + "?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
                    return;

                CurrentForm.Pages.Remove(page);
                SelectedItem = CurrentForm;
            }
            else if (SelectedItem is Element)
            {
                var element = SelectedItem as Model.Element;
                if (MessageBox.Show("Вы действительно хотите удалить поле " + element.Name + "?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
                    return;

                foreach (var page in CurrentForm.Pages)
                {
                    if (page.Elements.IndexOf(element) > -1)
                    {
                        page.Elements.Remove(element);
                        SelectedItem = page;
                        break;
                    }
                }
            }
        }

        private void Copy()
        {
            if (SelectedItem is Element)
            {
                var element = SelectedItem as Element;
                var page = CurrentForm.Pages.Single(p => p.Elements.Contains(element));
                var newElement = element.Clone();
                page.Elements.Insert(page.Elements.IndexOf(element) + 1, newElement);
                SelectedItem = newElement;
            }
            else if (SelectedItem is Model.Page)
            {

                var page = SelectedItem as Model.Page;
                var newPage = page.Clone();
                CurrentForm.Pages.Insert(CurrentForm.Pages.IndexOf(page) + 1, newPage);
                SelectedItem = newPage;
            }
        }

        private void ItemUp()
        {
            if (SelectedItem is Element)
            {
                var element = SelectedItem as Element;
                var page = CurrentForm.Pages.Single(p => p.Elements.Contains(element));
                int index = page.Elements.IndexOf(element);
                if (index > 0)
                {
                    page.Elements.RemoveAt(index);
                    page.Elements.Insert(--index, element);
                    SelectedItem = element;
                }
            }
            else if (SelectedItem is Model.Page)
            {

                var page = SelectedItem as Model.Page;
                int index = CurrentForm.Pages.IndexOf(page);
                if (index > 0)
                {
                    CurrentForm.Pages.RemoveAt(index);
                    CurrentForm.Pages.Insert(--index, page);
                    SelectedItem = page;
                }

            }
        }

        private void ItemDown()
        {
            if (SelectedItem is Element)
            {
                var element = SelectedItem as Element;
                var page = CurrentForm.Pages.Single(p => p.Elements.Contains(element));
                int index = page.Elements.IndexOf(element);
                if (index < page.Elements.Count - 1)
                {
                    page.Elements.RemoveAt(index);
                    page.Elements.Insert(++index, element);
                    SelectedItem = element;
                }
            }
            else if (SelectedItem is Model.Page)
            {

                var page = SelectedItem as Model.Page;
                int index = CurrentForm.Pages.IndexOf(page);
                if (index < CurrentForm.Pages.Count - 1)
                {
                    CurrentForm.Pages.RemoveAt(index);
                    CurrentForm.Pages.Insert(++index, page);
                    SelectedItem = page;
                }

            }
        }

        private void ZoomIn(Form p)
        {
            double zoom = MainWindowViewModel.GetScale(p);
            zoom *= 1.1D;
            MainWindowViewModel.SetScale(p, zoom);
        }

        private void ZoomOut(Form p)
        {
            double zoom = MainWindowViewModel.GetScale(p);
            zoom /= 1.1D;
            MainWindowViewModel.SetScale(p, zoom);
        }

        private void ZoomNormal(Form p)
        {

            MainWindowViewModel.SetScale(p, 1.0);
        }

        private void FileAssotiate()
        {
            try
            {
                FileAssotiation.AssotiateExtention(".frmx");
                FileAssotiation.AssotiateExtention(".frtx");
                MessageBox.Show("Файлы успешно ассоциированы с программой реФормер", "реФормер", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {

                ShowException.Show(ex);
            }
        }

        private bool Exit()
        {
            //if (Forms.Any(f => f.HasChanges))
            //{
            //    var res = MessageBox.Show("Некоторые формы содержат несохраненные данные!\r\nСохранить изменения?",
            //       "Несохраенные изменения", MessageBoxButton.YesNoCancel, MessageBoxImage.Stop, MessageBoxResult.Yes);

            //    if (res == MessageBoxResult.Yes && !Forms.All(f=>Save(f)))
            //        return false;
            //    else if (res == MessageBoxResult.Cancel)
            //        return false;
            //}

            Form[] forms = new Form[Forms.Count];
            Forms.CopyTo(forms, 0);

            if (!forms.All(f => Close(f)))
                return false;

            Application.Current.Shutdown();
            return true;
        }

        private void LoadTest()
        {

            using (Stream file = Application.GetResourceStream(new Uri("pack://application:,,,/new.frmx")).Stream)
            {
                file.Position = 0;
                System.Xml.Serialization.XmlSerializer reader =
                         new System.Xml.Serialization.XmlSerializer(typeof(Form));

                var form = (Form)reader.Deserialize(file);
                Forms.Add(form);
                form.HasChanges = false;
                CurrentForm = form;
            }


            return;



            var Form = new Form();
            Form.Name = "Мой бланк 1";
            Forms.Add(Form);

            CurrentForm = Form;

            var page = new formPrinter.Model.Page();

            page.Width = 21;
            page.Height = 15;
            page.Name = "Страница 1";


            Stream picStream = Application.GetResourceStream(new Uri("pack://application:,,,/новый файл.jpg")).Stream;
            picStream.Position = 0;
            byte[] bytes = new byte[picStream.Length];
            picStream.Read(bytes, 0, (int)picStream.Length);
            page.Image = formPrinter.Model.Page.ImageFromBytes(bytes);

            Form.Pages.Add(page);

            var element = new Element();
            element.Name = "Number";
            element.X = 3;
            element.Y = 5;
            element.Height = 1;
            element.Width = 6;
            element.Value = "Some text";
            page.Elements.Add(element);

            element = new Element();
            element.Name = "Date";
            element.ElementType = ElementType.Calendar;
            element.X = 7;
            element.Y = 4;
            element.Height = 1;
            element.Width = 6;
            element.Value = "01.05.2012";
            page.Elements.Add(element);


            element = new Element();
            element.Name = "Date";
            element.ElementType = ElementType.List;
            element.ListChoisesSeparetedText = "apple;mint;berry";

            element.X = 3;
            element.Y = 8;
            element.Height = 1;
            element.Width = 6;
            element.Value = "apple";
            page.Elements.Add(element);

            element = new Element();
            element.Name = "Date";
            element.ElementType = ElementType.Text;
            element.X = 13;
            element.Y = 5;
            element.Height = 1;
            element.Width = 6;
            element.Value = "True";
            page.Elements.Add(element);

            page = new formPrinter.Model.Page();



            page.Image = formPrinter.Model.Page.ImageFromBytes(bytes);

            page.Width = 21;
            page.Height = 29.7;
            page.Name = "Page 2";

            Form.Pages.Add(page);

            element = new Element();
            element.Name = "Number";
            element.X = 3;
            element.Y = 5;
            element.Height = 1;
            element.Width = 6;
            element.Value = "Some text";
            page.Elements.Add(element);
        }
    }
}
