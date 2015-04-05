using ExportModule.Services;
using Infrastructure;
using MahApps.Metro.Controls;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfDocumentPreviewer;
using MahApps.Metro.Controls.Dialogs;

namespace ExportModule.Views
{
    public class SelectExportTemplateViewModel : ViewModelBase
    {
        private ILoggerFacade _logger;
        private IRegionManager _regionManager;
        private IExcelService _excelService;
        private IExportModule _exportModule;
        private string _excelPreviewFileName;
        private string _textPreviewFileName;
        public SelectExportTemplateViewModel(ILoggerFacade logger, IRegionManager regionManager, IExcelService excelService, IExportModule exportModule)
        {
            _logger = logger;
            _regionManager = regionManager;
            _excelService = excelService;
            _exportModule = exportModule;
            string root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _excelPreviewFileName = Path.Combine(root, AppDirectories.AppDataFolder, AppDirectories.TempFolder, "preview.xls");
            _textPreviewFileName = Path.Combine(root, AppDirectories.AppDataFolder, AppDirectories.TempFolder, "preview.txt");
            FilePath = Properties.Settings.Default.SaveFolder ?? "Not Set" ;
            

        }

        private PreviewerControl _excelPreviewControl;
        public PreviewerControl ExcelPreviewControl
        {
            get { return _excelPreviewControl ?? (_excelPreviewControl = new PreviewerControl()); }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value;
            OnPropertyChanged("FilePath");
            }
        }

        public ChartExportArguments CurrentChart { get; set; }

        ///This method cant start from constructor beacuse Model is not ready jet
        /// call this method after instance is created
        public void Initialize()
        {

            ////bring into view
            //ExcelPreviewControl.FileName = _excelPreviewFileName;
            ExcelPreviewControl.Visibility = Visibility.Collapsed;
            Task fileTask = new Task(() => 
            {
                //Creates blank file
                _excelService.CreateExcelFile(_excelPreviewFileName);

            });
            Task generatePreviewTask = fileTask.ContinueWith(none =>
            {
                //Creates blank file
                var selectedGraphs = Model as List<ChartExportArguments>;
                _excelService.ExportToExcelFile(_excelPreviewFileName, selectedGraphs);
            });
            Task UITask = generatePreviewTask.ContinueWith(none =>
            {
                ////bring into view
                ExcelPreviewControl.FileName = _excelPreviewFileName;
                ExcelPreviewControl.Visibility = Visibility.Visible;

            }, TaskScheduler.FromCurrentSynchronizationContext());

            fileTask.Start();
        }

    
        private ICommand _doneCommand;
        public ICommand DoneCommand
        {
            get { return _doneCommand ?? (_doneCommand = new RelayCommand(OnDone, OnDoneCanExecute )); }
        }
        //TO DO: remove this aas it takes too much resources
        private bool OnDoneCanExecute(object obj)
        {
            return Directory.Exists(Properties.Settings.Default.SaveFolder);
        }

        private void OnDone(object obj)
        {
            IRegion main = _regionManager.Regions[RegionNames.MainRegion];
            var view = main.GetView("ChooseExportTypeKey");
            if (view != null)
            {
                main.Deactivate(view);
            }

            try
            {
                File.Copy(_excelPreviewFileName, Path.Combine(Properties.Settings.Default.SaveFolder, DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xls"));

                Process.Start("explorer.exe", Properties.Settings.Default.SaveFolder);
                _logger.Log("Console's content is saved to:" + Environment.NewLine + obj, Category.Info, Priority.Medium);
            }
            catch (Exception e)
            {
                _logger.Log("Error saveing or opening file!" + Environment.NewLine + " Exception message: " + Environment.NewLine + e.Message, Category.Exception, Priority.Medium);
            }
        }

        //public ObservableCollection<string> ModelObservable
        //{ 
        //}


        //User specifies save folder
        private ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get { return _browseCommand ?? (_browseCommand = new RelayCommand(OnBrowse)); }
        }

        private void OnBrowse(object obj)
        {
            _exportModule.ShowFolderBrowserView(OnUserSelelectedFolder);
        }

        private void OnUserSelelectedFolder(string newFolder)
        {

            IRegion main = _regionManager.Regions[RegionNames.MainRegion];
            var view = main.GetView("ChooseExportTypeKey");
            if (view != null)
            {
                main.Activate(view);
            }
            Properties.Settings.Default.SaveFolder = newFolder;
            Properties.Settings.Default.Save();
            FilePath = newFolder;
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new RelayCommand(OnRefresh)); }
        }

        private void OnRefresh(object obj)
        {
            Initialize();
        }

        //Same as refresh for now
        private ICommand _applyToCurrentCommand;
        public ICommand ApplyToCurrentCommand
        {
            get { return _applyToCurrentCommand ?? (_applyToCurrentCommand = new RelayCommand(OnApplyToCurrent)); }
        }

        private void OnApplyToCurrent(object obj)
        {
            Initialize();
        }

        //Same as refresh for now
        private ICommand _applyToAllCommand;
        public ICommand ApplyToAllCommand
        {
            get { return _applyToAllCommand ?? (_applyToAllCommand = new RelayCommand(OnApplyToAll)); }
        }

        private void OnApplyToAll(object obj)
        {

            var chartExportArguments = Model as List<ChartExportArguments>;
            foreach (var chart in chartExportArguments)
            {
                chart.CopyOptionsFromOtherChart(CurrentChart);
            }


            Initialize();
        }


        private ICommand _renameChartCommand;
        public ICommand RenameChartCommand
        {
            get { return _renameChartCommand ?? (_renameChartCommand = new RelayCommand(OnRenameChart)); }
        }

        private async void OnRenameChart(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            ExcelPreviewControl.Visibility = Visibility.Hidden;

            var result = await metroWindow.ShowInputAsync("Rename", "Type new name for " + CurrentChart.ChartTitle);

            if (result == null) //user pressed cancel
                return;

            CurrentChart.ChartTitle = result;
            ExcelPreviewControl.Visibility = Visibility.Visible;
        }


        protected override void Disposing()
        {
            ExcelPreviewControl.ForceDispose();
        }
       
    }
}
