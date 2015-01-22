/*
 NOT IMPLEMENTED => JUST A SCATCH!!!!!!!!
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBTetminal2.Commands;

namespace USBTetminal2.Protocol
{
    public class IdentificatorFrame: AbstractFrame
    {
        
        public const string CONNECTION_CODE = "1";
        public const string DEVICE_TYPE = "0B";
        
        public UInt16 status = 0;//ST
        public byte extraFunctions = 0;//FUN
        public UInt16 fabricNumber = 0;//NR_FAB
        public UInt16 productionYear = 0;//ROK_P

        public string mainStringInfo = "";

       

        public override byte[] Request()
        {
            byte[] result = new byte[8];
            result[0] = Convert.ToByte(AbstractFrame.START_BYTE, 16);//68h uint8_t START
            result[1] = 0;                       //
            result[2] = Convert.ToByte("8", 16);//  0008h uint16_t Długość ramki
            result[3] = Convert.ToByte(CONNECTION_CODE, 16);
            result[4] = Convert.ToByte(DEVICE_TYPE, 16);
            result[5] = HexStringToByteArray(ADDRESS)[0];
            result[6] = HexStringToByteArray(ADDRESS)[1];
            result[7] = Convert.ToByte(AbstractFrame.STOP_BYTE, 16);//16h uint8_t STOP
            return result;
           // if (BitConverter.IsLittleEndian)
           //     Array.Reverse(result);

        }


        public override bool tryPrase(byte[] dataFromDevice)
        {
            //If there are NO string values, minimal length is still 13
            if (dataFromDevice == null ||dataFromDevice.Length < 13)
                return false;
            try
            {

                status = (UInt16)BitConverter.ToInt16(new byte[dataFromDevice[7]], 0);
                extraFunctions = dataFromDevice[7];
                CustomCommands.ErrorReport.Execute("Data prased!", null);
            }
            catch { }
            return true;
        }



        public void err()
        {
            CustomCommands.ErrorReport.Execute("Error traker test is working!", App.Current.MainWindow);
        }



        protected override string getName()
        {
            return "Identificator Frame";
        }

        public override void executeActionForThisFrame()
        {
            throw new NotImplementedException();
        }
    }
}
