using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using NuGet;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            _repo = PackageRepositoryFactory.Default.CreateRepository("https://www.nuget.org/api/v2/");
            Load = ReactiveCommand.CreateAsyncObservable(SearchImpl);
            Load.ThrownExceptions.Subscribe(ex => Console.WriteLine("Error occurred: {0}", ex.ToString()));
            Load.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

            Load.Select(x => new List<PackageCardViewModel>(x.Select(GetPackageCardViewModel)).AsReadOnly())
                .ToProperty(this, x => x.Packages, out _packages, new List<PackageCardViewModel>().AsReadOnly());

            _random = new Random();
        }

        IObservable<IEnumerable<IPackage>> SearchImpl(object _)
        {
            return Observable.Start(() => 
                _repo.GetPackages()
                    .Where(p => String.IsNullOrWhiteSpace(Filter) || p.Title.Contains(Filter) || p.Id.Contains(Filter))
                    .Where(p => p.IsLatestVersion)
                    .OrderByDescending(p => p.DownloadCount)
                    .Take(10)
                    .AsEnumerable());
        }

        private PackageCardViewModel GetPackageCardViewModel(IPackage pacakge)
        {
            var isInstalled = _random.Next(1, 10)%2 == 0;
            return isInstalled
                ? (PackageCardViewModel)new InstalledPackageCardViewModel(pacakge)
                : (PackageCardViewModel)new GalleryPackageCardViewModel(pacakge);
        }

        readonly IPackageRepository _repo;
        public ReactiveCommand<IEnumerable<IPackage>> Load { get; }

        readonly ObservableAsPropertyHelper<IReadOnlyList<PackageCardViewModel>> _packages;
        public IReadOnlyList<PackageCardViewModel> Packages => _packages.Value;

        string _filter;
        readonly Random _random;

        public string Filter
        {
            get { return _filter; }
            set { this.RaiseAndSetIfChanged(ref _filter, value); }
        }

        readonly ObservableAsPropertyHelper<bool> _isBusy;
        public bool IsBusy => _isBusy.Value;
    }
}
