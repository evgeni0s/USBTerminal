using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using USBTetminal2.Communication;

namespace USBTetminal2.Controls.Settings
{
    public class SettingsViewModel: ViewModelBase, ISettingsViewModel//, IRegionMemberLifetime 
    {
        private ILoggerFacade _logger;
        private ICommunicationService _communicationService;
        public SettingsViewModel(ILoggerFacade logger, ICommunicationService communicationService)
        {
            _logger = logger;
            _communicationService = communicationService;
            OnRefreshPortList();
        }

        //TO DO: Add "refresh" icon to settings menu
        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new RelayCommand(OnRefreshPortList)); }
        }

        private void OnRefreshPortList(object obj = null)
        {
            AvailablePorts = new ObservableCollection<CustomSerialPort>(_communicationService.Ports);
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
            get { return _availablePorts; }
            set { _availablePorts = value;
            OnPropertyChanged("AvailablePorts");
            }
        }

        private void connect()
        {
            if (_selectedPort.IsOpen)
                _selectedPort.Close();
            if (!_selectedPort.IsOpen)
                _selectedPort.Open();
        }

    }
}
