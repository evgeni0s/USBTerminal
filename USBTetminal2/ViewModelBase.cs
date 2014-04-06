/*COPY PASTE FROM INTERNET TO IMPLLEMENT MVVM PATTERN*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBTetminal2
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public bool ThrowOnInvalidPropertyName { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
         //   Guard.ArgumentNotNullOrEmptyString(propertyName, "propertyName");

            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);

                handler(this, e);
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            this.Disposing();
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected virtual void Disposing()
        {
        }
        #endregion

        ///An attempt to create broadcast template for a short time
        //#region BroadCastRegion
        //public virtual void SendMessage(Utils.CommonBroadcastType msgType, object data);
        //public virtual void ReciveMessage(Utils.CommonBroadcastType smgType, object data);
        //#endregion
    }
}
