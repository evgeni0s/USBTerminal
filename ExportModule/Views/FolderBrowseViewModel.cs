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
    //folder is used by export module
    public class FolderBrowseViewModel : DialogViewModel
    {
        protected IRegion main;


        protected Action<string> selectedFileCallback;
        public FolderBrowseViewModel(Action<string> selectedFileCallback, IRegion main)
        {
            // TODO: Complete member initialization
            this.selectedFileCallback = selectedFileCallback;
            this.main = main;
            base.TreeBrowser.SelectedFolder = Properties.Settings.Default.SaveFolder;

        }

        public virtual Visibility SelectFileComboboxVisibility
        {
            get { return Visibility.Collapsed; }
        }


        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get { return _okCommand ?? (_okCommand = new RelayCommand(OkExecute)); }
        }

        protected virtual void OkExecute()
        {
            var view = main.GetView("FolderBrowserKey");
            if (view != null)
            {
                main.Remove(view);
            }
            Properties.Settings.Default.SaveFolder = base.TreeBrowser.SelectedFolder;
            Properties.Settings.Default.Save();
            if (selectedFileCallback != null)
            {
                selectedFileCallback(Properties.Settings.Default.SaveFolder);
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
