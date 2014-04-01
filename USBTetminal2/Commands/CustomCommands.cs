using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace USBTetminal2.Commands
{
    static class CustomCommands
    {
        static CustomCommands()
        {
            initializeShowLegendCommand();
            initializeShowPoints();
            initializeReset();
            initializeRemoveLegendCommand();
            initializeConnectCommand();
            initializeErrorReport();
            initializeDataRecived();
            initializePlotGraph();
        }

        private static void initializeErrorReport()
        {
            _errorReportCommand = new RoutedCommand("ErrorReport", typeof(CustomCommands));
        }

        private static void initializeConnectCommand()
        {
            _connectCommand = new RoutedCommand("Connect", typeof(CustomCommands));
        }

        private static void initializeReset()
        {
            _showLegend = new RoutedCommand("ShowLegend", typeof(CustomCommands));
        }

        private static void initializeShowPoints()
        {
            _resetCommand = new RoutedCommand("Reset", typeof(CustomCommands));
        }

        private static void initializeShowLegendCommand()
        {
            _showPointsCommand = new RoutedCommand("ShowPoints", typeof(CustomCommands));
        }

        private static void initializeRemoveLegendCommand()
        {
            _removeLegendCommand = new RoutedCommand("RemoveLegend", typeof(CustomCommands));
        }

        private static void initializeDataRecived()
        {
            _dataRecivedCommand = new RoutedCommand("DataRecivedCommand", typeof(CustomCommands));
        }

        private static void initializePlotGraph()
        {
            _plotGraphCommand = new RoutedCommand("PlotGraph", typeof(CustomCommands));
        }


        public static RoutedCommand ShowLegend
        {
            get { return CustomCommands._showLegend; }
        }
        
        public static RoutedCommand Reset
        {
            get { return CustomCommands._resetCommand; }
        }
        
        public static RoutedCommand ShowPoints
        {
            get { return CustomCommands._showPointsCommand; }
        }

        public static RoutedCommand RemoveLegend
        {
            get { return CustomCommands._removeLegendCommand; }
        }

        public static RoutedCommand Connect
        {
            get { return CustomCommands._connectCommand; }
        }

        public static RoutedCommand ErrorReport
        {
            get { return CustomCommands._errorReportCommand; }
        }

        public static RoutedCommand DataRecived
        {
            get { return CustomCommands._dataRecivedCommand; }
        }

        public static RoutedCommand PlotGraph
        {
            get { return CustomCommands._plotGraphCommand; }
        }


        static RoutedCommand _showLegend;
        static RoutedCommand _resetCommand;
        static RoutedCommand _showPointsCommand;
        static RoutedCommand _removeLegendCommand;
        static RoutedCommand _connectCommand;
        static RoutedCommand _errorReportCommand;
        static RoutedCommand _dataRecivedCommand;
        static RoutedCommand _plotGraphCommand;
    }
}
