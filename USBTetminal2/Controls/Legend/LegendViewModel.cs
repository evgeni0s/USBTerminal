using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using USBTetminal2.Utils;

namespace USBTetminal2.Controls.Legend
{
    public class LegendViewModel : ViewModelBase
    {

        public Legend Legend;
        public LegendViewModel(Legend legend)
        {
            this.Legend = legend;
        }

        public string Name
        {
            get { return Legend.Name; }
            set
            {
                Legend.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool IsChecked
        {
            get { return Legend.IsChecked; }
            set
            {
                Legend.IsChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public SolidColorBrush Brush
        {
            get { return Legend.Brush; }
            set
            {
                Legend.Brush = value;
                OnPropertyChanged("Brush");
            }
        }


        #region BroadCastRegion
        public void SendMessage(Utils.CommonBroadcastType msgType, object data)
        {
            int i = 0;
            i++;
        }
        public void ReciveMessage(Utils.CommonBroadcastType smgType, object data)
        {
            int i = 0;
            i++;
        }
        #endregion
    }
}
