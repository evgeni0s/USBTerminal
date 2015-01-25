using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ViewModelProvider : Infrastructure.IViewModelProvider
    {
        private IUnityContainer _container;

        private ViewModelCache _viewModelCache = new ViewModelCache();

        public ViewModelProvider(IUnityContainer container)
        {
            _container = container;
        }

        public T GetViewModel<T>(object model) where T : class, IViewModel
        {
            if (_viewModelCache.Contains<T>(model))
                return _viewModelCache.Get<T>(model);

            var viewModel = _container.Resolve<T>();
            viewModel.Model = model;

            _viewModelCache.Cache(viewModel);

            return viewModel;
        }
    }
}
