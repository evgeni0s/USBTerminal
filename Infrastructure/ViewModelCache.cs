using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    //TO DO: redesign it all to support new demands. read comments 
    internal class ViewModelCache : IDisposable
    {
        Dictionary<object, IViewModel> _cash = new Dictionary<object, IViewModel>();


        internal T Get<T>(object model) where T : class
        {
            //Do not remove comments here!
            //return _cash[model] as T;  this did not work decause I wanted to create 2 different viewmodels with the same model
            return _cash.Where(kvp => kvp.Value is T).Select(kvp => kvp.Value).FirstOrDefault() as T;

        }

        internal bool Contains<T>(object model)
        {
            //Do not remove comments here!
            //return _cash.ContainsKey(model);  this did not work decause I wanted to create 2 different viewmodels with the same model
            //return _cash.Where(kvp => kvp.Value is T).Select(kvp => kvp.Value).FirstOrDefault() is T; in settings there are 2 ports with same name. Need other solution
            //             1) filter items of same class  2) filter class by model 
            return _cash.Where(kvp => kvp.Value is T).Where(kvp => kvp.Value.Model == model)
                //3) trash
                .Select(kvp => kvp.Value).FirstOrDefault() is T;
        }

        internal void Cache(IViewModel viewModel)
        {
            //REALY MESSY HOTFIX!!! If i want to use same model for several classes, then I have problem with same key fields
            //_cash.Add(viewModel.Model, viewModel);
            if (!_cash.ContainsKey(viewModel.Model))
                _cash.Add(viewModel.Model, viewModel);
            else
            {
                //this should be fixed!!
            }
        }

        //List<IViewModel> _cash = new List<IViewModel>();

        public void Dispose()
        {
            if (_cash != null)
            {
                foreach (var item in _cash.Values)
                {
                    item.Dispose();
                }
            }
        }
    }
}
