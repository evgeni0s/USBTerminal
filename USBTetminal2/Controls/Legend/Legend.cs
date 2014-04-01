using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace USBTetminal2.Controls.Legend
{

        public class Legend
        {
            public string Name { get; set; }
            public bool IsChecked { get; set; }
            public SolidColorBrush Brush { get; set; }

            public Legend(string name, bool isChecked, SolidColorBrush brush)
            {
                this.Name = name;
                this.IsChecked = isChecked;
                this.Brush = brush;
            }
        }
}
