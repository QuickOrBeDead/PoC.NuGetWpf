using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public partial class InstalledPackageCardView : UserControl, IViewFor<InstalledPackageCardViewModel>
    {
        public InstalledPackageCardView()
        {
            InitializeComponent();

            var imageProvider = new ImageProvider();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, vm => vm.Title, v => v.Title.Text));
                d(this.OneWayBind(ViewModel, vm => vm.Description, v => v.Description.Text));
                d(this.OneWayBind(ViewModel, vm => vm.DownloadCount, v => v.DownloadCount.Text));
                d(this.OneWayBind(ViewModel, vm => vm.PublishedDate, v => v.PublishedDate.Text));
                d(this.OneWayBind(ViewModel, vm => vm.Authors, v => v.Authors.Text));
                d(this.OneWayBind(ViewModel, vm => vm.LatestVersion, v => v.LatestVersion.Text));
                d(this.OneWayBind(ViewModel, vm => vm.InstalledVersion, v => v.InstalledVersion.Text));
                d(this.OneWayBind(ViewModel, vm => vm.IconUrl, v => v.Icon.Source, uri => imageProvider.GetIcon(uri)));
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (InstalledPackageCardViewModel)value; }
        }

        public InstalledPackageCardViewModel ViewModel
        {
            get { return (InstalledPackageCardViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(InstalledPackageCardViewModel), typeof(InstalledPackageCardView));
    }
}
