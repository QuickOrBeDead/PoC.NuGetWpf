using System;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public class FirstModalViewModel : ReactiveModal
    {
        public FirstModalViewModel()
        {
            Close = ReactiveCommand.Create();
            Close.Subscribe(_ => NotifyClose());

            Message = "Some default message.";
        }

        public ReactiveCommand<object> Close { get; }

        string _message;
        public string Message
        {
            get { return _message; }
            set { this.RaiseAndSetIfChanged(ref _message, value); }
        }
    }
}