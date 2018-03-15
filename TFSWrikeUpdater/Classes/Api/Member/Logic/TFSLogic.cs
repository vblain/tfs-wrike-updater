using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Rocklan.WrikeUpdater
{
    public class TFSData
    {
        public string CommitID { get; set; }
        public string ChangesetID { get; set; }
        public string CommitMessage { get; set; }
        public string CommitAuthor { get; set; }
        public string FullMessageHtml { get; set; }
        public int WrikeJobID { get; set; }
    }

    public class TFSLogic
    {
        internal TFSData getTFSDataFromPush(TFSDataDTO content)
        {
            if (content?.resource?.commits == null) return null;
            if (content.resource.commits.Length == 0) return null;
            if (content.resource.commits[0] == null) return null;
            if (content.resource.commits[0].comment == null) return null;

            var commit = content.resource.commits[0];

            if (!commit.comment.ToLower().Contains("wrike")) return null;

            int? wrikeJobID = CommitMessageLogic.GetWrikeJobID(commit.comment);

            if (!wrikeJobID.HasValue) return null;

            return new TFSData
            {
                CommitID = commit.commitId,
                CommitAuthor = commit.author?.name,
                CommitMessage = commit.comment,
                FullMessageHtml = content.detailedMessage.html,
                WrikeJobID = wrikeJobID.Value,
            };

        }



        internal TFSData getTFSDataFromCheckin(TFSDataDTO content)
        {
            if (content?.resource?.changesetId == null) return null;
            if (content?.resource?.author == null) return null;
            if (content?.resource?.comment == null) return null;

            int? wrikeJobID = CommitMessageLogic.GetWrikeJobID(content.resource.comment);

            if (!wrikeJobID.HasValue) return null;

            return new TFSData
            {
                ChangesetID = content.resource.changesetId,
                CommitAuthor = content.resource.author.displayName,
                CommitMessage = content.resource.comment,
                FullMessageHtml = content.detailedMessage.html,
                WrikeJobID = wrikeJobID.Value,
            };

        }
    }

}
