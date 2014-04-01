/*
Common features of all frames
 Any frame can:
 - Create request
 - Check request
 - Manualy execute command associated with this frame. Example: Measurment frame can start bilding a graph
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBTetminal2.Commands;

namespace USBTetminal2.Protocol
{
    public abstract class AbstractFrame
    {
        /// <summary>
        /// Builds request
        /// </summary>
        /// <returns>Returns data which are requred for this type of request</returns>
        public abstract byte[] Request();
        protected abstract string getName();
        public abstract bool tryPrase(byte[] dataFromDevice);
        public abstract void executeActionForThisFrame();


        protected string ADDRESS = "1"; // FFFF is default but response from device can change it
        protected const string START_BYTE = "1";
        protected const string STOP_BYTE = "AA";
        /// <summary>
        /// Fixed light intensity is used
        /// </summary>
        protected const string LUMINOSITY = "0BB8";//Wartość naświetlenia 

        /// <summary>
        /// Is used for validation. Override is mandatory
        /// </summary>
        string expectedResponseCode { get; set; }

        /// <summary>
        /// Used to test ARM device
        /// </summary>
        public string Name
        {
            get { return getName(); }
        }

        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
        /// <returns> Returns an array of bytes. </returns>
        public byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            if (s.Length < 2)
                return new byte[]{Convert.ToByte(s, 16)};
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);//16 - type
            return buffer;
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
        public string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }



        /// <summary>
        /// FOR DEBUG PURPOSES ONLY. Allows to display request string
        /// </summary>
        public void print() 
        {
          string[] b = Request().Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).ToArray();
          foreach (string line in b)
          {
              CustomCommands.ErrorReport.Execute(line, null);
          }
        
        }


       
    }
}
