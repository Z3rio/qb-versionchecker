namespace VersionChecker.Models
{
    public class ResourceModel
    {
        public class Versions
        {
            public string? local { get; set; }
            public string? current { get; set; }
        }

        public class Resource
        {
            public string? name { get; set; }

            public string? currentVersion { get; set; }
            public Versions? versions { get; set; }
        }
    }
}
