using System;
namespace USBTetminal2.Controls.Settings
{
    interface ISettingsViewModel 
    {
        USBTetminal2.CustomSerialPort SelectedPort { get; set; }
    }
}
