using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace USBTetminal2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException_1(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            string stackTrace = "Stack Trace failed!";
            if (e.Exception != null && e.Exception.StackTrace != null)
               stackTrace =  e.Exception.StackTrace;
            MessageBox.Show("Got dispatcher exception!" + Environment.NewLine + "Stacktrace : " + stackTrace);
            MessageBox.Show("Method that throw an exception: " + Environment.NewLine + "... or TargetSite" + Environment.NewLine + e.Exception.TargetSite);
            MessageBox.Show("Exception message :" + Environment.NewLine + e.Exception.Message);

            e.Handled = true;
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Microsoft.Practices.Prism.Modularity.ModulesConfigurationSection t;

            //  Ошибка при создании обработчика раздела конфигурации для modules: Не удалось загрузить файл или сборку "Microsoft.Practices.Prism" либо одну из их зависимостей. Не удается найти указанный файл. (C:\Users\Zhenja\Documents\Visual Studio 2012\Projects\MyFirstPrismUnityApp\MyFirstPrismUnityApp\bin\Debug\MyFirstPrismUnityApp.vshost.exe.Config line 7)
            // The boostrapper will create the Shell instance, so the App.xaml does not have a StartupUri.
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
