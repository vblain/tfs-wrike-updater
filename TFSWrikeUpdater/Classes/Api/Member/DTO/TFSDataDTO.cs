namespace Rocklan.WrikeUpdater
{
    public class TFSDataDTO
    {
        public string subscriptionId { get; set; }
        public string notificationId { get; set; }
        public TFSMessage message { get; set; }
        public TFSDetailedMessage detailedMessage { get; set; }
        public TFSResource resource { get; set; }
    }
    public class TFSMessage
    {
        public string text { get; set; }
    }
    public class TFSDetailedMessage
    {
        public string text { get; set; }
        public string html { get; set; }
    }
    public class TFSResource
    {
        public TFSCommit[] commits { get; set; }
        public string changesetId { get; set; }
        public string comment { get; set; }
        public TFSChangeSetAuthor author { get; set; }
    }
    public class TFSCommit
    {
        public string commitId { get; set; }
        public string comment { get; set; }
        public string url { get; set; }
        public TFSCommitAuthor author { get; set; }
    }
    public class TFSCommitAuthor
    {
        public string name { get; set; }
    }
    public class TFSChangeSetAuthor
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
    }
}

