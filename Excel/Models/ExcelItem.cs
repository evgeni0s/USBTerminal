using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel.Models
{
    public class ExcelItem
    {
        public string ChartId { get; set; }

        public string OutputFolder { get; set; }

        public DataSet ChartData { get; set; }
    }
}
