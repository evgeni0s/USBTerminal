/*Handles Sending and reciveing data
 * Default BaudRate and Parity and etc
 * Works with byte[] does not know about frames
 * 
 */


using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using USBTetminal2.Commands;

namespace USBTetminal2
{
    public class CustomSerialPort : SerialPort, ISimpleBroadcastListener
    {

        MainWindow context;
        byte[] buffer;
        private CustomSerialPort()
        {
            DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            PinChanged += new SerialPinChangedEventHandler(comport_PinChanged);

            //Standart connection
            BaudRate = 9600;
            DataBits = 8;
            StopBits = StopBits.One;
            Parity = Parity.None;
            context = (MainWindow)App.Current.MainWindow;

            //Poor null port drivers suggest to RtsEnable = true; 
            //Else I get exception “The parameter is incorrect.” when sending data
          //  RtsEnable = true;
        }


        /// <summary>
        /// THIS CLASS DOES ALL THE CONNECTION JOB
        /// </summary>
        /// <param name="Name">Name is an address to connect</param>

        public CustomSerialPort(string Name)
            : this()
        {
            PortName = Name;
        }



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
            Application.Current.Dispatcher.Invoke(() =>
            {
                LogOnUiThread("Data recived on " + PortName);
                buffer = new byte[BytesToRead];
                Read(buffer, 0, BytesToRead);
                CustomCommands.DataRecived.Execute(buffer, App.Current.MainWindow);
            });
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


        public  void SendData(byte[] data)
        {
            // Send the binary data out the port

                Thread thread = new Thread(() => {
                   // ReadTimeout = 2500;
                    //WriteTimeout = 2500;
                    bool rtsEnable = RtsEnable;
                    Write(data, 0, data.Length);
                });
                thread.Start();
           
        }

        ~CustomSerialPort()
        {
            Close();
            buffer = null;
        }

        /// <summary>
        /// Data From last transaction. May be storing here is not the best idea.... MEMORY LEAK PLACE!!!!!!
        /// </summary>
        public byte[] RecivedData
        { get { return buffer; } }

        public void ReciveMessage(CommonBroadcastType smgType, object data)
        {
            int i = 0;
            i++;
        }

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
    }
}
