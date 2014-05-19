using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Infrastructure
{
    public interface IMeasurementsFeedService
    {
        DataTable GetData(string chartId);
    }
}
