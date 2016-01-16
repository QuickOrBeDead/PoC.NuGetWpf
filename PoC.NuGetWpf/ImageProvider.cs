using System;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;

namespace PoC.NuGetWpf
{
    public class ImageProvider
    {
        public BitmapSource GetIcon(Uri uri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = uri;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
