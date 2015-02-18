/*SHOULD BE DELETED!!!!!!
 CustomSerialPort is much better
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using USBTetminal2.Commands;
using USBTetminal2.Properties;
using USBTetminal2.Protocol;

namespace USBTetminal2
{
    public class USBConnector
    {
        public enum DataMode { Text, Hex }
        public enum LogMsgType { Incoming, Outgoing, Normal, Warning, Error };

        #region Local Variables

        // The main control for communicating through the RS-232 port
        private SerialPort comport = new SerialPort();

        // Various colors for logging info
        private Color[] LogMsgTypeColor = { Colors.Blue, Colors.Green, Colors.Black, Colors.Orange, Colors.Red };

        // Temp holder for whether a key was pressed
      //  private bool KeyHandled = false;

        private Settings settings = Settings.Default;
        #endregion


        public USBConnector()
        {
            // When data is recieved through the port, call this method
            comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            comport.PinChanged += new SerialPinChangedEventHandler(comport_PinChanged);

            //Standart connection
            comport.BaudRate = 19200;
            comport.DataBits = 8;
            comport.StopBits = StopBits.One;
            comport.Parity = Parity.None;
            comport.WriteTimeout = SerialPort.InfiniteTimeout;
            //I have made this commit to ensure that gitignore nightmare is over!
        }

        private void comport_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            throw new NotImplementedException();
        }


        #region public methods



        public void connect(string name)
        {

            //CustomCommands.ErrorReport.Execute(frame.ByteArrayToHexString(request), null);//prints HEX
            bool error = false;

            // If the port is open, close it.
            if (comport.IsOpen) comport.Close();
            else
            {
                // Set the port's settings

                 comport.PortName = name;

                try
                {

                    int threadID = Thread.CurrentThread.ManagedThreadId;
                    // Open the port
                    comport.Open();
                    CustomCommands.ErrorReport.Execute("Connection is Successfull", null);
                    CustomCommands.ErrorReport.Execute("STARTING TEST", null);
                    CustomCommands.ErrorReport.Execute("Step 1: Composing request", null);

                    AbstractFrame frame = new MeasurmentFrame();
                    byte[] request = frame.Request();
                    frame.print();
                    CustomCommands.ErrorReport.Execute("Step 2: Sending request", null);

                    SendData(request);
                    
                }
                catch (UnauthorizedAccessException) { error = true; }
                catch (IOException) { error = true; }
                catch (ArgumentException) { error = true; }
            }
            CustomCommands.ErrorReport.Execute(error ? "Request Failed!" : "Request Successfull", null);
        }
        #endregion





        public void SendData(byte[] requestData)
        {
            try
            {
                comport.Write(requestData, 0, requestData.Length);
            }
            catch (Exception e)
            {
                //null - is used because this command will not be bind to UI directly
                CustomCommands.ErrorReport.Execute("Error while sending data to devise. Stack Trace: " + e.StackTrace, null);
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
  
                CustomCommands.ErrorReport.Execute("Step 3: Response obtained!", null);
                if (comport == null || !comport.IsOpen)
                {
                    CustomCommands.ErrorReport.Execute("Port is closed!", null);
                    return;
                }

                // Obtain the number of bytes waiting in the port's buffer
                int responseLength = comport.BytesToRead > 0 ? comport.BytesToRead : 1;

                // Create a byte array buffer to hold the incoming data
                byte[] response = new byte[responseLength];
                try
                {
                    // Read the data from the port and store it in our buffer
                    comport.Read(response, 0, responseLength);
                }
                catch
                {
                    CustomCommands.ErrorReport.Execute("Response failed!" + Environment.NewLine +
                        "Response length = " + responseLength, null);
                }

                CustomCommands.ErrorReport.Execute(String.Format("Reading compleated!"), null);
                foreach (byte b in response)
                {
                    CustomCommands.ErrorReport.Execute(b.ToString(), null);
                }


        }




    }
}
