﻿using Infrastructure;
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
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Practices.Unity;
using ExportModule.Services;

namespace ExportModule
{
    public class ExportModule : IModule, IExportModule
    {
        private IRegionManager _regionManager;
        private ILoggerFacade _logger;
        private IViewModelProvider _viewModelProvider;
        private IUnityContainer _container;
        public ExportModule(IRegionManager regionManager, ILoggerFacade logger, IViewModelProvider viewModelProvider, IUnityContainer container)
        {
            _regionManager = regionManager;
            _viewModelProvider = viewModelProvider;
            _logger = logger;
            _container = container;

            //Create Temporary directiries
            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string specificFolder = Path.Combine(folder, Properties.Settings.Default.TempFolder);

            // Check if folder exists and if not, create it
            if (!Directory.Exists(specificFolder))
            {
                logger.Log("Initializing Export Folder..", Category.Debug, Priority.Medium);
                try
                {
                     Directory.CreateDirectory(specificFolder);
                }
                catch
                {
                    logger.Log("Can't create Export Folder!", Category.Exception, Priority.Medium);
                }
            }


            _container.RegisterType(typeof(IExcelService), typeof(ExcelService));
            //_container.RegisterTypeIfMissing(typeof(IGraphModule), typeof(GraphsManager), true);
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

        public void Export(List<ChartExportArguments> exportData)
        {
            IRegion main = _regionManager.Regions[RegionNames.MainRegion];
            var view = main.GetView("ChooseExportTypeKey");
            if (view == null)
            {
                var v = new ChooseExportTypeView();
                var vm = _viewModelProvider.GetViewModel<ChooseExportTypeViewModel>(exportData);//new ChooseExportTypeViewModel(selectedPathCallback, main);
                v.DataContext = vm;
                foreach (var otherView in main.Views)
                {
                    main.Deactivate(otherView);
                }
                main.Add(v, "ChooseExportTypeKey");
                view = main.GetView("ChooseExportTypeKey");
            }
            main.Activate(view);
        }

        public void Initialize()
        {
            
        }


    }
}
