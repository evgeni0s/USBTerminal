﻿using ExportModule;
using Infrastructure;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using USBTetminal2.Controls.Legend;
using USBTetminal2.Graphs;

namespace USBTetminal2.Controls.Chart
{
    public class ChartViewModel: ViewModelBase
    {
        private IExportModule _exportModule;
        private ILoggerFacade _logger;
        private IRegionManager _regionManager;
        //private IGraphModule _grahpModule;
        public ChartViewModel(ILoggerFacade logger, IExportModule exportModule, IRegionManager regionManager)
        {
           
            //Model = logger;
            _exportModule = exportModule;
            _logger = logger;
            _regionManager = regionManager;
            //_grahpModule = grahpModule;
        }

        public LegendListViewModel Legend
        {
            get { return Model as LegendListViewModel; }
        }

        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get { return _clearCommand ?? (_clearCommand = new RelayCommand(OnClear)); }
        }

        private void OnClear(object obj)
        {
          IGraphModule gm =  ServiceLocator.Current.GetInstance<IGraphModule>(); //Need get Grapgs module to impliment commands
          gm.clearAll();
          Legend.clearAll();
        }

        private ICommand _changeMarkersVisibilityCommand;
        public ICommand ChangeMarkersVisibilityCommand
        {
            get { return _changeMarkersVisibilityCommand ?? (_changeMarkersVisibilityCommand = new RelayCommand(OnChangeMarkersVisibility)); }
        }

        private void OnChangeMarkersVisibility(object obj)
        {
            IGraphModule gm = ServiceLocator.Current.GetInstance<IGraphModule>(); //Need get Grapgs module to impliment commands
            gm.changeMarkersVisibility();
        }

        //private ICommand _legendVisibilityCommand;
        public ICommand LegendVisibilityCommand
        {
            get { return Legend.SwitchVisibilityCommand; }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(OnSaveCommand)); }
        }

        private void OnSaveCommand(object obj)
        {
            _exportModule.ShowFolderBrowserView(OnFileSelected);
        }

        public void  OnFileSelected(string path)
        {

        }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(OnClose)); }
        }

        private void OnClose(object obj)
        {
            IRegion main = _regionManager.Regions[RegionNames.MainRegion];
            var view = main.GetView("PlotterKey");
            if (view != null)
            {
                main.Deactivate(view);
            }
        }

        
    }
}
