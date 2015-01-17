using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel
{
    public class ExcelModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;
        private IUnityContainer container;
        private IRegionManager regionManager;

        public ExcelModule(IRegionViewRegistry regionViewRegistry, IUnityContainer container, IRegionManager regionManager)
        {
            this.regionViewRegistry = regionViewRegistry;
            this.container = container;
            this.regionManager = regionManager;
            var myRegion = this.regionManager.Regions["TestRegion"];

            myRegion.Add(this.container.Resolve<Views.ExcelView>());
        }
    }
}
