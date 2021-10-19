namespace Sparky.Templates.Entities
{
    internal sealed class ApplicationEntity
    {
        public string Name { get; set; }
        public string ExecutableFilePath { get; set; }
        public bool IsSelected { get; set; }
        public bool OnFullScreen { get; set; }
        public string DisplayedText => $"{Name} {(OnFullScreen ? "[FullScreen]" : "")}";
    }
}