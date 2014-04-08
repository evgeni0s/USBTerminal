/*Handles Sending and reciveing data
 * Default BaudRate and Parity and etc
 * Works with byte[] does not know about frames
 * 
 * Reason for creating this ARM dublicate is that I needed to call ARM commands
 */

using ARMDevice.ARMCommands;
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
using USBTetminal2.Protocol;

namespace ARMDevice
{
    public class ARMCustomSerialPort : SerialPort
    {

        ARMWindow context;
        byte[] buffer;
        public ARMCustomSerialPort()
        {
            DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            PinChanged += new SerialPinChangedEventHandler(comport_PinChanged);
            
            //Standart connection
            BaudRate = 9600;
            DataBits = 8;
            StopBits = StopBits.One;
            Parity = Parity.None;
            context = (ARMWindow)App.Current.MainWindow;
        }

        public ARMCustomSerialPort(string Name) : this()
        {
            PortName = Name;
        }



        private void comport_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
           
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            buffer = new byte[BytesToRead];
            Read(buffer, 0, BytesToRead);
            Application.Current.Dispatcher.Invoke(() => 
                LogOnUiThread("Data recived on " + PortName)
                );
        }


 

        //Here is a wired Error: calling thread must be STA (single thread appartment)
        //Starting multiple threads to update UI
        private void LogOnUiThread(string msg)
        {
            //Crashes when not on UI thread. Use Dispatcher for this
            if (ARMCustomCommands.ErrorReport.CanExecute(msg, context))
            {
                ARMCustomCommands.ErrorReport.Execute(msg, context);
            }
            ARMCustomCommands.DataRecived.Execute(null, context);



        }

        public override string ToString()
        {
            return PortName; 
        }


        public void SendData(byte[] data)
        {
            // Send the binary data out the port
            Write(data, 0, data.Length);
        }

        public void SendData(string data)
        {
            TestFrame frame = new TestFrame();
            byte[] bytes = frame.HexStringToByteArray(data);
            SendData(bytes);
        }

        /// <summary>
        /// Data From last transaction
        /// </summary>
        public byte[] RecivedData
        { get { return buffer; } }
    }
}
