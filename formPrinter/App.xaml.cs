using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using akarov.Controls.Utils;

namespace formPrinter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            akarov.Controls.Exceptions.ShowException.Show(e.Exception);

        }



    }

    public partial class StartApp : Application
    {
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static void Main()
        {
            if (!SingleInstance.IsFirstInstance && SingleInstance.ActivatePreviousInstance()) //а если второе - передать все файлы на открытие и закрыть данный экзмепляр
            {
                try
                {
                    var args = Environment.GetCommandLineArgs();
                    foreach (var arg in args.Where(a => a.Contains(".frmx") | a.Contains(".frtx")).Where(f => File.Exists(f)))
                    {
                        SingleInstance.SendMessage(arg);
                    }

                    
                }
                catch (Exception ex)
                {

                    
                }
            }
            else
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();
            }


        }
    }

}
