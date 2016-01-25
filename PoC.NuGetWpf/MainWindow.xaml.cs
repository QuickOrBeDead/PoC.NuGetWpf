﻿using System.Windows;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            this.WhenActivated(async d =>
            {
                d(this.Bind(ViewModel, vm => vm.Filter, v => v.Filter.Text));
                d(this.BindCommand(ViewModel, vm => vm.Load, v => v.Search));
                d(this.OneWayBind(ViewModel, vm => vm.Packages, v => v.Packages.ItemsSource));
                d(this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.IsBusy.Visibility));
                d(this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.Packages.Visibility, b => b ? Visibility.Collapsed : Visibility.Visible));

                d(this.BindCommand(ViewModel, vm => vm.Next, v => v.Next));
                d(this.BindCommand(ViewModel, vm => vm.Previous, v => v.Previous));

                await ViewModel.Load.ExecuteAsyncTask();
            });

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
