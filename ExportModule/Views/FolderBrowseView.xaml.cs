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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FolderBrowser;
using FolderBrowser.Behaviour;


namespace ExportModule.Views
{
    /// <summary>
    /// Used lib from codes project site
    /// This is analog: http://www.cnblogs.com/mschen/p/4288475.html
    /// </summary>
    public partial class FolderBrowseView : UserControl
    {
        public FolderBrowseView()
        {
            InitializeComponent();
        }

        private void DirectoryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingOperations.GetBindingExpression(FileComboBox, ComboBox.ItemsSourceProperty).UpdateTarget();
        }
    }
}
