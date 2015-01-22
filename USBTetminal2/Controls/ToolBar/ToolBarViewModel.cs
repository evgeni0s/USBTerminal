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

namespace USBTetminal2.Controls.ToolBar
{

    /// <summary>
    /// NOT USED FOR NOW
    /// </summary>
    public class ToolBarViewModel : ViewModelBase
    {

        //protected Shell _mainWindow = null;
        private IRegionManager _regionManager;
        private SettingsViewModel _settings;

        public ToolBarViewModel(IRegionManager regionManager)
        {
            //_mainWindow = App.Current.MainWindow as Shell;
            _regionManager = regionManager;


            _settings = ServiceLocator.Current.GetInstance<SettingsViewModel>();
        }




        #region Commands


        private ICommand _showSettingsCommand;
        public ICommand ShowSettingsCommand
        {
            get { return _showSettingsCommand ?? (_showSettingsCommand = new RelayCommand(ShowSettings)); }
        }

        private void ShowSettings(object obj)
        {
            var settings = _regionManager.Regions["BottomPanelRegion"].GetView("Settings") as SettingsView;

            if (settings == null)
            {
                _regionManager.Regions["BottomPanelRegion"].Add(new SettingsView() { DataContext = _settings }, "Settings");
            }
            else
            {
                    settings.Visibility = settings.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        //private ICommand _commandBufferExportTo;
        //private ICommand _commandBufferClear;
        //private ICommand _commandBufferAutoScrol;

        //public ICommand CommandBufferExportTo
        //{
        //    get
        //    {
        //        if (_commandBufferExportTo == null)
        //        {
        //            _commandBufferExportTo = new AlaskaCommand(p => OnExecuteBufferExportTo(), c => CanExecuteBufferExportTo());
        //        }

        //        return _commandBufferExportTo;
        //    }
        //}
        //public ICommand CommandBufferClear
        //{
        //    get
        //    {
        //        if (_commandBufferClear == null)
        //        {
        //            _commandBufferClear = new AlaskaCommand(p => OnExecuteBufferClear(), c => CanExecuteEBufferClear());
        //        }

        //        return _commandBufferClear;
        //    }
        //}
        //public ICommand CommandBufferAutoScrol
        //{
        //    get
        //    {
        //        if (_commandBufferAutoScrol == null)
        //        {
        //            _commandBufferAutoScrol = new AlaskaCommand(p => OnExecuteBufferAutoScrol(), c => CanExecuteBufferAutoScrol());
        //        }

        //        return _commandBufferAutoScrol;
        //    }
        //}

        #endregion
    }
}
