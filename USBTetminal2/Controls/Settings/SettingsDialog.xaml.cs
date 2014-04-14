using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace USBTetminal2.Controls.Settings
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void onClick(object sender, RoutedEventArgs e)
        {
            //// Configure open file dialog box 
            //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.Filter = "";
            //System.Drawing.
            //ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            //string sep = string.Empty;

            //foreach (var c in codecs)
            //{
            //    string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
            //    dlg.Filter = String.Format("{0}{1}{2} ({3})|{3}", dlg.Filter, sep, codecName, c.FilenameExtension);
            //    sep = "|";
            //}

            //dlg.Filter = String.Format("{0}{1}{2} ({3})|{3}", dlg.Filter, sep, "All Files", "*.*");

            //dlg.DefaultExt = ".png"; // Default file extension 

            //// Show open file dialog box 
            //Nullable<bool> result = dlg.ShowDialog();

            //// Process open file dialog box results 
            //if (result == true)
            //{
            //    // Open document 
            //    string fileName = dlg.FileName;
            //    // Do something with fileName  
            //}
        }
    }
}
