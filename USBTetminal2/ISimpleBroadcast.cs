/*Got no time to learn Prism pattern. 
 So I've made my own broadcasting framework:
 -All commands are executed in MainViewModel
 -MainViewModel.AddListener(ListenerClass) - adds any viewmodel to broadcasting space
 -MainViewModel.NotifyAllBroadcastListeners - sends message to all listeners
 */
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
       void ReciveMessage(CommonBroadcastType smgType, object data);//Meaasges will be sent from MainViewModel
    }
}
