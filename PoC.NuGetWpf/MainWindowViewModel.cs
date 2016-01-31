using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using NuGet;
using PoC.NuGetWpf.Infrastructure;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PoC.NuGetWpf
{
    public class MainWindowViewModel : ReactiveViewModel
    {
        public MainWindowViewModel()
        {
            ModalController = new ModalController();
            _repo = PackageRepositoryFactory.Default.CreateRepository("https://www.nuget.org/api/v2/");
            _random = new Random();
            Page = 0;
            PageSize = 25;

            Load = ReactiveCommand.CreateAsyncObservable(_ => SearchImpl(Page, PageSize), RxApp.TaskpoolScheduler);
            Load.ThrownExceptions.Subscribe(ex => this.Log().Error($"Error occurred: {ex.ToString()}"));
            Load.IsExecuting.ObserveOn(RxApp.MainThreadScheduler).ToPropertyEx(this, x => x.IsBusy);

            Load.ToReadOnlyList(GetPackageCardViewModel)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToPropertyEx(this, x => x.Packages, new List<PackageCardViewModel>().AsReadOnly());

            var canNext = this.WhenAny(x => x.IsBusy, busy => !busy.Value);
            Next = ReactiveCommand.CreateAsyncObservable(canNext, _ =>
            {
                Page = Page + 1;
                return Load.ExecuteAsync();
            });
            Next.ThrownExceptions.Subscribe(ex => this.Log().Error($"Error occurred: {ex.ToString()}"));

            var canPrevious = this.WhenAny(x => x.IsBusy, x => x.Page, (busy, page) => !busy.Value && page.Value > 0);
            Previous = ReactiveCommand.CreateAsyncObservable(canPrevious, _ =>
            {
                Page = Page - 1;
                return Load.ExecuteAsync();
            });
            Previous.ThrownExceptions.Subscribe(ex => this.Log().Error($"Error occurred: {ex.ToString()}"));

            ShowDialog = ReactiveCommand.Create();
            ShowDialog.Subscribe(_ => ModalController.ShowModal(new FirstModalViewModel()));

            this.WhenActivated(d =>
            {
                d(Load.ExecuteAsync().Subscribe());
            });
        }

        IObservable<IEnumerable<IPackage>> SearchImpl(int page, int pageSize)
        {
            return Observable.Defer(() =>
            {
                return Observable.Return(_repo.GetPackages()
                    .Where(p => String.IsNullOrWhiteSpace(Filter) || p.Title.Contains(Filter) || p.Id.Contains(Filter))
                    .Where(p => p.IsLatestVersion)
                    .OrderByDescending(p => p.DownloadCount)
                    .Skip(page*pageSize)
                    .Take(pageSize)
                    .AsEnumerable());
            });
        }

        PackageCardViewModel GetPackageCardViewModel(IPackage pacakge)
        {
            var isInstalled = _random.Next(1, 10)%2 == 0;
            return isInstalled
                ? (PackageCardViewModel)new InstalledPackageCardViewModel(pacakge)
                : (PackageCardViewModel)new GalleryPackageCardViewModel(pacakge);
        }
        
        readonly Random _random;
        readonly IPackageRepository _repo;
        public IModalController ModalController { get; }

        public ReactiveCommand<IEnumerable<IPackage>> Load { get; }
        public ReactiveCommand<IEnumerable<IPackage>> Previous { get; }
        public ReactiveCommand<IEnumerable<IPackage>> Next { get; }
        public ReactiveCommand<object> ShowDialog { get; }
        
        [ObservableAsProperty] public IReadOnlyList<PackageCardViewModel> Packages { get; }
        [ObservableAsProperty] public bool IsBusy { get; }

        [Reactive] public string Filter { get; set; }
        [Reactive] public int Page { get; set; }
        [Reactive] public int PageSize { get; set; }
    }
}
