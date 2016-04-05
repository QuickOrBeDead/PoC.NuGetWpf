using System;
using ReactiveUI;

namespace PoC.NuGetWpf.Infrastructure
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null) where T : class
        {
            var viewTypeName = viewModel.GetType().FullName.TrimEnd("Model".ToCharArray());
            var viewType = Type.GetType(viewTypeName);

            return Activator.CreateInstance(viewType) as IViewFor;
        }
    }
}