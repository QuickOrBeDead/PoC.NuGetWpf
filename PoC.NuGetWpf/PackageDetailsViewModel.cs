using System;
using NuGet;
using PoC.NuGetWpf.Infrastructure;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public class PackageDetailsViewModel : ReactiveViewModel
    {
        public PackageDetailsViewModel()
        {
            
        }

        PackageCardViewModel _selectedPackage;
        public PackageCardViewModel SelectedPackage
        {
            get { return _selectedPackage; }
            set { this.RaiseAndSetIfChanged(ref _selectedPackage, value); }
        }
    }
}
