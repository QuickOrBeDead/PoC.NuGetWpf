using ReactiveUI;
using Splat;

namespace PoC.NuGetWpf
{
    public class AppBootstrapper
    {
        public AppBootstrapper(IMutableDependencyResolver dependencyResolver)
        {
            // Bind 
            RegisterParts(dependencyResolver);

            // TODO: This is a good place to set up any other app 
            // startup tasks, like setting the logging level
            LogHost.Default.Level = LogLevel.Debug;
        }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));

            dependencyResolver.Register(() => new MainWindow(), typeof(IViewFor<MainWindowViewModel>));
            dependencyResolver.Register(() => new GalleryPackageCardView(), typeof(IViewFor<GalleryPackageCardViewModel>));
            dependencyResolver.Register(() => new InstalledPackageCardView(), typeof(IViewFor<InstalledPackageCardViewModel>));
        }
    }
}