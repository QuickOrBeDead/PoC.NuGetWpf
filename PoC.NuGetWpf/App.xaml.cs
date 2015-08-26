using System.Windows;
using Splat;

namespace PoC.NuGetWpf
{
    public partial class App : Application
    {
        public AppBootstrapper Bootstrapper { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Bootstrapper = new AppBootstrapper(Locator.CurrentMutable);
            base.OnStartup(e);
        }
    }
}
