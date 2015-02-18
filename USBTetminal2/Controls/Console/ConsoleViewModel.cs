using Microsoft.Practices.Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBTetminal2.Controls
{
    public class ConsoleViewModel: ViewModelBase
    {
        /// <summary>
        /// in current implementation Model is simply key "ConsoleKey". 
        /// I do not  need Complicated model for now
        /// </summary>
        public ConsoleViewModel(ILoggerFacade logger)
        {
            _consolePresenter = logger as CustomRichTextBox;
            //Model = logger;
        }

        private CustomRichTextBox _consolePresenter;
        public CustomRichTextBox ConsolePresenter
        {
            get { return _consolePresenter; }
        }
    }
}
