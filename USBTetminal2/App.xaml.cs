using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Infrastructure;

namespace USBTetminal2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException_1(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //default VS Exceptions are better
            //string stackTrace = "Stack Trace failed!";
            //if (e.Exception != null && e.Exception.StackTrace != null)
            //   stackTrace =  e.Exception.StackTrace;
            //MessageBox.Show("Got dispatcher exception!" + Environment.NewLine + "Stacktrace : " + stackTrace);
            //MessageBox.Show("Method that throw an exception: " + Environment.NewLine + "... or TargetSite" + Environment.NewLine + e.Exception.TargetSite);
            //MessageBox.Show("Exception message :" + Environment.NewLine + e.Exception.Message);

            //e.Handled = true;


           

        }

        Bootstrapper bootstrapper = new Bootstrapper();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Microsoft.Practices.Prism.Modularity.ModulesConfigurationSection t;

            //  Ошибка при создании обработчика раздела конфигурации для modules: Не удалось загрузить файл или сборку "Microsoft.Practices.Prism" либо одну из их зависимостей. Не удается найти указанный файл. (C:\Users\Zhenja\Documents\Visual Studio 2012\Projects\MyFirstPrismUnityApp\MyFirstPrismUnityApp\bin\Debug\MyFirstPrismUnityApp.vshost.exe.Config line 7)
            // The boostrapper will create the Shell instance, so the App.xaml does not have a StartupUri.

            bootstrapper.Run();

            //DO TO: create installer and export this code to installer
            string root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string myAppData = Path.Combine(root, AppDirectories.AppDataFolder);

            if (!Directory.Exists(myAppData))
            {
                Directory.CreateDirectory(myAppData);
            }

            string excelTempFolder = Path.Combine(myAppData, AppDirectories.TempFolder);
            if (!Directory.Exists(excelTempFolder))
            {
                Directory.CreateDirectory(excelTempFolder);
            }
        }
    }
}
