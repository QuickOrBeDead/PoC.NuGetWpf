using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;
using Splat;

namespace PoC.NuGetWpf
{
    public class AppBootstrapper
    {
        public AppBootstrapper(IMutableDependencyResolver dependencyResolver)
        {
            InitializeLogging();
            RegisterExceptionHandlers();
            RegisterTypesInContainer(dependencyResolver);
        }

        private void InitializeLogging()
        {
            // Set the main thread's name to make it clear in the logs.
            Thread.CurrentThread.Name = "Main";

            // Sets my logger to the console, which goes to the debug output.
            Log.InitializeWith<ConsoleLog>();

            // Show a banner to easily pick out where new instances start
            // in the log file. Plus it just looks cool.
            this.Log().Info(@" _______       _______             ");
            this.Log().Info(@"(_______)     (_______)        _   ");
            this.Log().Info(@" _     _ _   _ _   ___ _____ _| |_ ");
            this.Log().Info(@"| |   | | | | | | (_  | ___ (_   _)");
            this.Log().Info(@"| |   | | |_| | |___) | ____| | |_ ");
            this.Log().Info(@"|_|   |_|____/ \_____/|_____)  \__)");
            this.Log().Info(@"");

            // Show some basic information about the assembly.
            var assemblyLocation = GetAssemblyDirectory();
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            var productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            var principal = WindowsIdentity.GetCurrent().IfNotNull(x => x.Name, "[Unknown]");

            this.Log().Info("Assembly location: {0}", assemblyLocation);
            this.Log().Info(" Assembly version: {0}", assemblyVersion);
            this.Log().Info("     File version: {0}", fileVersion);
            this.Log().Info("  Product version: {0}", productVersion);
            this.Log().Info("       Running as: {0}", principal);
        }

        private static string GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        private void RegisterExceptionHandlers()
        {
            // If we are running this app in Visual Studio then do not handle
            // any of the unhandled exceptions. Let Visual Studio catch them.
            if (AppDomain.CurrentDomain.FriendlyName.EndsWith("vshost.exe"))
            {
                return;
            }

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var loggerTarget = sender ?? this;
                var exception = args.ExceptionObject as Exception;
                loggerTarget.Log().Fatal(exception, "Unhandled exception in the app domain.");
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                var loggerTarget = sender ?? this;
                loggerTarget.Log().Fatal(args.Exception, "Unhandled exception in the task scheduler.");
            };

            Application.Current.DispatcherUnhandledException += (sender, args) =>
            {
                var loggerTarget = sender ?? this;
                loggerTarget.Log().Fatal(args.Exception, "Unhandled exception in the application dispatcher.");
            };
        }

        private void RegisterTypesInContainer(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));

            dependencyResolver.Register(() => new MainWindow(), typeof(IViewFor<MainWindowViewModel>));
            dependencyResolver.Register(() => new GalleryPackageCardView(), typeof(IViewFor<GalleryPackageCardViewModel>));
            dependencyResolver.Register(() => new InstalledPackageCardView(), typeof(IViewFor<InstalledPackageCardViewModel>));
        }
    }
}