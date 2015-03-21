using ExportModule.Services;
//using Excel;
using FolderBrowser.Command;
using Infrastructure;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfDocumentPreviewer;

namespace ExportModule.Views
{
    public class ExcelPreviewViewModel : ViewModelBase
    {

        private ILoggerFacade _logger;
        private IRegionManager _regionManager;
        //private IGraphModule _grahpModule;
        private IExcelService _excelService;
        private string _tempExcelFileName;
        public ExcelPreviewViewModel(ILoggerFacade logger, IRegionManager regionManager, IExcelService excelService)
        {
            _logger = logger;
            _regionManager = regionManager;
            //_grahpModule = grahpModule;
            _excelService = excelService;
      

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            //_tempExcelFileName = Path.Combine(folder, Properties.Settings.Default.TempFolder, string.Format(@"{0}.xlsx", Guid.NewGuid()));
            //_tempExcelFileName = Path.Combine(folder, Properties.Settings.Default.TempFolder, string.Format(@"{0}.xlsx", DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss")));
            //_tempExcelFileName = Path.Combine(folder, Properties.Settings.Default.TempFolder, string.Format(@"{0}.xlsx", 1));
            _tempExcelFileName = Path.Combine(folder, Properties.Settings.Default.TempFolder, string.Format(@"{0}.xls", 1));
             //_tempExcelFileName = Path.Combine(specificFolder string.Format(@"{0}.xlsx", Guid.NewGuid());
            // Check if folder exists and if not, create it
            //if (!Directory.Exists(specificFolder))
            //{
            //    logger.Log("Initializing Export Folder..", Category.Debug, Priority.Medium);
            //    try
            //    {
            //        Directory.CreateDirectory(specificFolder);
            //    }
            //    catch
            //    {
            //        logger.Log("Can't create Export Folder!", Category.Exception, Priority.Medium);
            //    }
            //}
         
        }

        private PreviewerControl _previewerControl;
        public PreviewerControl PreviewerControl
        {
            get { return _previewerControl ?? (_previewerControl = new PreviewerControl()); }
        }
        

        //This method has to be called from outside to initialize this class
        public void CreatePreview()
        {
            var selectedGraphs = Model as List<LineGraph>;

            bool result = _excelService.CreateExcelFile(_tempExcelFileName);
            if (!result)
            {
                _logger.Log("Error generating excel preview", Category.Exception, Priority.Medium);
            }
            PreviewerControl.FileName = _tempExcelFileName;

        }



        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(OnClose)); }
        }

        private void OnClose()
        {
            IRegion main = _regionManager.Regions[RegionNames.MainRegion];
            var view = main.GetView("ExcelPreviewKey");
            if (view != null)
            {
                main.Deactivate(view);
            }
        }
    }
}
