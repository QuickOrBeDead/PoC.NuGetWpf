using System;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public partial class PackageDetailsView : UserControl, IViewFor<PackageDetailsViewModel>
    {
        public PackageDetailsView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, vm => vm.SelectedPackage, v => v.PackageCard.ViewModel));
            });
        }

        public PackageDetailsViewModel ViewModel
        {
            get { return (PackageDetailsViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof (PackageDetailsViewModel), typeof (PackageDetailsView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PackageDetailsViewModel) value; }
        }
    }
}
