using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Rocklan.WrikeUpdater
{
    public class CommitMessageLogic
    {
        private static readonly string _commitParserRegEx = ConfigurationManager.AppSettings["CommitParserRegEx"];

        public static int? GetWrikeJobID(string commitMessage)
        {
            // commit log will look something like: "we did something wrike 12314"

            Match m = Regex.Match(commitMessage, _commitParserRegEx);

            if (m.Success)
            {
                string jobId = m.Groups[1].Value;
                int intJobId;

                if (Int32.TryParse(jobId, out intJobId))
                {
                    return intJobId;
                }
            }

            return null;
        }
    }
}