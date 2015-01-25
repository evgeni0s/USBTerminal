using Infrastructure;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBTetminal2.Controls.Settings;

namespace USBTetminal2.Communication
{
    public class CommunicationService : ICommunicationService
    {

        private IUnityContainer _container;
        private IViewModelProvider _viewModelProvider;//provider does not realy fits here, but I'm reusing code
        public CommunicationService(IUnityContainer container, IViewModelProvider viewModelProvider)
        {
            _container = container;
            _viewModelProvider = viewModelProvider;
            OnRefreshPortList();
        }

        private void OnRefreshPortList(object obj = null)
        {
            
            foreach (var item in SerialPort.GetPortNames())
            {
                PortModel model = new PortModel() { Name = item, BoudRate = 9600, DataBits = 8, StopBits = StopBits.One };
                CustomSerialPort port = _viewModelProvider.GetViewModel<CustomSerialPort>(model);//Injects logger
                _cash.Add(port.PortName, port);
            }
        }

        #region Cash implementation
        private  Dictionary<string, CustomSerialPort> _cash = new Dictionary<string, CustomSerialPort>();
        public CustomSerialPort Get(string name)
        {
            return _cash[name];
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
