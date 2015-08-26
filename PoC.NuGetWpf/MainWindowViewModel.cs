using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NuGet;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            _repo = PackageRepositoryFactory.Default.CreateRepository("https://www.nuget.org/api/v2/");
            Load = ReactiveCommand.CreateAsyncTask(_ =>
            {
                return Task.Run(() => _repo
                    .GetPackages()
                    .Where(p => String.IsNullOrWhiteSpace(Filter) || p.Title.Contains(Filter) || p.Id.Contains(Filter))
                    .Where(p => p.IsLatestVersion)
                    .OrderByDescending(p => p.DownloadCount)
                    .Take(10)
                    .AsEnumerable());
            });
            Load.ThrownExceptions.Subscribe(ex => Console.WriteLine("Error occurred: {0}", ex.ToString()));

            Load.Select(x => new ReactiveList<PackageCardViewModel>(x.Select(p => new PackageCardViewModel(p))))
                .ToProperty(this, x => x.Packages, out _packages, new ReactiveList<PackageCardViewModel>());
        }

        readonly IPackageRepository _repo;
        public ReactiveCommand<IEnumerable<IPackage>> Load { get; }

        readonly ObservableAsPropertyHelper<ReactiveList<PackageCardViewModel>> _packages;
        public ReactiveList<PackageCardViewModel> Packages => _packages.Value;

        string _filter;
        public string Filter
        {
            get { return _filter; }
            set { this.RaiseAndSetIfChanged(ref _filter, value); }
        }
    }
}
