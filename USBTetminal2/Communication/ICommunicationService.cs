using Modbus.Device;
using Modbus.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace USBTetminal2.Communication
{
    public interface ICommunicationService
    {
        event EventHandler OnPortsRefreshed;
        IEnumerable<CustomSerialPort> Ports { get; }
        CustomSerialPort GetPort(string name);
        ModbusSerialMaster GetDevice(string name);
        ICommand RefreshCommand { get; }
        bool Contains(string name);
    }
}
