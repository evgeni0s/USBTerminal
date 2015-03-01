using FolderBrowser;
using FolderBrowser.Command;
using FolderBrowser.ViewModel;
using FolderBrowser.ViewModel.Base;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExportModule.Views
{
    public class FolderBrowseViewModel : DialogViewModel
    {
        private IRegion main;


        Action<string> selectedFileCallback;
        public FolderBrowseViewModel(Action<string> selectedFileCallback, IRegion main)
        {
            // TODO: Complete member initialization
            this.selectedFileCallback = selectedFileCallback;
            this.main = main;
            base.TreeBrowser.SelectedFolder = Properties.Settings.Default.SaveFolder;
        }


        public IEnumerable Files
        {
            get
            {
                if (string.IsNullOrEmpty(TreeBrowser.SelectedFolder))
                    return new string[] { "" };
                return new DirectoryInfo(base.TreeBrowser.SelectedFolder).GetFiles().Select(file => file.Name); ; }
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

        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get { return _okCommand ?? (_okCommand = new RelayCommand(OkExecute)); }
        }

        private void OkExecute()
        {
            if (!UserFileName.Contains(".txt"))
            {
                UserFileName += ".txt";
            }
            string filePath = base.TreeBrowser.SelectedFolder + "\\" + UserFileName;;

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



        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(CancelExecute)); }
        }

        private void CancelExecute()
        {
            var view = main.GetView("FolderBrowserKey");
            if (view == null)
            {
                main.Remove("FolderBrowserKey");
            }
        }
    }
}
