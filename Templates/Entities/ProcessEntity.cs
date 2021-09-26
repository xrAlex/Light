#region

#endregion

namespace Light.Templates.Entities
{
    public class ProcessEntity
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool OnFullScreen { get; set; }
        public string DisplayedText => $"{Name} {(OnFullScreen ? "[FullScreen]" : "")}";
    }
}