using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using NuGet;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            _repo = PackageRepositoryFactory.Default.CreateRepository("https://www.nuget.org/api/v2/");
            _random = new Random();
            Page = 0;
            PageSize = 25;

            Load = ReactiveCommand.CreateAsyncObservable(_ => SearchImpl(Page, PageSize));
            Load.ThrownExceptions.Subscribe(ex => Console.WriteLine("Error occurred: {0}", ex.ToString()));
            Load.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

            Load.ToReadOnlyList(GetPackageCardViewModel)
                .ToProperty(this, x => x.Packages, out _packages, new List<PackageCardViewModel>().AsReadOnly());

            var canNext = this.WhenAny(x => x.IsBusy, busy => !busy.Value);
            Next = ReactiveCommand.CreateAsyncObservable(canNext, _ =>
            {
                Page = Page + 1;
                return Load.ExecuteAsyncTask().ToObservable();
            });

            var canPrevious = this.WhenAny(x => x.IsBusy, x => x.Page, (busy, page) => !busy.Value && page.Value > 0);
            Previous = ReactiveCommand.CreateAsyncObservable(canPrevious, _ =>
            {
                Page = Page - 1;
                return Load.ExecuteAsyncTask().ToObservable();
            });
        }

        IObservable<IEnumerable<IPackage>> SearchImpl(int page, int pageSize)
        {
            return Observable.Start(() => 
                _repo.GetPackages()
                    .Where(p => String.IsNullOrWhiteSpace(Filter) || p.Title.Contains(Filter) || p.Id.Contains(Filter))
                    .Where(p => p.IsLatestVersion)
                    .OrderByDescending(p => p.DownloadCount)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .AsEnumerable());
        }

        private PackageCardViewModel GetPackageCardViewModel(IPackage pacakge)
        {
            var isInstalled = _random.Next(1, 10)%2 == 0;
            return isInstalled
                ? (PackageCardViewModel)new InstalledPackageCardViewModel(pacakge)
                : (PackageCardViewModel)new GalleryPackageCardViewModel(pacakge);
        }
        
        string _filter;
        readonly Random _random;
        readonly IPackageRepository _repo;

        public ReactiveCommand<IEnumerable<IPackage>> Load { get; }
        public ReactiveCommand<IEnumerable<IPackage>> Previous { get; }
        public ReactiveCommand<IEnumerable<IPackage>> Next { get; }

        readonly ObservableAsPropertyHelper<IReadOnlyList<PackageCardViewModel>> _packages;
        public IReadOnlyList<PackageCardViewModel> Packages => _packages.Value;

        public string Filter
        {
            get { return _filter; }
            set { this.RaiseAndSetIfChanged(ref _filter, value); }
        }

        int _page;
        public int Page
        {
            get { return _page; }
            set { this.RaiseAndSetIfChanged(ref _page, value); }
        }

        int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set { this.RaiseAndSetIfChanged(ref _pageSize, value); }
        }

        readonly ObservableAsPropertyHelper<bool> _isBusy;
        public bool IsBusy => _isBusy.Value;
    }
}
