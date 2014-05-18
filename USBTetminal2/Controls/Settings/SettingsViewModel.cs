using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBTetminal2.Controls.Settings
{
    public class SettingsViewModel: ViewModelBase, ISimpleBroadcastListener
    {
        CustomSerialPort _selectedPort;
        public SettingsViewModel()
        {
            _selectedPort = new CustomSerialPort();
        }

        public CustomSerialPort SelectedPort
        {
            get
            {
               
                return _selectedPort;
            }
            set
            {
                //////FIX EXCEPTION!! Доступ к порту закрыт. Возникает если к порту подключена другая программа
                _selectedPort = value;
                OnPropertyChanged("SelectedPort");
            }
        }



        private void connect()
        {
            if (_selectedPort.IsOpen)
                _selectedPort.Close();
            if (!_selectedPort.IsOpen)
                _selectedPort.Open();
        }

        public void ReciveMessage(CommonBroadcastType smgType, object data)
        {
            switch (smgType)
            {
                case CommonBroadcastType.CONNECT_TO_DEVICE:
                    connect();
                    break;
            }
        }

    }
}
