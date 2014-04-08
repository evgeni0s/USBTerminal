/*COPY PASTE FROM INTERNET TO MAKE COMMANDS HANDY*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace USBTetminal2
{
   public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /**
         * @brief   Creates a new command.
        */
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("Execute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /*how to use
         * 
         * Command="{Binding RemoveLegend}" CommandParameter="{Binding RelativeSource={RelativeSource Self}}"
         *         RelayCommand _removeLegend;
         public ICommand RemoveLegend
         {
             get
             {
                 if (_removeLegend == null)
                 {
                     _removeLegend = new RelayCommand(param => this.onRemoveLegendExecute(param),
                         param => true);
                 }
                 return _removeLegend;
             }
         }

         private void onRemoveLegendExecute(Object sender)
         {
             int i = 0;
             i++;
         }
        
         */
    }
}
