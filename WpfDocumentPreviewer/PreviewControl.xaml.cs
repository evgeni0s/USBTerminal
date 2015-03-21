using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.ComponentModel;



namespace WpfDocumentPreviewer
{
    /// <summary>
    /// Interaction logic for PreviewerControl.xaml
    /// </summary>
    public partial class PreviewerControl : UserControl,INotifyPropertyChanged
    {
        private string fileName;

        public string FileName
        {
            get { return System.IO.Path.GetFileName(fileName); }
            set
            {
                fileName = value;
                SetFileName(fileName);
                RaisePropertyChanged("ImageSource");
                RaisePropertyChanged("FileName");
            }
        }

        public ImageSource ImageSource
        {
            get { return IconFromFileName(fileName); }
        }
        
        private void SetFileName(string fileName)
        {

            if (previewHandlerHost1.Open(fileName) == false)
            {
                wb1.Visibility = Visibility.Visible;
                wb1.Navigate(fileName);

                wfh1.Visibility = Visibility.Collapsed;
            }
            else
            {
                wb1.Visibility = Visibility.Collapsed;
                wfh1.Visibility = Visibility.Visible;
            }

        }

        public PreviewerControl()
        {
            InitializeComponent();
        }


        internal BitmapSource IconFromFileName(string fileName)
        {
            BitmapImage bmpImage = new BitmapImage();
            if (fileName!=null && fileName.Contains("."))
            {
                try
                {
                    System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
                    Bitmap bmp = icon.ToBitmap();
                    MemoryStream strm = new MemoryStream();
                    bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);
                    bmpImage.BeginInit();
                    strm.Seek(0, SeekOrigin.Begin);
                    bmpImage.StreamSource = strm;
                    bmpImage.EndInit();
                }
                catch
                { }
            }

            return bmpImage;
        }

        private void wb1_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (wb1.Document == null) return;
            try
            {
                mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2)wb1.Document;
                if (doc.title == "Navigation Canceled")
                {
                    wb1.Visibility = Visibility.Collapsed;
                    wfh1.Visibility = Visibility.Visible;
                }
            }
            catch { }

        }
        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
