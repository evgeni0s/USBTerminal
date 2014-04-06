using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBTetminal2.Commands;

namespace USBTetminal2.Graphs
{
    public class GraphsManager: ISimpleBroadcastListener
    {


        private void log(string logMsg)
        {
            CustomCommands.ErrorReport.Execute(logMsg, App.Current.MainWindow);
        }

        public void ReciveMessage(Utils.CommonBroadcastType smgType, object data)
        {
            switch (smgType)
            {
                case USBTetminal2.Utils.CommonBroadcastType.DECODE_BYTE_ARRAY_FROM_DEVICE:
                    break;
                case USBTetminal2.Utils.CommonBroadcastType.BUILD_GRAPH_FROM_Y_POINTS:
                    showPoints(data);
                    break;
                case USBTetminal2.Utils.CommonBroadcastType.msg3:
                    break;
                default:
                    break;
            }
        }

        private void showPoints(object data)
        {
            List<int> yPoints = (List<int>)data;
            log("GraphsManager: Recived Points!");
            foreach (int y in yPoints)
            {
                log(y.ToString());
            }
        }
    }
}
