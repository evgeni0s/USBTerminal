using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExportModule.Views
{
    //I've inheroted this control because I'm too lasy
    //FileBrowseViewModel is used by console
    class FileBrowseViewModel: FolderBrowseViewModel
    {
        public FileBrowseViewModel(Action<string> selectedFileCallback, IRegion main)
            :base(selectedFileCallback, main)
        {

        }
        protected override void OkExecute()
        {
            if (!UserFileName.Contains(".txt"))
            {
                UserFileName += ".txt";
            }
            string filePath = base.TreeBrowser.SelectedFolder + "\\" + UserFileName; ;

            var view = main.GetView("FolderBrowserKey");
            if (view != null)
            {
                main.Remove(view);
            }
            Properties.Settings.Default.SaveFolder = base.TreeBrowser.SelectedFolder;
            Properties.Settings.Default.Save();
            if (selectedFileCallback != null)
            {
                selectedFileCallback(filePath);
            }
        }


        public IEnumerable Files
        {
            get
            {
                if (string.IsNullOrEmpty(TreeBrowser.SelectedFolder))
                    return new string[] { "" };
                return new DirectoryInfo(base.TreeBrowser.SelectedFolder).GetFiles().Select(file => file.Name); ;
            }
        }

        private string _selectedFile;
        public string SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                RaisePropertyChanged("SelectedItem");
            }
        }
        public string UserFileName
        {
            get;
            set;
        }


        public override Visibility SelectFileComboboxVisibility
        {
            get { return Visibility.Visible; }
        }
    }
}
