using ReactiveUI;

namespace PoC.NuGetWpf.Infrastructure
{
    public class ReactiveViewModel : ReactiveObject, ISupportsActivation
    {
        public ReactiveViewModel()
        {
            Activator = new ViewModelActivator();
        }

        public ViewModelActivator Activator { get; }
    }
}
