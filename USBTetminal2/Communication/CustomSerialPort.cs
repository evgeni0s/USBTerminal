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

namespace USBTetminal2
{
    public class CustomSerialPort : SerialPort, INotifyPropertyChanged
    {

        Shell context;
        byte[] buffer;
        private Timer _statusCheckTimer;
        private readonly PortModel _model;

        public CustomSerialPort()
        {
            DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            PinChanged += new SerialPinChangedEventHandler(comport_PinChanged);

            //Poor null port drivers suggest to RtsEnable = true; 
            //Else I get exception “The parameter is incorrect.” when sending data
            //  RtsEnable = true;
        }

        ILoggerFacade _logger;
        public CustomSerialPort(PortModel model, ILoggerFacade logger)
           :this()
        {
            _model = model;
            _logger = logger;
            PortName = model.Name;
            BaudRate = model.BoudRate;
            DataBits = model.DataBits;
            Parity = model.Parity;
            StopBits = model.StopBits;
            //DO TO: this is antipattern. Think about elegant solution
            _statusCheckTimer = new Timer(OnCheckPortIsOpen, null, 1000, 2000);
        }

        //Port Opened/Closed event
        //bool _customSerialPortIsOpened;
        //public bool CustomSerialPortIsOpened
        //{
        //    get { return _customSerialPortIsOpened = IsOpen; }
        //    p
        //}
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
                    //CustomCommands.ErrorReport.Execute("Connection is Successfull", null);
                }
                else
                {
                    Close();
                    _logger.Log("DIsconected from " + PortName, Category.Info, Priority.Medium);
                    //CustomCommands.ErrorReport.Execute("Port is successfully closed", null);
                }

            }
            catch (UnauthorizedAccessException e) { error = true; errorMsg = e.Message; }
            catch (IOException e) { error = true; errorMsg = e.Message; }
            catch (ArgumentException e) { error = true; errorMsg = e.Message; }

            if (error)
                _logger.Log(String.Format("Error occured on port Close/Open command \n {0}", errorMsg), Category.Exception, Priority.Medium);
                //CustomCommands.ErrorReport.Execute(String.Format("Error occured on port Close/Open command \n {0}", errorMsg), null);
        }


        #endregion





        #region public properties


        #endregion



        ///// <summary>
        ///// THIS CLASS DOES ALL THE CONNECTION JOB
        ///// </summary>
        ///// <param name="Name">Name is an address to connect</param>

        //public CustomSerialPort(string Name)
        //    : this()
        //{
        //    PortName = Name;
        //}



        private void comport_PinChanged(object sender, SerialPinChangedEventArgs e)
        {

        }

        /// <summary>
        /// What ever is recived - is stored in this class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    LogOnUiThread("Data recived on " + PortName);
            //    buffer = new byte[BytesToRead];
            //    Read(buffer, 0, BytesToRead);
            //    CustomCommands.DataRecived.Execute(buffer, App.Current.MainWindow);
            //});


             byte[]  bArr = new byte[BytesToRead];
             Read(bArr, 0, BytesToRead);
             //string str = GetString(bArr);
             TestFrame frame = new TestFrame();
             string str = frame.ByteArrayToHexString(bArr);
             _logger.Log(str, Category.Info, Priority.Medium);
        }




        //Here is a wired Error: calling thread must be STA (single thread appartment)
        private void LogOnUiThread(string msg)
        {
            //Crashes when not on UI thread
            if (CustomCommands.ErrorReport.CanExecute(msg, context))
            {
                CustomCommands.ErrorReport.Execute(msg, context);
            }
        }

        public override string ToString()
        {
            return PortName;
        }


        public void SendData(byte[] data)
        {
            // Send the binary data out the port

            Thread thread = new Thread(() =>
            {
                // ReadTimeout = 2500;
                //WriteTimeout = 2500;
                bool rtsEnable = RtsEnable;
                Write(data, 0, data.Length);
            });
            thread.Start();

        }

        ~CustomSerialPort()
        {
            _statusCheckTimer = null;
            Close();
            buffer = null;
        }

        /// <summary>
        /// Data From last transaction. May be storing here is not the best idea.... MEMORY LEAK PLACE!!!!!!
        /// </summary>
        //public byte[] RecivedData
        //{ get { return buffer; } }

        //public void ReciveMessage(CommonBroadcastType smgType, object data)
        //{
        //    int i = 0;
        //    i++;
        //}

        #region convertors and utils ...not used for now
        //They solve encoding problem if it ever appears
        //http://stackoverflow.com/questions/472906/converting-a-string-to-byte-array
        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
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
    }
}
