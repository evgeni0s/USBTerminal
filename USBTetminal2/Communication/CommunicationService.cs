using Infrastructure;
using Microsoft.Practices.Unity;
using Modbus.Device;
using Modbus.IO;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using USBTetminal2.Controls.Settings;

namespace USBTetminal2.Communication
{
    /*
     Think about taking this service to different project. It references 
     * Modbus.dll
     * log4net.dll
     * FtdAdapter.dll
     * FTD2XX.dll (not COM and uses FtdAdapter)
     * Modbus.Extensions.dll
     * Unme.Common.dll
     */
    public class CommunicationService : ICommunicationService
    {
        public event EventHandler OnPortsRefreshed;
        private IUnityContainer _container;
        private IViewModelProvider _viewModelProvider;//provider does not realy fits here, but I'm reusing code
        public CommunicationService(IUnityContainer container, IViewModelProvider viewModelProvider)
        {
            _container = container;
            _viewModelProvider = viewModelProvider;
            OnRefreshPortList();
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new RelayCommand(OnRefreshPortList)); }
        }

        private void OnRefreshPortList(object obj = null)
        {
            List<string> openedPorts = new List<string>();
            foreach (var port in _cash.Values.ToList())
            {
                if (port.IsOpen)
                {
                    openedPorts.Add(port.PortName);
                }
                else
                {
                    _cash.Remove(port.PortName);
                    port.Dispose();
                }
            }
            foreach (var item in SerialPort.GetPortNames().Except(openedPorts))
            {
                PortModel model = new PortModel() { Name = item, BoudRate = 19200, DataBits = 8, StopBits = StopBits.One, DataMode = DataMode.Hex };
                CustomSerialPort port = _viewModelProvider.GetViewModel<CustomSerialPort>(model);//Injects logger
                _cash.Add(port.PortName, port);
            }
            if (OnPortsRefreshed != null)
            {
                OnPortsRefreshed(null, EventArgs.Empty);
            }
        }

        #region Cash implementation
        private  Dictionary<string, CustomSerialPort> _cash = new Dictionary<string, CustomSerialPort>();
        public CustomSerialPort GetPort(string name)
        {
            return _cash[name];
        }

        public ModbusSerialMaster GetDevice(string name)
        {
            var port = _cash[name];
            var device = ModbusSerialMaster.CreateRtu(port);
            device.Transport.ReadTimeout = port.ReadTimeout;
            device.Transport.WriteTimeout = port.WriteTimeout;
            device.Transport.Retries = 0;
            return device;
        }

        public bool Contains(string name)
        {
            return _cash.ContainsKey(name);
        }
        #endregion


        public IEnumerable<CustomSerialPort> Ports
        {
            get { return _cash.Values; }
        }

    }
}
