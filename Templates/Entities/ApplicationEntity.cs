#region

#endregion

using System;
using Light.WinApi;

namespace Light.Templates.Entities
{
    public class ApplicationEntity
    {
        public string Name { get; set; }
        public string ExecutableFilePath { get; set; }
        public bool IsSelected { get; set; }
        public bool OnFullScreen { get; set; }
        public string DisplayedText => $"{Name} {(OnFullScreen ? "[FullScreen]" : "")}";
    }
}