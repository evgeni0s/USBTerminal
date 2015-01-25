using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBTetminal2.Communication
{
    public interface ICommunicationService
    {
        IEnumerable<CustomSerialPort> Ports { get; }
        CustomSerialPort Get(string name);
        bool Contains(string name);
    }
}
