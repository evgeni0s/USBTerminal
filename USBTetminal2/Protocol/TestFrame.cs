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
            throw new NotImplementedException();
        }
    }
}
