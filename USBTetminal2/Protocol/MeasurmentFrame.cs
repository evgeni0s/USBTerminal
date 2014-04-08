/*
 * Command number 6
 * Generates request as byte[]
 * Extracts points from this array
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBTetminal2.Commands;

namespace USBTetminal2.Protocol
{
    public class MeasurmentFrame: AbstractFrame
    {
        private string COMMAND = "6";
        private List<int> points;

        /// <summary>
        /// Master (PC):	
        ///Adres urządzenia "slave"	0x01
        ///Komenda	0x06
        ///Dane - bajt 1	0x0B
        ///Dane - bajt 2	0xB8
        ///Koniec ramki	0xAA

        /// </summary>
        /// <returns>Data which are ready to be sent to device</returns>
        public override byte[] Request()
        {
            byte[] result = new byte[5];
            result[0] = Convert.ToByte(ADDRESS, 16);
            result[1] = Convert.ToByte(COMMAND, 16);
            result[2] = HexStringToByteArray(LUMINOSITY)[0];
            result[3] = HexStringToByteArray(LUMINOSITY)[1];
            result[4] = Convert.ToByte(STOP_BYTE, 16);
            return result;
            //return HexStringToByteArray(ADDRESS)
            //    .Concat(HexStringToByteArray(COMMAND))
            //    .Concat(HexStringToByteArray())
            //    .Concat(HexStringToByteArray(STOP_BYTE)).ToArray();
        }



        /// <summary>
        /// Slave (CO-1):	
        ///Adres urządzenia "slave"	0x01
        ///Komenda              	0x06
        ///Dane - bajt 1        	0xnn
        ///Dane - bajt 2        	0xnn
        ///……………	0x…
        ///Dane - bajt 236       	0xnn
        ///Koniec ramki	            0xAA
        ///
        /// 01 06 0B B8 AA    //Data should be between B8 and AA. Each number takes 2 bytes
        /// </summary>
        /// <param name="dataFromDevice"></param>
        /// <returns></returns>
        public override bool tryPrase(byte[] dataFromDevice)
        {
            if (dataFromDevice == null|| dataFromDevice.Length < 4)
                return false;
            points = new List<int>();
            // [0] - address
            // [1] - command

            // [2] < Data < dataFromDevice.Length - 2
            try
            {
                for (int i = 2; i < dataFromDevice.Length - 2; i += 2)
                {
                    
                    //0B B8
                    //string str = ByteArrayToHexString(new byte[2] { dataFromDevice[i], dataFromDevice[i + 1] });
                    int actualPort = 0;
                    if (BitConverter.IsLittleEndian)
                        actualPort = BitConverter.ToUInt16(new byte[2] {dataFromDevice[i + 1],dataFromDevice[i] }, 0);
                    else
                        actualPort = BitConverter.ToUInt16(new byte[2] { dataFromDevice[i], dataFromDevice[i+1] }, 0);
                    points.Add(actualPort);
                }
            }
            catch { return false; }
            return true;
        }

        public List<int> Points
        {
            get { return points; }
        }


        protected override string getName()
        {
            return "Measurment Frame";
        }

        public override void executeActionForThisFrame()
        {
            CustomCommands.PlotGraph.Execute(points, App.Current.MainWindow);
        }

    }
}
