using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBTetminal2
{
    public enum CommonBroadcastType
    {
        DECODE_BYTE_ARRAY_FROM_DEVICE,
        BUILD_GRAPH_FROM_Y_POINTS,
        ADD_LEGEND_TO_GRAPH,
        DELETE_LEGEND,
        DELETE_GRAPH,
        CLEAR_ALL,
        CHANGE_MARKERS_VISIBILITY,
        USER_CHANGED_LEGEND_CONTAINER_VISIBILITY,
        CONNECT_TO_DEVICE,
        msg3
    }
}
