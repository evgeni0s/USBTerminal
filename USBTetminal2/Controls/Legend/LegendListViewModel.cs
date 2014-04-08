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
using System.Windows.Input;
using USBTetminal2.Commands;
using System.Windows;

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

            public LineAndMarker<MarkerPointsGraph> Graph
            {
                get
                {
                    return _graph;
                }
            }
        }

        #endregion


        Visibility _containerVisibility = Visibility.Visible;
        public LegendListViewModel()
        {
            //
        }


        public Visibility ContainerVisibility
        {
            get
            {
                if (LegendsList.Count <= 0)
                    return Visibility.Collapsed;
                return _containerVisibility;
            }
            set 
            {
                _containerVisibility = value;
                OnPropertyChanged("ContainerVisibility");
            }
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
                OnPropertyChanged("ContainerVisibility");
                _legendsList = value;
            }
        }

        private void addNewLegend(object data)
        {
            if (data.GetType() != typeof(LineAndMarker<MarkerPointsGraph>))
                return;
            var graph = (LineAndMarker<MarkerPointsGraph>)data;
            LegendListItem legend = new LegendListItem(graph);
            LegendsList.Add(legend);
            OnPropertyChanged("ContainerVisibility");
        }


        private void deleteLegend(object data)
        {
            if (data.GetType() != typeof(LegendListItem))
                return;
            LegendListItem item = (LegendListItem)data;
            LegendsList.Remove((LegendListItem)data);
            OnPropertyChanged("ContainerVisibility");
            CustomCommands.RemoveGraph.Execute(item.Graph, App.Current.MainWindow);
        }


        private void clearAll()
        {
            LegendsList.Clear();// = new ObservableCollection<LegendListItem>();
            OnPropertyChanged("ContainerVisibility");
        }


        private void switchLegendVisibility()
        {
            if (ContainerVisibility == Visibility.Collapsed)
                ContainerVisibility = Visibility.Visible;

            else if(ContainerVisibility == Visibility.Visible)
                ContainerVisibility = Visibility.Collapsed;
        }

        public void ReciveMessage(CommonBroadcastType smgType, object data)
        {
            switch (smgType)
            {
                case CommonBroadcastType.ADD_LEGEND_TO_GRAPH:
                    addNewLegend(data);
                    break;
                case CommonBroadcastType.DELETE_LEGEND:
                    deleteLegend(data);
                    break;
                case CommonBroadcastType.CLEAR_ALL:
                    clearAll();
                    break;
                case CommonBroadcastType.USER_CHANGED_LEGEND_CONTAINER_VISIBILITY:
                    switchLegendVisibility();
                    break;
                default:
                    break;
            }
        }




    }
}
