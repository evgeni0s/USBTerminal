using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    //[Module(ModuleName = "TestModule")]

    public class TestModule : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;
        private IUnityContainer container;
        private IRegionManager regionManager;

        public TestModule(IRegionViewRegistry regionViewRegistry, IUnityContainer container, IRegionManager regionManager)
        {
            this.regionViewRegistry = regionViewRegistry;
            this.container = container;
            this.regionManager = regionManager;
            var myRegion = this.regionManager.Regions["TestRegion"];

            myRegion.Add(this.container.Resolve<Views.TestModuleView>());
        }

        public void Initialize()
        {
            int i = 0;
            i++;
        }
    }
}
