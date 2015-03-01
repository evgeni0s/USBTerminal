using Infrastructure;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderBrowser;
using FolderBrowser.ViewModel;
using ExportModule.Views;
using System.IO;

namespace ExportModule
{
    public class ExportModule : IModule, IExportModule
    {
        private IRegionManager _regionManager;
        private ILoggerFacade _logger;
        private IViewModelProvider _viewModelProvider;
        public ExportModule(IRegionManager regionManager, ILoggerFacade logger, IViewModelProvider viewModelProvider)
        {
            _regionManager = regionManager;
            _viewModelProvider = viewModelProvider;
            _logger = logger;
        }

        public void ShowFolderBrowserView(Action<string> selectedPathCallback)
        {
            IRegion main = _regionManager.Regions[RegionNames.MainRegion];
            var view = main.GetView("FolderBrowserKey");
            if (view == null)
            {
                var v = new FolderBrowseView();
                var vm = new FolderBrowseViewModel(selectedPathCallback, main);
                v.DataContext = vm;
                foreach (var otherView in main.Views)
                {
                    main.Deactivate(otherView);
                }
                main.Add(v, "FolderBrowserKey");
                view = main.GetView("FolderBrowserKey");
            }
            main.Activate(view);
            //IRegion legendRegion = _regionManager.Regions[RegionNames.LegendRegion];
            //var legendview = legendRegion.GetView("LegendListKey");
            //if (legendview != null)
            //{
            //    legendRegion.Deactivate(legendview);
            //}
        }

        public void Initialize()
        {
            
        }
    }
}
