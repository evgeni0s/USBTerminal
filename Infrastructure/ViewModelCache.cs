using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    internal class ViewModelCache: IDisposable
    {
        Dictionary<object, IViewModel> _cash = new Dictionary<object, IViewModel>();


        internal T Get<T>(object model) where T : class
        {
            return _cash[model] as T;
        }

        internal bool Contains<T>(object model)
        {
            return _cash.ContainsKey(model);
        }

        internal void Cache(IViewModel viewModel)
        {
            _cash.Add(viewModel.Model, viewModel);
        }

        //List<IViewModel> _cash = new List<IViewModel>();

        public void Dispose()
        {
            if (_cash != null && _cash.Count>0)
            {
                int i = 0;
                i++;
            }
        }
    }
}
