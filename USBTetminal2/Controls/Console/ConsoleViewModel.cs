using ExportModule;
using Microsoft.Practices.Prism.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace USBTetminal2.Controls
{
        /// <summary>
        /// in current implementation Model is simply key "ConsoleKey". 
        /// I do not  need Complicated model for now
        /// </summary>
    public class ConsoleViewModel: ViewModelBase
    {
        private IExportModule _exportModule;
        private ILoggerFacade _logger;
        public ConsoleViewModel(ILoggerFacade logger, IExportModule exportModule)
        {
            _consolePresenter = logger as CustomRichTextBox;
            //Model = logger;
            _exportModule = exportModule;
            _logger = logger;
        }

        private CustomRichTextBox _consolePresenter;
        public CustomRichTextBox ConsolePresenter
        {
            get { return _consolePresenter; }
        }


        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(OnSave)); }
        }

        private void OnSave(object obj)
        {
            _exportModule.ShowFolderBrowserView(OnUserSelelectedFile);
        }

        private void OnUserSelelectedFile(string obj)
        {
            try
            {
                File.WriteAllText(obj, ConsolePresenter.GetText());
                Process.Start(obj);
                _logger.Log("Console's content is saved to:" + Environment.NewLine + obj, Category.Info, Priority.Medium);
            }
            catch(Exception e)
            {
                _logger.Log("Error saveing or opening file!" + Environment.NewLine + " Exception message: " + Environment.NewLine + e.Message, Category.Exception, Priority.Medium);
            }
        }


    }
}
