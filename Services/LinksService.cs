using System.Diagnostics;
using Sparky.Services.Interfaces;

namespace Sparky.Services
{
    internal class LinksService : ILinksService
    {
        public void OpenLink(string url)
        {
            var startInfo = new ProcessStartInfo(fileName: url);
            using var openLink = Process.Start(startInfo);
        }
    }
}
