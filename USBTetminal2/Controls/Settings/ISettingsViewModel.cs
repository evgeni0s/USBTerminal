using System;
using System.Collections.Generic;
namespace USBTetminal2.Controls.Settings
{
    interface ISettingsViewModel 
    {
        List<USBTetminal2.CustomSerialPort> Ports { get; }
    }
}
