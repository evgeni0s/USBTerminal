
    
using System;
using System.Windows;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModule;
using USBTetminal2.Controls;


namespace USBTetminal2
{

    public class Bootstrapper : UnityBootstrapper
    {
        //private readonly Logger _logger = new Logger();

        //protected override ILoggerFacade CreateLogger()
        //{
        //    return this._logger;
        //}

        //CallbackLogger in tutorial
        private readonly CustomRichTextBox _logger = new CustomRichTextBox();
        protected override ILoggerFacade CreateLogger()
        {
            return this._logger;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        //здесь вроди нужно связать интерфейсы с имплементашкой
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            this.RegisterTypeIfMissing(typeof(ITestModule), typeof(TestModule.TestModule), true);
            /*
             RegisterInstance - extension method from namespace using Microsoft.Practices.Unity;
             * this namespace comes from Nuget Unity package. If Something does not work, go to solution 
             * -> manage Nuget -> remove and add again checkbox over Unity pakage. 
             * If there is no unity package, then install from console
             
             */
            this.Container.RegisterInstance<CustomRichTextBox>(this._logger);
           
        }

        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<Shell>();
        }


        //protected override IModuleCatalog CreateModuleCatalog()
        //{
        //    return new ConfigurationModuleCatalog();
        //}

        //protected override void ConfigureModuleCatalog()
        //{
            //Type moduleBType = typeof(TestModule.TestModule);
            //Type moduleAType = typeof(ModuleA);
           // ModuleCatalog.AddModule(new ModuleInfo(moduleBType.Name, moduleBType.AssemblyQualifiedName) { InitializationMode = InitializationMode.WhenAvailable });
            //     Type moduleAType = typeof(ModuleA);
            //   ModuleCatalog.AddModule(new ModuleInfo(moduleAType.Name, moduleAType.AssemblyQualifiedName) { InitializationMode = InitializationMode.WhenAvailable });
            //Type moduleCType = typeof(ModuleC.ModuleC);
            //ModuleCatalog.AddModule(new ModuleInfo(moduleCType.Name, moduleCType.AssemblyQualifiedName) { InitializationMode = InitializationMode.WhenAvailable });

            //18_01_2015
            //base.ConfigureModuleCatalog();
            //ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            //moduleCatalog.AddModule(typeof(TestModule.TestModule));



        //}





    }
}
