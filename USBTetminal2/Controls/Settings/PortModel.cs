using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace USBTetminal2.Controls.Settings
{
    //Acts like Proxy. 
    public class PortModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private int _boudRate;
        public int BoudRate
        {
            get { return _boudRate; }
            set
            {
                _boudRate = value;
                OnPropertyChanged("BoudRate");
            }
        }

        private Parity _parity;
        public Parity Parity
        {
            get { return _parity; }
            set
            {
                _parity = value;
                OnPropertyChanged("Parity");
            }
        }

        private int _dataBits;
        public int DataBits
        {
            get { return _dataBits; }
            set
            {
                _dataBits = value;
                OnPropertyChanged("DataBits");
            }
        }

        private StopBits _stopBits;
        public StopBits StopBits
        {
            get { return _stopBits; }
            set
            {
                _stopBits = value;
                OnPropertyChanged("StopBits");
            }
        }

        //HOTFIX. There are problems identifying if port is open or closed

        //public void setIsOpened(bool newValue)
        //{
        //    _isOpened = newValue;
        //    OnPropertyChanged("IsOpened");
        //}

    }
}
