using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public partial class GalleryPackageCardView : UserControl, IViewFor<GalleryPackageCardViewModel>
    {
        public GalleryPackageCardView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.Bind(ViewModel, vm => vm.Title, v => v.Title.Text));
                d(this.Bind(ViewModel, vm => vm.Description, v => v.Description.Text));
                d(this.Bind(ViewModel, vm => vm.DownloadCount, v => v.DownloadCount.Text));
                d(this.Bind(ViewModel, vm => vm.PublishedDate, v => v.PublishedDate.Text));
                d(this.Bind(ViewModel, vm => vm.Authors, v => v.Authors.Text));
                d(this.Bind(ViewModel, vm => vm.LatestVersion, v => v.LatestVersion.Text));
            });
            
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (GalleryPackageCardViewModel)value; }
        }

        public GalleryPackageCardViewModel ViewModel
        {
            get { return (GalleryPackageCardViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(GalleryPackageCardViewModel), typeof(GalleryPackageCardView));
    }
}
