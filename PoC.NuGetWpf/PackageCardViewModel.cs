using System;
using NuGet;
using ReactiveUI;

namespace PoC.NuGetWpf
{
    public class InstalledPackageCardViewModel : PackageCardViewModel
    {
        public string InstalledVersion { get; }

        public InstalledPackageCardViewModel(IPackage package) : base(package)
        {
            InstalledVersion = new Version(package.Version.Version.Major, Math.Min(0, package.Version.Version.Minor)).ToString();
        }
    }

    public class GalleryPackageCardViewModel : PackageCardViewModel
    {
        public GalleryPackageCardViewModel(IPackage package) : base(package)
        {
        }
    }

    public class PackageCardViewModel : ReactiveObject
    {
        public string LatestVersion { get; }
        public string DownloadCount { get; }
        public string PublishedDate { get; }
        public string Authors { get; }
        public string Title { get; }
        public string Description { get; }

        public PackageCardViewModel(IPackage package)
        {
            Title = package.Title;
            Description = package.Description;
            Authors = String.Join(", ", package.Authors);
            PublishedDate = package.Published.GetValueOrDefault(DateTimeOffset.Now).LocalDateTime.ToShortDateString();
            DownloadCount = String.Format("{0:N0}", package.DownloadCount);
            LatestVersion = package.Version.ToString();
        }
    }
}
