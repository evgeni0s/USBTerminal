using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBTetminal2
{
    public interface ISimpleBroadcastListener
    {
       // public void SendMessage(Utils.CommonBroadcastType msgType, object data);
       void ReciveMessage(Grahps.CommonBroadcastType smgType, object data);//Meaasges will be sent from MainViewModel
    }
}
