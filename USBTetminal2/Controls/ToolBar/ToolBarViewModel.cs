using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using USBTetminal2.Controls.Settings;
using Infrastructure;
using Microsoft.Practices.Unity;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Logging;

namespace USBTetminal2.Controls.ToolBar
{

    /// <summary>
    /// NOT USED FOR NOW
    /// </summary>
    public class ToolBarViewModel : ViewModelBase
    {
        private IRegionManager _regionManager;
        private IViewModelProvider _viewModelProvider;
        private ILoggerFacade _logger;
        public ToolBarViewModel(IRegionManager regionManager, IViewModelProvider viewModelProvider, ILoggerFacade logger)
        {
            _regionManager = regionManager;
            _viewModelProvider = viewModelProvider;
            _logger = logger;
        }

        #region Commands


        private ICommand _showSettingsCommand;
        public ICommand ShowSettingsCommand
        {
            get { return _showSettingsCommand ?? (_showSettingsCommand = new RelayCommand(ShowSettings)); }
        }

        //Potential Memory leak place. I do not know if my views and VMs are ever disposed
        private void ShowSettings(object obj)
        {
            IRegion bottom = _regionManager.Regions[RegionNames.BottomPanelRegion];
            var view = bottom.GetView("Settings");
            if (view == null)
            {
                var vm = _viewModelProvider.GetViewModel<SettingsViewModel>("Settings");//in theory, viewmodel should be created just once
                bottom.Add(new SettingsView() { DataContext = vm }, "Settings");
            }
            else 
            {
                bottom.Remove(view);
            }
        }


        private ICommand _showConsoleCommand;
        public ICommand ShowConsoleCommand
        {
            get { return _showConsoleCommand ?? (_showConsoleCommand = new RelayCommand(ShowConsole)); }
        }

        private void ShowConsole(object obj)
        {
            IRegion left = _regionManager.Regions[RegionNames.LeftPanelRegion];
            var view = left.GetView("Console");
            if (view == null)
            {
                left.Add(_logger, "Console");
            }
            else
            {
                left.Remove(view);
            }
        }

        #endregion
    }
}
