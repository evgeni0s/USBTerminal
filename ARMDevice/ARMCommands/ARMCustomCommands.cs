using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ARMDevice.ARMCommands
{
    static class ARMCustomCommands
    {
        static ARMCustomCommands()
        {

            initializeSendStandartFrame();
            initializeSendCustomFrame();
            initializeConnectCommand();
            initializeErrorReport();
            initializeDataRecived();

        }


        private static void initializeErrorReport()
        {
            _errorReportCommand = new RoutedCommand("ErrorReport", typeof(ARMCustomCommands));
        }

        private static void initializeConnectCommand()
        {
            _connectCommand = new RoutedCommand("Connect", typeof(ARMCustomCommands));
        }

        private static void initializeSendStandartFrame()
        {
            _sendStandartFrameCommand = new RoutedCommand("SendStandartFrameCommand", typeof(ARMCustomCommands));
        }

        private static void initializeSendCustomFrame()
        {
            _sendCustomFrameCommand = new RoutedCommand("SendCustomFrameCommand", typeof(ARMCustomCommands));
        }

        private static void initializeDataRecived()
        {
            _dataRecivedCommand = new RoutedCommand("DataRecivedCommand", typeof(ARMCustomCommands));
        }



        public static RoutedCommand SendStandartFrame
        {
            get { return ARMCustomCommands._sendStandartFrameCommand; }
        }

        public static RoutedCommand SendCustomFrame
        {
            get { return ARMCustomCommands._sendCustomFrameCommand; }
        }

        public static RoutedCommand DataRecived
        {
            get { return ARMCustomCommands._dataRecivedCommand; }
        }

        public static RoutedCommand Connect
        {
            get { return ARMCustomCommands._connectCommand; }
        }

        public static RoutedCommand ErrorReport
        {
            get { return ARMCustomCommands._errorReportCommand; }
        }

        static RoutedCommand _sendStandartFrameCommand;
        static RoutedCommand _sendCustomFrameCommand;
        static RoutedCommand _dataRecivedCommand;
        static RoutedCommand _connectCommand;
        static RoutedCommand _errorReportCommand;

    }
}
