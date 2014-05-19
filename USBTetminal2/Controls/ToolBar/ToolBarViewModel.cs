using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace USBTetminal2.Controls.ToolBar
{

    /// <summary>
    /// NOT USED FOR NOW
    /// </summary>
    public class ToolBarViewModel : ViewModelBase
    {

        protected Shell _mainWindow = null;

        protected ToolBarViewModel()
        {
            _mainWindow = App.Current.MainWindow as Shell;
            
        }




        #region Commands

        //private ICommand _commandBufferExportTo;
        //private ICommand _commandBufferClear;
        //private ICommand _commandBufferAutoScrol;

        //public ICommand CommandBufferExportTo
        //{
        //    get
        //    {
        //        if (_commandBufferExportTo == null)
        //        {
        //            _commandBufferExportTo = new AlaskaCommand(p => OnExecuteBufferExportTo(), c => CanExecuteBufferExportTo());
        //        }

        //        return _commandBufferExportTo;
        //    }
        //}
        //public ICommand CommandBufferClear
        //{
        //    get
        //    {
        //        if (_commandBufferClear == null)
        //        {
        //            _commandBufferClear = new AlaskaCommand(p => OnExecuteBufferClear(), c => CanExecuteEBufferClear());
        //        }

        //        return _commandBufferClear;
        //    }
        //}
        //public ICommand CommandBufferAutoScrol
        //{
        //    get
        //    {
        //        if (_commandBufferAutoScrol == null)
        //        {
        //            _commandBufferAutoScrol = new AlaskaCommand(p => OnExecuteBufferAutoScrol(), c => CanExecuteBufferAutoScrol());
        //        }

        //        return _commandBufferAutoScrol;
        //    }
        //}

        #endregion
    }
}
