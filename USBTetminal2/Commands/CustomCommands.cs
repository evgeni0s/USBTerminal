using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace USBTetminal2.Commands
{
    //Note: need to write util for adding and removeing commands automatically
    static class CustomCommands
    {
        static CustomCommands()
        {
            initializeShowLegendCommand();
            initializeLegendContainerVisibilityCommand();
            initializeAddNewLegend();
            initializeRemoveLegendCommand();

            initializeShowPoints();
            initializePlotGraph();
            initializeRemoveGraph();

            initializeChangeMarkersVisibility();
            initializeShowSettingsDialog();
            initializeConsoleVisibility();
            initializeLoadDataToGrid();

            initializeDataRecived();

            initializeReset();
            initializeConnectCommand();
            initializeErrorReport();

        }

        private static void initializeConsoleVisibility()
        {
            _consoleVisibilityCommand = new RoutedCommand("ConsoleVisibility", typeof(CustomCommands));
        }

        private static void initializeLoadDataToGrid()
        {
            _loadDataToGridCommand = new RoutedCommand("LoadDataToGrid", typeof(CustomCommands));
        }

        private static void initializeShowSettingsDialog()
        {
            _showSettingsDialogCommand = new RoutedCommand("ShowSettingsDialog", typeof(CustomCommands));
        }

        private static void initializeChangeMarkersVisibility()
        {
            _changeMarkersVisibilityCommand = new RoutedCommand("ChangeMarkersVisibility", typeof(CustomCommands));
        }

        private static void initializeRemoveGraph()
        {
            _removeGraphCommand = new RoutedCommand("RemoveGraph", typeof(CustomCommands));
        }

        private static void initializeAddNewLegend()
        {
            _addNewLegendCommand = new RoutedCommand("AddNewLegend", typeof(CustomCommands));
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

        private static void initializeLegendContainerVisibilityCommand()
        {
            _legendContainerVisibilityCommand = new RoutedCommand("LegendContainerVisibility", typeof(CustomCommands));
        }

        private static void initializeDataRecived()
        {
            _dataRecivedCommand = new RoutedCommand("DataRecivedCommand", typeof(CustomCommands));
        }

        private static void initializePlotGraph()
        {
            _plotGraphCommand = new RoutedCommand("PlotGraph", typeof(CustomCommands));
        }

        public static RoutedCommand AddNewLegend
        {
            get { return CustomCommands._addNewLegendCommand; }
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

        public static RoutedCommand LegendContainerVisibility
        {
            get { return CustomCommands._legendContainerVisibilityCommand; }
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

        public static RoutedCommand RemoveGraph
        {
            get { return CustomCommands._removeGraphCommand; }
        }

        public static RoutedCommand ChangeMarkersVisibility
        {
            get { return CustomCommands._changeMarkersVisibilityCommand; }
        }

        public static RoutedCommand ShowSettingsDialog
        {
            get { return CustomCommands._showSettingsDialogCommand; }
        }

        public static RoutedCommand LoadDataToGrid
        {
            get { return CustomCommands._loadDataToGridCommand; }
        }

        public static RoutedCommand ConsoleVisibility
        {
            get { return CustomCommands._consoleVisibilityCommand; }
        }
        static RoutedCommand _showLegend;
        static RoutedCommand _resetCommand;
        static RoutedCommand _showPointsCommand;
        static RoutedCommand _removeLegendCommand;
        static RoutedCommand _legendContainerVisibilityCommand;
        static RoutedCommand _connectCommand;
        static RoutedCommand _errorReportCommand;
        static RoutedCommand _dataRecivedCommand;
        static RoutedCommand _plotGraphCommand;
        static RoutedCommand _addNewLegendCommand;
        static RoutedCommand _removeGraphCommand;
        static RoutedCommand _changeMarkersVisibilityCommand;
        static RoutedCommand _showSettingsDialogCommand;
        static RoutedCommand _loadDataToGridCommand;
        static RoutedCommand _consoleVisibilityCommand;
    }
}
