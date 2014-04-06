using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBTetminal2.Protocol
{

    /// <summary>
    /// Uses BroadCast, which is sent from MainViewModel
    /// Main Function is start recognition process for data from divece
    /// </summary>
    public class FrameManager: ISimpleBroadcastListener
    {

        private AbstractFrame getFrameFromData(object data)
        {
            if (data.GetType() != typeof (byte[]))
                return new TestFrame();
            byte[] byteData = (byte[])data;
            byte commandType = byteData[1];
            switch (commandType)
            {
                case 6:
                    return new MeasurmentFrame();
                default:
                    return new TestFrame();
            }
        }



        public void ReciveMessage(Grahps.CommonBroadcastType smgType, object data)
        {
            switch (smgType)
            {
                case USBTetminal2.Grahps.CommonBroadcastType.DECODE_BYTE_ARRAY_FROM_DEVICE:
                    AbstractFrame frame = getFrameFromData(data);
                    byte[] byteData = (byte[])data;
                    frame.tryPrase(byteData);
                    frame.executeActionForThisFrame();
                    break;
                case USBTetminal2.Grahps.CommonBroadcastType.BUILD_GRAPH_FROM_Y_POINTS:
                    break;
                case USBTetminal2.Grahps.CommonBroadcastType.msg3:
                    break;
                default:
                    break;
            }
        }
    }
}
