/*MAIN LOGIC
 ALL COMMANDS ARE EXECUTED HERE
 ALL THE REST CLASSES ARE JUST TOOLS*/

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

namespace USBTetminal2
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region fields
       // private ObservableCollection<CLegendItemViewModel> legendsList;
        private CustomSerialPort _selectedPort;
        private ChartPlotter _plotter;
        private LegendListViewModel _legendListViewModel;
        private string errMSG;
        MainWindow _mainWindow;
        #endregion

        public MainWindowViewModel()
        {
            _mainWindow = App.Current.MainWindow as MainWindow;
            _plotter = _mainWindow.mPlotter;

            initializeCommandBindings();
            initPseudoBroadCast();

        }





        #region public properties
        //public ObservableCollection<CLegendItemViewModel> LegendsList
        //{
        //    get
        //    {
        //        if (legendsList == null)
        //            legendsList = new ObservableCollection<CLegendItemViewModel>();
        //        return legendsList;
        //    }
        //    set { legendsList = value; }
        //}

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
            set { errMSG = value;
            OnPropertyChanged("ErrMSG");
            }
        }
        //Not binded to UI for now and is used as private
        public CustomSerialPort SelectedPort
        {
            get
            {
                return _selectedPort;
            }
            set
            {
                if (_selectedPort != null && _selectedPort.IsOpen)
                    _selectedPort.Close();
                if (value != null && !value.IsOpen)
                    value.Open();             //////FIX EXCEPTION!! Доступ к порту закрыт. Возникает если к порту подключена другая программа
                RemoveListener(_selectedPort);
                AddListener(value);
                _selectedPort = value;
                OnPropertyChanged("SelectedPort");
            }
        }

        public LegendListViewModel LegendListDataContext
        {
            get
            {
                if (_legendListViewModel == null)
                {
                    _legendListViewModel = new LegendListViewModel();
                     AddListener(_legendListViewModel);
                }
                return _legendListViewModel;
            }
            set
            {
                _legendListViewModel = value;
            }
        }
        #endregion


        private void initPseudoBroadCast()
        {
            AddListener(new FrameManager());
            AddListener(new GraphsManager(_plotter));
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
            createCommandBinding(CustomCommands.Connect, onConnect, onConnectCanExecute);
            createCommandBinding(CustomCommands.DataRecived, onDataRecived, onDataRecivedCanExecute);
            createCommandBinding(CustomCommands.PlotGraph, onPlotGraph, onPlotGraphCanExecute);
            createCommandBinding(CustomCommands.AddNewLegend, onAddNewLegend, onAddNewLegendCanExecute);
            createCommandBinding(CustomCommands.RemoveGraph, onRemoveGraphLegend, onRemoveGraphCanExecute);
            createCommandBinding(CustomCommands.ChangeMarkersVisibility, onChangeMarkersVisibility, onChangeMarkersVisibilityCanExecute);
            createCommandBinding(CustomCommands.LegendContainerVisibility, onLegendContainerVisibility, onLegendContainerVisibilityCanExecute);
        }

        #region LegendContainerVisibility
        private void onLegendContainerVisibility(object sender, ExecutedRoutedEventArgs e)
        {
            CommonBroadcastType type = CommonBroadcastType.USER_CHANGED_LEGEND_CONTAINER_VISIBILITY;
            NotifyAllBroadcastListeners(type, e.Parameter);
        }

        private void onLegendContainerVisibilityCanExecute(object sender, CanExecuteRoutedEventArgs e)
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

            //List<int> points = (List<int>)e.Parameter;
            //if (points != null)
            //{
 
            //}                                                                                                                                           
        }

        private void onPlotGraphCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null && e.Parameter.GetType() == typeof(List<int>);
        }
        #endregion



        #region DataRecivedCommand

        private void onDataRecived(object sender, ExecutedRoutedEventArgs e)
        {
            CommonBroadcastType type = CommonBroadcastType.DECODE_BYTE_ARRAY_FROM_DEVICE;

            NotifyAllBroadcastListeners(type, SelectedPort.RecivedData);
            //byte[] data = SelectedPort.RecivedData;
           
            //AbstractFrame frame = (AbstractFrame)getSelectedFrame(data[1]);
            //bool frameIsCorrect = frame.tryPrase(data);
            //if (frameIsCorrect)
            //{
            //    //Frame knows about what action is
            //    frame.executeActionForThisFrame();
            //}
        }


        //private AbstractFrame getSelectedFrame(byte commandType)
        //{
        //    switch (commandType)
        //    {
        //        case 6:
        //            return new MeasurmentFrame();
        //        default: 
        //            return new TestFrame();
        //    }

        //}

        //LineGraph mGraph;
        //private void plotGraph(CompositeDataSource dataSource)
        //{
        //    mGraph = new LineGraph();
        //    mGraph.DataSource = dataSource;
        //    //Binding binding = new Binding();
        //    //binding.Path = new PropertyPath("TimeData");
        //    //binding.Source = tableData;
        //    //binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        //    //BindingOperations.SetBinding(mGraph, LineGraph.DataSourceProperty, binding);
        //    //Add checkBox
        //    LineLegendItem legendItem = (LineLegendItem)mGraph.Description.LegendItem;
        //    DataTemplate legendItemNewTemplate = FindResource("legendTemplate") as DataTemplate;
        //    legendItem.ContentTemplate = legendItemNewTemplate;

        //    mPlotter.Children.Add(mGraph);
        //}

        private void onDataRecivedCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null && SelectedPort.RecivedData.Length > 2;//2 - second byte and points to frame type
        }

        #endregion


        #region commands
        //ICommand m_removeLegendCommand;
        //public ICommand RemoveLegendCommand
        //{
        //    get
        //    {
        //        if (m_removeLegendCommand == null)
        //        {
        //            m_removeLegendCommand = new RelayCommand(
        //                (param) => RemoveLegend(param));
        //        }
        //        return m_removeLegendCommand;
        //    }
        //}

        //private void RemoveLegend(object param)
        //{

        //}
        #endregion

        #region Command Implementation

        #region ConnectCommand

        private void onConnect(object sender, ExecutedRoutedEventArgs e)
        {
           // IdentificatorFrame connectionFrame = new IdentificatorFrame();
           // byte[] frameData = connectionFrame.Request();

          //  _connector.connect(e.Parameter.ToString());

          //  connectionFrame.

            AbstractFrame tempFrame = new MeasurmentFrame();
            byte[] request = tempFrame.Request();
            string PortName = e.Parameter.ToString();
            SelectedPort = new CustomSerialPort(PortName);
            SelectedPort.SendData(request);
           
        }

        private void onConnectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null;
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
            ErrMSG += Environment.NewLine + e.Parameter.ToString();
            //   MessageBox.Show("Command: ShowLegend");
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
            CommonBroadcastType type = CommonBroadcastType.DELETE_LEGEND;
            NotifyAllBroadcastListeners(type, e.Parameter);
        }

        private void onRemoveLegendCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        protected void createCommandBinding(ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            var binding = new CommandBinding(command, executed, canExecute);
            _mainWindow.CommandBindings.Add(binding);
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


        private List<ISimpleBroadcastListener> broadCastSubscribers  = new List<ISimpleBroadcastListener>();
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
            foreach(ISimpleBroadcastListener listener in broadCastSubscribers)
            {
                listener.ReciveMessage(msgType, data);
            }
        }

      

        #endregion
    }
}
