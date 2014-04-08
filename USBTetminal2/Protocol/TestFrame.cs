/*
 IS NOT USED FOR NOW
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBTetminal2.Commands;

namespace USBTetminal2.Protocol
{
    public class TestFrame : AbstractFrame
    {

        public override byte[] Request()
        {
            byte[] result = new byte[1];
            result[0] = Convert.ToByte(ADDRESS, 16);
            return result;
        }

        public override bool tryPrase(byte[] dataFromDevice)
        {
            //If there are NO string values, minimal length is still 13
            if (dataFromDevice == null)
                return false;
            try
            {
                CustomCommands.ErrorReport.Execute("Total Bytes recived : " + dataFromDevice.Length, App.Current.MainWindow);
            }
            catch { }
            return true;
        }

        protected override string getName()
        {
            return "Test Frame";
        }

        public override void executeActionForThisFrame()
        {
            CustomCommands.ErrorReport.Execute("TestFrame got executed!", App.Current.MainWindow);
        }




        public byte[] Request(FrameType type)
        {
            switch (type)
            {
                case FrameType.Measurment:
                     //Byte array to test measurment frame. It contains many points
                     string hexString = IntArrayToHexString(getPoints());//Extra action for debug
                     byte[] points = HexStringToByteArray(hexString);
                     MeasurmentFrame frame = new MeasurmentFrame();
                     List<byte> deviceResponse = new List<byte>(frame.Request());
                     deviceResponse.InsertRange(4, points);
                     return deviceResponse.ToArray();

                case FrameType.Identification:
                    break;
                case FrameType.GeneratePoints:
                     string hexString1 = IntArrayToHexString(func1(nextShift++));//Extra action for debug
                     byte[] points1 = HexStringToByteArray(hexString1);
                     MeasurmentFrame frame1 = new MeasurmentFrame();
                     List<byte> deviceResponse1 = new List<byte>(frame1.Request());
                     deviceResponse1.InsertRange(4, points1);
                     return deviceResponse1.ToArray();
                default:
                    break;
            }
            return Request();
        }

        public enum FrameType
        {
            Measurment,
            Identification,
            GeneratePoints
        }


        #region test Data provider



        int nextShift = 1;
        private IEnumerable<int> func1(int shift)
        {
            IEnumerable<int> y = Enumerable.Range(-100, 101);
            return y.Select(v => v + shift);
        }

        private IEnumerable<int> getPoints()
        {
            // Compute x array of 1001 points from 0 to 100 with 0.1 step
            IEnumerable<int> x = Enumerable.Range(0, 100);

            // Compute y array as sin(x)/x function defined on x grid
            int sinAmplitude = 10; // Sin func will be y =  [-50;50]
            var y = x.Select(v => (int)(Math.Abs(v) < 1e-10 ? 1 / sinAmplitude : Math.Sin(v) / sinAmplitude)).ToArray();
            return y;
        }
        


        #endregion
    }
}
