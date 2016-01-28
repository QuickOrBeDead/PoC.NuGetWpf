using System;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public partial class FirstModalView : UserControl, IViewFor<FirstModalViewModel>
    {
        public FirstModalView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.BindCommand(ViewModel, vm => vm.Close, v => v.Close));
            });
        }

        public FirstModalViewModel ViewModel
        {
            get { return (FirstModalViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof (FirstModalViewModel), typeof (FirstModalView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FirstModalViewModel) value; }
        }
    }
}
