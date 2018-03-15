using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Rocklan.WrikeUpdater
{
    public class WrikeClientLogic
    {
        private readonly string _wrikeBaseUrl = ConfigurationManager.AppSettings["Wrike:BaseUrl"];
        private readonly string _oAuthToken = ConfigurationManager.AppSettings["Wrike:oAuth"];

        private readonly string _commitCustomFieldID = ConfigurationManager.AppSettings["Wrike:CommitCustomFieldID"];
        private readonly string _changesetCustomFieldID = ConfigurationManager.AppSettings["Wrike:ChangesetCustomFieldID"];

        private readonly int _apiTimeout =
            Int32.Parse(ConfigurationManager.AppSettings["Wrike:ApiTimeoutSeconds"]) * 1000;

       
     

        public void UpdateCommitIDs(int jobId, string commitId, string commitMessage, string comment, string author)
        {
            try
            {
                string wrikeTaskID = updateCustomField(jobId, _commitCustomFieldID, commitId);

                AddComment(wrikeTaskID, comment);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        public void UpdateChangeSets(int jobId, string changesetId, string commitMessage, string comment, string author)
        {
            try
            {
                string wrikeTaskID = updateCustomField(jobId, _changesetCustomFieldID, changesetId);

                AddComment(wrikeTaskID, comment);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }


        private string updateCustomField(int jobId, string customFieldId, string value )
        {
            var wrikeMetaData = getWrikeData(jobId);

            if (wrikeMetaData == null ||
                wrikeMetaData.data == null ||
                wrikeMetaData.data.Length == 0 ||
                wrikeMetaData.data[0] == null)
            {
                return null; 
            }

            var wrikeTaskData = wrikeMetaData.data[0];

            // check if there's any existing commit Ids
            string existingCommitIds = wrikeTaskData.customFields.FirstOrDefault(x => x.id == customFieldId)?.value;

            // generate the new value for the CommitIds attribute
            string newCommitIds = (string.IsNullOrWhiteSpace(existingCommitIds) ? value : value + "," + existingCommitIds);

            // update it in wrike:
            SetCustomField(wrikeTaskData.id, customFieldId, newCommitIds);

            return wrikeTaskData.id;
        }

        public WrikeResponseDTO GetAllCustomFields()
        {
            string WEBSERVICE_URL = _wrikeBaseUrl + "/customfields";
            return getData<WrikeResponseDTO>(WEBSERVICE_URL);
        }


        private WrikeResponseDTO getWrikeData(int jobId)
        {
            string WEBSERVICE_URL = _wrikeBaseUrl +
                "/tasks?permalink=https://www.wrike.com/open.htm?id=" + jobId + "&fields=[\"customFields\"]";

            return getData<WrikeResponseDTO>(WEBSERVICE_URL);
        }



        private void SetCustomField(string TaskID, string CustomFieldID, string commitId)
        {
            string endpointUrl = _wrikeBaseUrl + "/tasks/" + TaskID;
            string updateTask = "customFields=[{\"id\": \"" + CustomFieldID + "\", \"value\": \"" + commitId + "\"}]";

            writeData(endpointUrl, "PUT", updateTask);
        }


        private void AddComment(string TaskID, string comment)
        {
            if (TaskID == null || comment == null) return;
            
            string endpointUrl = _wrikeBaseUrl + "/tasks/" + TaskID + "/comments";
            string escapedComment = comment.Replace("&amp;", "%26");
            string addCommentInstruction = "plainText=false&text=" + escapedComment;

            writeData(endpointUrl, "POST", addCommentInstruction);
        }


        private void writeData(string url, string method, string data)
        {
            var webRequest = System.Net.WebRequest.Create(url);
            if (webRequest != null)
            {
                byte[] dataAsBytes = stringAsAsciiBytes(data);

                webRequest.Method = method;
                webRequest.Timeout = _apiTimeout;
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = dataAsBytes.Length;
                webRequest.Headers.Add("Authorization", "bearer " + _oAuthToken);

                using (var requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(dataAsBytes, 0, dataAsBytes.Length);
                }
            }
        }

        private byte[] stringAsAsciiBytes(String comment)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetBytes(comment);
        }

        private T getData<T>(string url)
        {
            var webRequest = System.Net.WebRequest.Create(url);

            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = _apiTimeout;
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add("Authorization", "bearer " + _oAuthToken);

                using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        Console.WriteLine(String.Format("Response: {0}", jsonResponse));

                        return JsonConvert.DeserializeObject<T>(jsonResponse);
                    }
                }
            }

            return default(T);
        }
    }

  

}