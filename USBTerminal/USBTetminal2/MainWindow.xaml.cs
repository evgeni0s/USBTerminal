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
using USBTetminal2.Commands;
using USBTetminal2.Protocol;

namespace USBTetminal2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            Title = "Terminal window. " + Thread.CurrentThread.ManagedThreadId;
            //mPlotter.CopyScreenshotToClipboard // Can be usefull
            


         //   IdentificatorFrame frame = new IdentificatorFrame();

            //byte[] resultB = frame.Request();
            //string[] b = resultB.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).ToArray();
            ////b.Select(x => console.Text += x +Environment.NewLine);
            
            //foreach (string line in b)
            //{
            //    console.Text += line + Environment.NewLine;
            //}

            //foreach (byte line in resultB)
            //{
            //    console.Text += line + Environment.NewLine;
            //}
     
        }

        //private void onConnectClicked(object sender, RoutedEventArgs e)
        //{
        //    legendListView.ItemsSource = (DataContext as MainWindowViewModel).LegendsList;
        //}

        private void ThumbDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double newHeight = e.VerticalChange + legendContainer.ActualHeight;
            if (newHeight > legendContainer.MinHeight)
                legendContainer.Height = newHeight;
        }

        private void onlistLoaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void onSetPort(object sender, RoutedEventArgs e)
        {
            CustomCommands.Connect.Execute(portName.Text, null);
        }

        //FOR DEBUG ONLY
        private void onLoaded(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).showAllPorts() ;
        }
    }
}
