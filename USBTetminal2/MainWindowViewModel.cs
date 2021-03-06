﻿/*MAIN LOGIC
 * Problems with build?
 * In project I use lots of external Nuget packages. They are stored in /pakeges folder. 
 * Which I did not add to my repo. SO... Visit http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages
 * to know more... And I've never checked if problem with /pakeges is real
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using USBTetminal2.Commands;
using USBTetminal2.Controls.Legend;
using USBTetminal2.Graphs;
using USBTetminal2.Protocol;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using USBTetminal2.Controls;
using USBTetminal2.Controls.Settings;
using TestModule;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using USBTetminal2.Controls.ToolBar;
using Microsoft.Practices.ServiceLocation;
using Infrastructure;
using Microsoft.Practices.Unity;
using USBTetminal2.Communication;
using Modbus.Utility;
using System.Threading;
using USBTetminal2.Decorations;
using MahApps.Metro;

namespace USBTetminal2
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region fields
        private IGraphModule _graphModule;
        private ObservableCollection<Point> _graphData;
        private ILoggerFacade _logger;
        private ICommunicationService _communicationService;
        private IRegionManager _regionManager;
        private IUnityContainer _container;
        private bool _isConsoleVisible;
        private bool _isBottomVisible;
        private object _rightPanel;
        private object _bottomPanel;

        private int _themeIndex = 1;
        private int _accentIndex = 0;

        private List<AccentColorMenuData> _accentColors;
        private List<AppThemeMenuData> _appThemes;

        private string errMSG;
        Shell _mainWindow;
        #endregion

        public MainWindowViewModel(ITestModule module, ILoggerFacade logger, IRegionManager regionManager, IUnityContainer container, ICommunicationService communicationService, IGraphModule graphModule)
        {
            _logger = logger;
            _mainWindow = App.Current.MainWindow as Shell;
            _regionManager = regionManager;
            _container = container;
            _communicationService = communicationService;
            _graphModule = graphModule;
            // create accent color menu items for the demo
            this._accentColors = ThemeManager.Accents
                                            .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                                            .ToList();

            // create metro theme color menu items for the demo
            this._appThemes = ThemeManager.AppThemes
                                           .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as Brush, ColorBrush = a.Resources["WhiteColorBrush"] as Brush })
                                           .ToList();
            //restore user settings
            _themeIndex = Properties.Settings.Default.ThemeIndex;
            _accentIndex = Properties.Settings.Default.AccentIndex;
            //apply user settings at startup
            _accentColors.ElementAt(_accentIndex).ChangeAccentCommand.Execute(null);
            _appThemes.ElementAt(_themeIndex).ChangeAccentCommand.Execute(null);

            initializeCommandBindings();
            initPseudoBroadCast();

            App.Current.Exit += Current_Exit;
        }

        //there is issue with export module, so that is does not close excel files properly
        //I desided to dispose all viewmodels malualy
        void Current_Exit(object sender, ExitEventArgs e)
        {
            App.Current.Exit -= Current_Exit;
           var vmprovider =  _container.Resolve<IViewModelProvider>();
           vmprovider.Dispose();
        }

        #region public properties

        private bool _isModalOpen;

	public bool IsModalOpen
	{
		get { return _isModalOpen;}
		set { _isModalOpen = value;
        OnPropertyChanged("IsModalOpen");
        }
	}


        public string ErrMSG
        {
            get
            {
                if (errMSG == null)
                {
                    errMSG = "Error tracker is ON";

                }
                return errMSG;
            }
            set
            {
                errMSG = value;
                OnPropertyChanged("ErrMSG");
            }
        }
        //Not binded to UI for now and is used as private
        //public CustomSerialPort SelectedPort
        //{
        //    get
        //    {
        //        return _selectedPort;
        //    }
        //    set
        //    {
        //        if (_selectedPort != null && _selectedPort.IsOpen)
        //            _selectedPort.Close();
        //        if (value != null && !value.IsOpen)
        //            value.Open();
        //        //////FIX EXCEPTION!! Доступ к порту закрыт. Возникает если к порту подключена другая программа
        //        // RemoveListener(_selectedPort);
        //        //  AddListener(value);
        //        _selectedPort = value;
        //        OnPropertyChanged("SelectedPort");
        //    }
        //}

        //public LegendListViewModel LegendListDataContext
        //{
        //    get
        //    {
        //        if (_legendListViewModel == null)
        //        {
        //            _legendListViewModel = new LegendListViewModel();
        //            AddListener(_legendListViewModel);
        //        }
        //        return _legendListViewModel;
        //    }
        //    set
        //    {
        //        _legendListViewModel = value;
        //        OnPropertyChanged("LegendListDataContext");
        //    }
        //}

        //not used for now
        //public LegendListViewModel.LegendListItem LastSelectedLegend
        //{
        //    get
        //    {
        //        return _lastSelectedLegend;
        //    }
        //    set
        //    {
        //        _lastSelectedLegend = value;

        //    }
        //}
        //not used for now
        public ObservableCollection<Point> GraphData
        {
            get
            {
                if (_graphData == null)
                    return new ObservableCollection<Point>();
                return _graphData;
            }
            set
            {
                _graphData = value;
                OnPropertyChanged("GraphData");
            }
        }

        public bool IsConsoleVisible
        {
            get
            {
                return _isConsoleVisible;
            }
            set
            {
                _isConsoleVisible = value;
                OnPropertyChanged("IsConsoleVisible");
            }
        }

        public bool IsBottomVisible
        {
            get
            {
                return _isBottomVisible;
            }
            set
            {
                _isBottomVisible = value;
                OnPropertyChanged("IsBottomVisible");
            }
        }

        public double ConsoleWidth
        {
            get { return 150d; }
        }

        public double BottomHeight
        {
            get { return 150d; }
        }

        //Need to export this to XAML container
        public object RightPanel
        {
            get
            {
                if (_rightPanel == null)
                    _rightPanel = new object();
                return _rightPanel;
            }
            set
            {
                _rightPanel = value;
                OnPropertyChanged("RightPanel");
            }
        }

        public object BottomPanel
        {
            get
            {
                if (_bottomPanel == null)
                    _bottomPanel = new object();
                return _bottomPanel;
            }
            set
            {
                _bottomPanel = value;
                OnPropertyChanged("BottomPanel");
            }
        }

        private SettingsViewModel _settingsViewModel;
        public SettingsViewModel SettingsViewModel
        {
            get { return _settingsViewModel; }
            set
            {
                _settingsViewModel = value;
                OnPropertyChanged("SettingsViewModel");
            }
        }

        #endregion

        #region private properties
        private ToolBarViewModel _toolBar;
        public ToolBarViewModel ToolBar
        {
            get
            {
                return _toolBar ?? (_toolBar = _container.Resolve<ToolBarViewModel>());
            }
        }

        //public CustomRichTextBox Console
        //{
        //    get { return _logger as CustomRichTextBox; }
        //}



        #endregion

        private void initPseudoBroadCast()
        {
            AddListener(new FrameManager());
            //AddListener(new GraphsManager(_plotter));
            // +SELECTED PORT WILL ADD ITSELF
            // +LegendLisDataContext WILL ADD ITSELF
        }


        private void initializeCommandBindings()
        {
            createCommandBinding(CustomCommands.Reset, onResetExecute, onResetCanExecute);
            createCommandBinding(CustomCommands.ShowPoints, onShowPoints, onShowPointsCanExecute);
            createCommandBinding(CustomCommands.ShowLegend, onShowLegend, onShowLegendCanExecute);
            createCommandBinding(CustomCommands.RemoveLegend, onRemoveLegend, onRemoveLegendCanExecute);
            createCommandBinding(CustomCommands.ErrorReport, onErrorReport, onErrorReportCanExecute);
            //createCommandBinding(CustomCommands.DataRecived, onDataRecived, onDataRecivedCanExecute);
            createCommandBinding(CustomCommands.PlotGraph, onPlotGraph, onPlotGraphCanExecute);
            createCommandBinding(CustomCommands.AddNewLegend, onAddNewLegend, onAddNewLegendCanExecute);
            createCommandBinding(CustomCommands.RemoveGraph, onRemoveGraphLegend, onRemoveGraphCanExecute);
            createCommandBinding(CustomCommands.ChangeMarkersVisibility, onChangeMarkersVisibility, onChangeMarkersVisibilityCanExecute);
            createCommandBinding(CustomCommands.LegendLegendVisibility, onLegendLegendVisibility, onLegendLegendVisibilityCanExecute);
            createCommandBinding(CustomCommands.LoadDataToGrid, onLoadDataToGrid, onLoadDataToGridCanExecute);
            //createCommandBinding(CustomCommands.ConsoleVisibility, onConsoleVisibility, onConsoleVisibilityCanExecute);
        }

        #region Command Implementation

        //#region ConsoleVisibilityCommand
        //private void onConsoleVisibility(object sender, ExecutedRoutedEventArgs e)
        //{
        //    IsConsoleVisible = !IsConsoleVisible;
        //    if (RightPanel.GetType() != typeof(ConsoleView))
        //    RightPanel = Console;
        //}
        //private void onConsoleVisibilityCanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = true;
        //}

        //#endregion

        #region LegendSelectedCommand
        private void onLoadDataToGrid(object sender, ExecutedRoutedEventArgs e)
        {
            CompositeDataSource dataSource = (CompositeDataSource)e.Parameter;
            GraphData = new ObservableCollection<Point>(dataSource.GetPoints());
            OnPropertyChanged("GraphData");

            //EnumerableDataSource<double> test =  _rawData.DataParts.ElementAt(1) as EnumerableDataSource<double>;
            //var someValues = _rawData.GetPoints();
            //var someValues1 = _rawData.DataParts.ElementAt(0).GetPoints();
            //var someValues2 = _rawData.DataParts.ElementAt(1).GetPoints();
            //Got 2 arrays here need to bind to Grid
        }

        private void onLoadDataToGridCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null; // &e.Parameter.GetType() == typeof(IEnumerable<IPointDataSource>); unfortunatly GetType() says "runtime type" 
        }
        #endregion

        #region ErrorReportCommand
        //Provides Loging functional
        private void onErrorReportCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null;
        }

        private void onErrorReport(object sender, ExecutedRoutedEventArgs e)
        {
            //ErrMSG += Environment.NewLine + e.Parameter.ToString();
            //   MessageBox.Show("Command: ShowLegend");

            //Console.Instanse.Log(e.Parameter.ToString());
        }


        #region ConnectCommand


        #endregion


        #endregion

        #region ShowLegendCommand
        private void onShowLegendCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void onShowLegend(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Command: ShowLegend");
        }
        #endregion

        #region ShowPointsCommand
        private void onShowPoints(object sender, ExecutedRoutedEventArgs e)
        {
            CommonBroadcastType type = CommonBroadcastType.CHANGE_MARKERS_VISIBILITY;
            NotifyAllBroadcastListeners(type, e.Parameter);
        }

        private void onShowPointsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region ResetCommand
        public void onResetExecute(object sender, ExecutedRoutedEventArgs e)
        {
            CommonBroadcastType type = CommonBroadcastType.CLEAR_ALL;
            NotifyAllBroadcastListeners(type, e.Parameter);
        }

        private void onResetCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region RemoveLegendCommand
        private void onRemoveLegend(object sender, ExecutedRoutedEventArgs e)
        {
            //CommonBroadcastType type = CommonBroadcastType.DELETE_LEGEND;
            //NotifyAllBroadcastListeners(type, e.Parameter);
        }

        private void onRemoveLegendCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region LegendLegendVisibility
        private void onLegendLegendVisibility(object sender, ExecutedRoutedEventArgs e)
        {
            CommonBroadcastType type = CommonBroadcastType.USER_CHANGED_LEGEND_CONTAINER_VISIBILITY;
            NotifyAllBroadcastListeners(type, e.Parameter);
        }

        private void onLegendLegendVisibilityCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region ChangeMarkersVisibilityCommand
        private void onChangeMarkersVisibility(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Command: ChangeMarkersVisibility not implemented!");
        }

        private void onChangeMarkersVisibilityCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region RemoveGraphCommand
        private void onRemoveGraphLegend(object sender, ExecutedRoutedEventArgs e)
        {
            CommonBroadcastType type = CommonBroadcastType.DELETE_GRAPH;
            NotifyAllBroadcastListeners(type, e.Parameter);
        }

        private void onRemoveGraphCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null;
        }
        #endregion

        #region AddNewLegendCommand


        private void onAddNewLegend(object sender, ExecutedRoutedEventArgs e)
        {
            CommonBroadcastType type = CommonBroadcastType.ADD_LEGEND_TO_GRAPH;
            NotifyAllBroadcastListeners(type, e.Parameter);
        }

        private void onAddNewLegendCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null && e.Parameter.GetType() == typeof(LineAndMarker<MarkerPointsGraph>);
        }
        #endregion

        #region PlotGraphCommand
        private void onPlotGraph(object sender, ExecutedRoutedEventArgs e)
        {

            CommonBroadcastType type = CommonBroadcastType.BUILD_GRAPH_FROM_Y_POINTS;
            NotifyAllBroadcastListeners(type, e.Parameter);
        }

        private void onPlotGraphCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null && e.Parameter.GetType() == typeof(List<int>);
        }
        #endregion

        #region DataRecivedCommand


        //private void onDataRecived(object sender, ExecutedRoutedEventArgs e)
        //{
        //    CommonBroadcastType type = CommonBroadcastType.DECODE_BYTE_ARRAY_FROM_DEVICE;
        //   // NotifyAllBroadcastListeners(type, SelectedPort.RecivedData);
        //    //Console.Instanse.Log(e.Parameter.ToString());
        //}


        //private void onDataRecivedCanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //  //  e.CanExecute = e.Parameter != null && SelectedPort.RecivedData.Length > 2;//2 - second byte and points to frame type
        //    e.CanExecute = true;
        //}

        #endregion

        #region MeasureCommand


        private ICommand _measureCommand;
        public ICommand MeasureCommand
        {
            get { return _measureCommand ?? (_measureCommand = new RelayCommand(OnMeasureCommand)); }
        }

        private void OnMeasureCommand(object obj)
        {

            _graphModule.buildGraphFromYPoints(GraphsManager.TestData);

            //NotifyAllBroadcastListeners(CommonBroadcastType.BUILD_GRAPH_FROM_Y_POINTS, GraphsManager.TestData);
            ////works perfect

            foreach (var port in _communicationService.Ports)
            {
                if (!port.IsOpen) continue;

                // ModbusUtility.GetUInt32(registers[1], registers[0]);
                //new Thread(() =>
                //{

                    try
                    {
                       var device = _communicationService.GetDevice(port.PortName);
                        //Test Frame. works perfect. Returns all data    FUNCTION 6
                        //device.WriteSingleRegister(255, 250, 170);

                        //FUNCTION 3
                        //var response = device.ReadHoldingRegisters(255, 252, 1);
                       //var response = device.ReadHoldingRegisters(0x03, 0xFC, 0x01);//Do not delete. Way to decode hex
                        var response = device.ReadHoldingRegisters(0x03, 0x00, 0x78);//Final!!!!!!!!!
                        string msg = "";
                       
                     
                        for (int i = 0; i < response.Length; i++)
                        {
                            //_logger.Log(Convert.ToString(response[i], 16), Category.Info, Priority.High);
                            //_logger.Log(response[i].ToString(), Category.Info, Priority.High);
                            //msg+= " " + response[i].ToString("X");

                            msg += " " + response[i].ToString();
                        }
                        _logger.Log(msg, Category.Info, Priority.High);
                        NotifyAllBroadcastListeners(CommonBroadcastType.BUILD_GRAPH_FROM_Y_POINTS, response.ToList());

                    }
                    catch (TimeoutException)
                    {
                        _logger.Log("OnMeasureCommand: no response have been recived", Category.Exception, Priority.Low);
                        port.dataReceived(null, null);
                    }
                //}).Start();
            }

        }
        #endregion


        #region AccentCommand
        private ICommand _accentCommand;
        public ICommand AccentCommand
        {
            get { return _accentCommand ?? (_accentCommand = new RelayCommand(OnAccentCommand)); }
        }
        
        private void OnAccentCommand(object obj)
        {
            if (_accentIndex >= _accentColors.Count)
            {
                _accentIndex = 0;
            }
            Properties.Settings.Default.AccentIndex = _accentIndex;
            Properties.Settings.Default.Save();
            var nextAccend = _accentColors.ElementAt(_accentIndex++);
            nextAccend.ChangeAccentCommand.Execute(null);
        }
        #endregion

        #region ThemeCommand
        private ICommand _themeCommand;
        public ICommand ThemeCommand
        {
            get { return _themeCommand ?? (_themeCommand = new RelayCommand(OnThemeCommand)); }
        }
        private void OnThemeCommand(object obj)
        {
            if (_themeIndex >= _appThemes.Count)
            {
                _themeIndex = 0;
            }
            Properties.Settings.Default.ThemeIndex = _themeIndex;
            Properties.Settings.Default.Save();
            var nextAccend = _appThemes.ElementAt(_themeIndex++);
            nextAccend.ChangeAccentCommand.Execute(null);
        }
        #endregion
		 

        protected void createCommandBinding(ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            var binding = new CommandBinding(command, executed, canExecute);
            _mainWindow.CommandBindings.Add(binding);
        }
        #endregion

        #region simple broadcast
        private List<ISimpleBroadcastListener> broadCastSubscribers = new List<ISimpleBroadcastListener>();
        private LegendListViewModel.LegendListItem _lastSelectedLegend;

        private void AddListener(ISimpleBroadcastListener listener)
        {
            broadCastSubscribers.Add(listener);
        }
        private void RemoveListener(ISimpleBroadcastListener listener)
        {
            if (broadCastSubscribers.Contains(listener))
                broadCastSubscribers.Remove(listener);
        }
        private void NotifyAllBroadcastListeners(CommonBroadcastType msgType, object data)
        {
            foreach (ISimpleBroadcastListener listener in broadCastSubscribers)
            {
                listener.ReciveMessage(msgType, data);
            }
        }

        #endregion

        #region for debug only
        /// <summary>
        /// Dislpay ports in debug window
        /// </summary>
        /// <returns></returns>
        private string[] OrderedPortNames()
        {
            // Just a placeholder for a successful parsing of a string to an integer
            int num;
            // Order the serial port names in numberic order (if possible)
            return SerialPort.GetPortNames().OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out num) ? num : 0).ToArray();
        }


        public void showAllPorts()
        {

            string[] ports = OrderedPortNames();
            CustomCommands.ErrorReport.Execute(ports.Length == 0 ? "No ports detected!" : "Available ports: ", null);
            foreach (string port in ports)
            {
                CustomCommands.ErrorReport.Execute(port, null);
            }

        }
        #endregion
    }
}
