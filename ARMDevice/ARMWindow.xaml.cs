using ARMDevice.ARMCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ARMDevice
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class ARMWindow : Window
    {
        public ARMWindow()
        {
            InitializeComponent();
            DataContext = new ARMWindowViewModel();
            Title = "Device simulator. " + Thread.CurrentThread.ManagedThreadId;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          //  ports.Items.Add(new CustomSerialPort("COM3"));
           // SelectedItem
           // ARMCustomCommands.ErrorReport.Execute("Test MSG", null);
        }
    }
}
