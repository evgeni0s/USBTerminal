/*ARM Device project is a simulator to test frames
 For simulation "NULL port" is required. 
 * App to create virtual pipe is called
http://www.hhdsoftware.com/free-virtual-serial-ports
 
 Main application logic
 portList - list of all available ports
 selectedPort - currently opened port
 frameList - list of frame types
 _connector - works with frames
 */

using ARMDevice.ARMCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using USBTetminal2;
using USBTetminal2.Protocol;


namespace ARMDevice
{
    public class ARMWindowViewModel : ViewModelBase
    {
        #region fields
        private ObservableCollection<ARMCustomSerialPort> portList;
        private ARMCustomSerialPort selectedPort;
        private ObservableCollection<AbstractFrame> frameList;
        private string inBox;
        private string outBox;
        ARMWindow _armWindow;
     //   ARM_USB_Connector _connector;
        #endregion

        public ARMWindowViewModel()
        {
            _armWindow = App.Current.MainWindow as ARMWindow;
            initializeCommandBindings();
         // 01 06 00 12 00 2B 00 24 0A 6C AA
        }



        #region public properties
        public ObservableCollection<ARMCustomSerialPort> Ports
        {
            get
            {
                if (portList == null)
                {
                    int num = 0;
                    portList = new ObservableCollection<ARMCustomSerialPort>(
                        SerialPort.GetPortNames()
                        .OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out num) ? num : 0)
                        .Select(portName => new ARMCustomSerialPort(portName))
                        );
                }
                return portList;
            }
            set
            {
                portList = value;
                OnPropertyChanged("Ports");
            }
        }

        public ARMCustomSerialPort SelectedPort
        {
            get
            {
               return selectedPort;
            }
            set
            {
                if (selectedPort != null && selectedPort.IsOpen)
                    selectedPort.Close();
                if (value != null && !value.IsOpen)
                    value.Open();
                selectedPort = value;
                OnPropertyChanged("SelectedPort");
            }
        }


        public ObservableCollection<AbstractFrame> Frames
        {
            get
            {
                if (frameList == null)
                {
                    frameList = new ObservableCollection<AbstractFrame>();
                    frameList.Add(new IdentificatorFrame());
                    frameList.Add(new MeasurmentFrame());
                    frameList.Add(new TestFrame());
                }
                return frameList;
            }
            set
            {
                frameList = value;
                OnPropertyChanged("Frames");
            }
        }

        public string InBox
        {
            get { return inBox; }
            set
            {
                inBox = value;
                OnPropertyChanged("InBox");
            }
        }

        public string OutBox
        {
            get { return outBox; }
            set
            {
                outBox = value;
                OnPropertyChanged("OutBox");
            }
        }
        #endregion


        #region private methods


        private object getSelectedFrame()
        {
            return _armWindow.frames.SelectedItem;
        }

        private AbstractFrame getFrameFromData(byte[] data)
        {
            
            return null;
        }

        #endregion


        #region commands region


        private void initializeCommandBindings()
        {
           // createCommandBinding(ARMCustomCommands.Reset, onResetExecute, onResetCanExecute);
            createCommandBinding(ARMCustomCommands.ErrorReport, onErrorReport, onErrorReportCanExecute);
            createCommandBinding(ARMCustomCommands.Connect, onConnect, onConnectCanExecute);
            createCommandBinding(ARMCustomCommands.SendStandartFrame, onSendStandartFrame, onSendStandartFrameCanExecute);
            createCommandBinding(ARMCustomCommands.SendCustomFrame, onSendCustomFrame, onSendCustomFrameCanExecute);
            createCommandBinding(ARMCustomCommands.DataRecived, onDataRecived, onDataRecivedCanExecute);

        }

        #region DataRecivedCommand
        private void onDataRecived(object sender, ExecutedRoutedEventArgs e)
        {
            AbstractFrame frame = (AbstractFrame)getSelectedFrame();
            if (frame == null)
            {
                ARMCustomCommands.ErrorReport.Execute("Data Recived, but frame is still not selected", App.Current.MainWindow);
                return;
            }

            byte[] data = SelectedPort.RecivedData;
            frame.tryPrase(data);

        }

        private void onDataRecivedCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region SendCustomFrameCommand
        private void onSendCustomFrameCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = e.Parameter != null && e.Parameter.ToString().Length>0;
        }

        private void onSendCustomFrame(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter.ToString() != "777")
                SelectedPort.SendData(e.Parameter.ToString());
            else//suepruser
            {
                TestFrame frame = new TestFrame();
                for (int i = 0; i < 10; i++)
                {
                    byte[] data = frame.Request(USBTetminal2.Protocol.TestFrame.FrameType.GeneratePoints);
                    SelectedPort.SendData(data);
                    Thread.Sleep(50);
                }
            }
        }
        #endregion

        #region SendStandartFrameCommand
        private void onSendStandartFrameCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null && SelectedPort != null;
        }

        private void onSendStandartFrame(object sender, ExecutedRoutedEventArgs e)
        {
            AbstractFrame frame = (AbstractFrame)getSelectedFrame(); 
            byte[] data = frame.Request();
            SelectedPort.SendData(data);
            OnPropertyChanged("Ports");
        }
        #endregion

        #region ConnectCommand
        private void onConnectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null;
        }

        private void onConnect(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ErrorReportCommand
        private void onErrorReportCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = e.Parameter != null;
            e.CanExecute = true;
        }

        private void onErrorReport(object sender, ExecutedRoutedEventArgs e)
        {
            InBox += e.Parameter.ToString() + Environment.NewLine;
        }
        #endregion

        #region ResetCommand
        private void onResetCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != null;
        }

        private void onResetExecute(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        protected void createCommandBinding(ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            var binding = new CommandBinding(command, executed, canExecute);
            _armWindow.CommandBindings.Add(binding);
        }

        #endregion
    }
}
