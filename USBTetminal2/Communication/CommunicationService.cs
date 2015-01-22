using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBTetminal2.Communication
{
    public class CommunicationService : ICommunicationService
    {
        /*пока только идеи
         нужно попробовать подмени интерфейса
         идеально для Console
         •••
         в конструктор принимает порт 
         * CustomRichTextBox(port)
         порт или порты могут меняться
         
         http://rules.ssw.com.au/SoftwareDevelopment/RulesToBetterMVC/Pages/Use-a-Dependency-Injection-Centric-Architecture.aspx
         * Из этой диаграмки выходит что может быть BinaryDataSender, StringDataSender и HexDataSender  :IDataSender
         *                                           слишком малы и пока вырождаются в методы, или как модель данных! 
         *                                           1) добавить в порт поле EDataSenderType.  
         *                                           
         CustomRichTextBox(IDataSender)
         * DataRecived - приходит через логи
         * 
         
         * 
         * Графики... они принимают данные. 2) Добавить в графики IPortListener
         * Excel...  
         * 
         * CommunicationService
         */
        /*
         
         ◘◘◘◘◘◘◘◘◘◘◘
         * 
         * 
         CommunicationService содержит список портов
         * ему сначала нужно слать сообщения на Консоль и график
         * потом на Excel
         * 
         * 
         * тогда эти 3 части могли бы унаследоваться от ISimpleBroadcastListener
         * 
         * 
         * 
         * •••••••
         * ToolBarViewModel - может управлять view, но ViewModel находятся за 3-9 земель в MainWindowVM
         */

        public void SendData(byte[] data)
        {
            // Send the binary data out the port
            //Write(data, 0, data.Length);
        }

        public void SendData(string data)
        {
            //TestFrame frame = new TestFrame();
            //byte[] bytes = frame.HexStringToByteArray(data);
            //SendData(bytes);
        }
    }
}
