using FolderBrowser.Command;
using Infrastructure;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
//using USBTetminal2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

//Uses common ViewModel, but different RelayCommand
namespace ExportModule.Views
{
    public class ChooseExportTypeViewModel_OLD : ViewModelBase
    {

        private ILoggerFacade _logger;
        private IRegionManager _regionManager;
        //private IGraphModule _grahpModule;
        private IViewModelProvider _viewModelProvider;
        public ChooseExportTypeViewModel_OLD(ILoggerFacade logger, IRegionManager regionManager, IViewModelProvider viewModelProvider)
        {
            _logger = logger;
            _regionManager = regionManager;
            //_grahpModule = grahpModule;
            _viewModelProvider = viewModelProvider;
        }

        //public EExportType ExportType
        //{
        //    get
        //    {
        //        return (Model is EExportType) ? (EExportType)Model : EExportType.Text;
        //    }
        //}

        private ICommand _excelCommand;
        public ICommand ExcelCommand
        {
            get { return _excelCommand ?? (_excelCommand = new FolderBrowser.Command.RelayCommand(OnExcel)); }
        }

        private void OnExcel()
        {

            //Note: to set Model properly it has to be initialized after class is created
            IRegion main = _regionManager.Regions[RegionNames.MainRegion];
            var view = main.GetView("ExcelPreviewKey");
            if (view == null)
            {
                var v = new ExcelPreview();
                var vm = _viewModelProvider.GetViewModel<ExcelPreviewViewModel_OLD>(Model);//new ChooseExportTypeViewModel(selectedPathCallback, main);
                vm.CreatePreview();
                v.DataContext = vm;
                foreach (var otherView in main.Views)
                {
                    main.Deactivate(otherView);
                }
                main.Add(v, "ExcelPreviewKey");
                view = main.GetView("ExcelPreviewKey");
            }
            main.Activate(view);

            //IRegion main = _regionManager.Regions[RegionNames.MainRegion];
            //var view = main.GetView("PlotterKey");
            //if (view != null)
            //{
            //    main.Deactivate(view);
            //}
        }


        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new FolderBrowser.Command.RelayCommand(OnClose)); }
        }

        private void OnClose()
        {
            IRegion main = _regionManager.Regions[RegionNames.MainRegion];
            var view = main.GetView("ChooseExportTypeKey");
            if (view != null)
            {
                main.Deactivate(view);
            }
        }


    }
         
}
