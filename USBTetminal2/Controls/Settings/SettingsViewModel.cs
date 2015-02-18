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
            AvailablePorts = new ObservableCollection<CustomSerialPort>(_communicationService.Ports);
            _communicationService.OnPortsRefreshed += OnPortsRefreshed;
        }

        private void OnPortsRefreshed(object sender, EventArgs e)
        {
            AvailablePorts = new ObservableCollection<CustomSerialPort>(_communicationService.Ports);
        }

        public ICommand RefreshCommand
        {
            get { return _communicationService.RefreshCommand; }
        }

        private ObservableCollection<CustomSerialPort> _availablePorts;
        public ObservableCollection<CustomSerialPort> AvailablePorts
        {
            get { return _availablePorts; }
            set { _availablePorts = value;
            OnPropertyChanged("AvailablePorts");
            }
        }

        public List<CustomSerialPort> Ports
        {
            get
            {
                return AvailablePorts.Where(port => port.IsOpen).ToList(); 
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
            
        }
        
    }
}
