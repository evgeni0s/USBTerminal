using System;
namespace Infrastructure
{
    public interface IViewModelProvider : IDisposable
    {
        T GetViewModel<T>(object model) where T : class, IViewModel;
    }
}
