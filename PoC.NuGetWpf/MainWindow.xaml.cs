using System.Windows;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, vm => vm.Load, v => v.Search);
            this.OneWayBind(ViewModel, vm => vm.Packages, v => v.Packages.ItemsSource);
            this.Bind(ViewModel, vm => vm.Filter, v => v.Filter.Text);

            ViewModel = new MainWindowViewModel();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainWindowViewModel)value; }
        }

        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MainWindowViewModel), typeof(MainWindow));
    }
}
