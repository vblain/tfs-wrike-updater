namespace Rocklan.WrikeUpdater
{
    public class WrikeResponseDTO
    {
        public string kind { get; set; }
        public TaskInfo[] data { get; set; }
    }
    public class TaskInfo
    {
        public string id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public CustomField[] customFields { get; set; }
    }

    public class CustomField
    {
        public string id { get; set; }
        public string value { get; set; }
    }
}