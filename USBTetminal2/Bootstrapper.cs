using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace USBTetminal2
{
    public class Bootstrapper : UnityBootstrapper
    {
        private readonly Logger _logger = new Logger();

        protected override ILoggerFacade CreateLogger()
        {

            return _logger;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        //здесь вроди нужно связать интерфейсы с имплементашкой
        //protected override void ConfigureContainer()
        //{
        //    base.ConfigureContainer();
        //}

        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<Shell>();
        }


        //protected override IModuleCatalog CreateModuleCatalog()
        //{
        //    return new ConfigurationModuleCatalog();
        //}

        protected override void ConfigureModuleCatalog()
        {
            Type moduleBType = typeof(TestModule.TestModule);
            //Type moduleAType = typeof(ModuleA);
            ModuleCatalog.AddModule(new ModuleInfo(moduleBType.Name, moduleBType.AssemblyQualifiedName) { InitializationMode = InitializationMode.WhenAvailable });
            //     Type moduleAType = typeof(ModuleA);
            //   ModuleCatalog.AddModule(new ModuleInfo(moduleAType.Name, moduleAType.AssemblyQualifiedName) { InitializationMode = InitializationMode.WhenAvailable });
            //Type moduleCType = typeof(ModuleC.ModuleC);
            //ModuleCatalog.AddModule(new ModuleInfo(moduleCType.Name, moduleCType.AssemblyQualifiedName) { InitializationMode = InitializationMode.WhenAvailable });


        }





    }
}
