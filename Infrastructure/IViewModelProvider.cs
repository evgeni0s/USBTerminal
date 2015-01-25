using System;
namespace Infrastructure
{
    public interface IViewModelProvider
    {
        T GetViewModel<T>(object model) where T : class, IViewModel;
    }
}
