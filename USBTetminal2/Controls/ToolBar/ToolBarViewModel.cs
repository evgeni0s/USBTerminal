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
    /// ColsoleKey, SettingsKey are used to register VMs in IViewModelProvider's dict
    /// </summary>
    public class ToolBarViewModel : ViewModelBase
    {
        private IRegionManager _regionManager;
        private IViewModelProvider _viewModelProvider;
        public ToolBarViewModel(IRegionManager regionManager, IViewModelProvider viewModelProvider)
        {
            _regionManager = regionManager;
            _viewModelProvider = viewModelProvider;
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
            var view = bottom.GetView("SettingsKey");
            if (view == null)
            {
                var vm = _viewModelProvider.GetViewModel<SettingsViewModel>("SettingsKey");//in theory, viewmodel should be created just once
                bottom.Add(new SettingsView() { DataContext = vm }, "SettingsKey");
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
            var view = left.GetView("ColsoleKey");
            if (view == null)
            {
                var vm =_viewModelProvider.GetViewModel<ConsoleViewModel>("ColsoleKey");
                left.Add(new ConsoleView() { DataContext = vm }, "ColsoleKey");
            }
            else
            {
                left.Remove(view);
            }
        }

        #endregion
    }
}
