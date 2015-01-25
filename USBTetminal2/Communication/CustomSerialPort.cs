/*Handles Sending and reciveing data
 * Default BaudRate and Parity and etc
 * Works with byte[] does not know about frames
 * 
 */


using Microsoft.Practices.Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using USBTetminal2.Commands;
using USBTetminal2.Controls.Settings;
using USBTetminal2.Protocol;
using Infrastructure;
using USBTetminal2.Utils;

namespace USBTetminal2
{
    public enum DataMode { Text, Hex }
    public class CustomSerialPort : SerialPort, INotifyPropertyChanged, IViewModel
    {
        private Timer _statusCheckTimer;
        private PortModel _model;
        private ILoggerFacade _logger;

        public CustomSerialPort(ILoggerFacade logger)
        {
            _logger = logger;
            DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            PinChanged += new SerialPinChangedEventHandler(comport_PinChanged);
            _statusCheckTimer = new Timer(OnCheckPortIsOpen, null, 1000, 2000);

            //Poor null port drivers suggest to RtsEnable = true; 
            //Else I get exception “The parameter is incorrect.” when sending data
            //  RtsEnable = true;
        }

        public bool CustomSerialPortIsOpened
        { get; private set; }
        private void OnCheckPortIsOpen(object state)
        {
            if (CustomSerialPortIsOpened != IsOpen)
            {
                CustomSerialPortIsOpened = IsOpen;
                OnPropertyChanged("CustomSerialPortIsOpened");
            }
        }



        #region Commands

        private ICommand _openClosePortCommand;
        public ICommand OpenClosePortCommand
        {
            get { return _openClosePortCommand ?? (_openClosePortCommand = new RelayCommand(OpenClosePort)); }
        }

        private void OpenClosePort(object obj)
        {
            bool error = false;
            string errorMsg = "";
            try
            {
                if (!IsOpen)
                {
                    Open();
                    _logger.Log("Connected to " + PortName, Category.Info, Priority.Medium);
                }
                else
                {
                    Close();
                    _logger.Log("DIsconected from " + PortName, Category.Info, Priority.Medium);
                }

            }
            catch (UnauthorizedAccessException e) { error = true; errorMsg = e.Message; }
            catch (IOException e) { error = true; errorMsg = e.Message; }
            catch (ArgumentException e) { error = true; errorMsg = e.Message; }

            if (error)
                _logger.Log(String.Format("Error occured on port Close/Open command \n {0}", errorMsg), Category.Exception, Priority.Medium);
        }


        #endregion

        #region public properties

        private DataMode _dataMode;
        public DataMode DataMode
        {
            get { return _dataMode; }
            set
            {
                _dataMode = value;
                OnPropertyChanged("DataMode");
            }
        }
        #endregion



        private void comport_PinChanged(object sender, SerialPinChangedEventArgs e)
        {

        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            if (!IsOpen) return;
            switch (DataMode)
            {
                case DataMode.Text:
                    string data = ReadExisting();
                    _logger.Log(data, Category.Info, Priority.Medium);
                    break;
                case DataMode.Hex:
                    byte[] buffer = new byte[BytesToRead];
                    Read(buffer, 0, BytesToRead);
                    _logger.Log(ByteArrayToHexString(buffer), Category.Info, Priority.Medium);
                    break;
                default:
                    break;
            }
        }

        public override string ToString()
        {
            return PortName;
        }

        public void SendData(string data)
        {
            switch (DataMode)
            {
                case DataMode.Text:
                    // Send the user's text straight out the port
                    Write(data);
                    // Show in the terminal window the user's text
                    _logger.Log(data, Category.Info, Priority.Medium);
                    break;
                case DataMode.Hex:
                    try
                    {
                        byte[] bdata = HexStringToByteArray(data);
                        Write(bdata, 0, bdata.Length);
                        _logger.Log(ByteArrayToHexString(bdata), Category.Info, Priority.Medium);
                    }
                    catch (FormatException)
                    {
                        _logger.Log("Not properly formatted hex string: " + data + "\n", Category.Exception, Priority.Medium);
                    }
                    break;
                default:
                    break;
            }
        }



        #region convertors 
        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }

        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
        #endregion

        #region PropertyChanged part
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);

                handler(this, e);
            }
        }
        #endregion

        public object Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value as PortModel;
                if (_model == null) return;
                PortName = _model.Name;
                BaudRate = _model.BoudRate;
                DataBits = _model.DataBits;
                Parity = _model.Parity;
                StopBits = _model.StopBits;
            }
        }

        ~CustomSerialPort()
        {
            _statusCheckTimer = null;
            Close();
        }

    }
}
