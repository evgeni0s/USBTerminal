using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace USBTetminal2.Controls.Legend
{
    /// <summary>
    /// This class is a bit complex. It provides data for list and items inside that list
    /// LegendListViewModel - contains list of items
    /// LegendListItem - items themselves
    /// </summary>
    public class LegendListViewModel : ViewModelBase, ISimpleBroadcastListener
    {


        #region Class for ListItem
        /// <summary>
        /// Data for every particular item
        /// </summary>
        public class LegendListItem : ViewModelBase
        {
            private LineAndMarker<MarkerPointsGraph> _graph;
            private string description;
            private SolidColorBrush lineColor;
            public bool isChecked;
            public LegendListItem(LineAndMarker<MarkerPointsGraph> graph)
            {
                _graph = graph;
                Description = graph.LineGraph.Description.ToString();
                LineColor = graph.LineGraph.Stroke as SolidColorBrush;
                IsChecked = true;
            }

            public string Description
            {
                get { return description; }
                set
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }

            public bool IsChecked
            {
                get { return isChecked; }
                set
                {
                    isChecked = value;
                    _graph.LineGraph.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    _graph.MarkerGraph.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    OnPropertyChanged("IsChecked");
                }
            }

            public SolidColorBrush LineColor
            {
                get { return lineColor; }
                set
                {
                    lineColor = value;
                    OnPropertyChanged("LineColor");
                }
            }
        }

        #endregion



        public LegendListViewModel()
        {
            //
        }


        private ObservableCollection<LegendListItem> _legendsList = new ObservableCollection<LegendListItem>();
        public ObservableCollection<LegendListItem> LegendsList
        {
            get
            {
                return _legendsList;
            }
            set
            {
                _legendsList = value;
            }
        }

        public void Clear()
        {
            LegendsList = new ObservableCollection<LegendListItem>();
        }

        private void addNewLegend(object data)
        {
            if (data.GetType() != typeof(LineAndMarker<MarkerPointsGraph>))
                return;
            var graph = (LineAndMarker<MarkerPointsGraph>)data;
            LegendListItem legend = new LegendListItem(graph);
            LegendsList.Add(legend);
        }

        public void ReciveMessage(Grahps.CommonBroadcastType smgType, object data)
        {
            switch (smgType)
            {
                case USBTetminal2.Grahps.CommonBroadcastType.DECODE_BYTE_ARRAY_FROM_DEVICE:
                    break;
                case USBTetminal2.Grahps.CommonBroadcastType.BUILD_GRAPH_FROM_Y_POINTS:
                    break;
                case USBTetminal2.Grahps.CommonBroadcastType.ADD_LEGEND_TO_GRAPH:
                    addNewLegend(data);
                    break;
                case USBTetminal2.Grahps.CommonBroadcastType.msg3:
                    break;
                default:
                    break;
            }
        }
    }
}
