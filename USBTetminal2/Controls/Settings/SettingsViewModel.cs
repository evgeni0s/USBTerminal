using Microsoft.Practices.Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace USBTetminal2.Controls.Settings
{
    public class SettingsViewModel: ViewModelBase   //, ISimpleBroadcastListener
    {
        private ILoggerFacade _logger;
        public SettingsViewModel(ILoggerFacade logger)
        {
            _logger = logger;
            OnRefreshPortList();
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new RelayCommand(OnRefreshPortList)); }
        }

        private void OnRefreshPortList(object obj = null)
        {
            foreach (var item in SerialPort.GetPortNames())
            {
                PortModel model = new PortModel() { Name = item, BoudRate = 9600, DataBits = 8, StopBits = StopBits.One };
                CustomSerialPort port = new CustomSerialPort(model, _logger);
                AvailablePorts.Add(port);
            }
        }

        private CustomSerialPort _selectedPort;
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

        private ObservableCollection<CustomSerialPort> _availablePorts;
        public ObservableCollection<CustomSerialPort> AvailablePorts
        {
            get { return _availablePorts ?? (_availablePorts = new ObservableCollection<CustomSerialPort>()); }
        }



        //{
        //    get
        //    {
        //        return new ObservableCollection<PortModel>(CustomSerialPort.GetPortNames());
        //    }
        //}

        private void connect()
        {
            if (_selectedPort.IsOpen)
                _selectedPort.Close();
            if (!_selectedPort.IsOpen)
                _selectedPort.Open();
        }

        //public void ReciveMessage(CommonBroadcastType smgType, object data)
        //{
        //    switch (smgType)
        //    {
        //        case CommonBroadcastType.CONNECT_TO_DEVICE:
        //            connect();
        //            break;
        //    }
        //}

    }
}
