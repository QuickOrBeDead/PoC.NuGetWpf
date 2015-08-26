using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    /// <summary>
    /// Interaction logic for InstalledPackageCardView.xaml
    /// </summary>
    public partial class InstalledPackageCardView : UserControl, IViewFor<InstalledPackageCardViewModel>
    {
        public InstalledPackageCardView()
        {
            InitializeComponent();

            this.Bind(ViewModel, vm => vm.Title, v => v.Title.Text);
            this.Bind(ViewModel, vm => vm.Description, v => v.Description.Text);
            this.Bind(ViewModel, vm => vm.DownloadCount, v => v.DownloadCount.Text);
            this.Bind(ViewModel, vm => vm.PublishedDate, v => v.PublishedDate.Text);
            this.Bind(ViewModel, vm => vm.Authors, v => v.Authors.Text);
            this.Bind(ViewModel, vm => vm.LatestVersion, v => v.LatestVersion.Text);
            this.Bind(ViewModel, vm => vm.InstalledVersion, v => v.InstalledVersion.Text);
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
