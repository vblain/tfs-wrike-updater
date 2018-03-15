using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Rocklan.WrikeUpdater
{
    public class TFSController : ApiController
    {
        private WrikeClientLogic wrikeClient = new WrikeClientLogic();
        private TFSLogic tfsLogic = new TFSLogic();

        [HttpGet]
        [Route("~/v1/hello-world")]
        public HttpResponseMessage HelloWorld()
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, "Hello World");
        }

        [HttpGet]
        [Route("~/v1/custom-fields")]
        public HttpResponseMessage customFields()
        {
            var fields = wrikeClient.GetAllCustomFields();
            return this.Request.CreateResponse(HttpStatusCode.OK, fields.data);
        }


        [HttpPost]
        [Route("~/v1/code-pushed-hook")]
        public HttpResponseMessage codePushedHook([FromBody]TFSDataDTO Content)
        {
            var tfsData = tfsLogic.getTFSDataFromPush(Content);

            if (tfsData != null)
                wrikeClient.UpdateCommitIDs(
                    tfsData.WrikeJobID, tfsData.CommitID, tfsData.CommitMessage, tfsData.FullMessageHtml, tfsData.CommitAuthor);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("~/v1/code-checkin-hook")]
        public HttpResponseMessage codeCheckinHook([FromBody]TFSDataDTO Content)
        {
            var tfsData = tfsLogic.getTFSDataFromCheckin(Content);

            if (tfsData != null)
                wrikeClient.UpdateChangeSets(
                    tfsData.WrikeJobID, tfsData.ChangesetID, tfsData.CommitMessage, tfsData.FullMessageHtml, tfsData.CommitAuthor);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

    }
    
}
