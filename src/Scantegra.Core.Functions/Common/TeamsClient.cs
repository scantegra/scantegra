using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Scantegra.Core.Functions.Common
{
    public class TeamsClient
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();

        public TeamsClient(string webhookUrl)
        {
            _uri = new Uri(webhookUrl);

        }

        public void PostMessage(string text)
        {
            var teamsPayload = new AlertCard()
            {
                Text = text
            };

            PostMessage(teamsPayload);
        }

        public void PostMessage(AlertCard payload)
        {
            var payloadJson = _encoding.GetBytes(JsonConvert.SerializeObject(payload));                
            using (WebClient client = new WebClient())
            {
                var response = client.UploadData(_uri, "POST", payloadJson);

                //The response text is usually "ok"
                string responseText = _encoding.GetString(response);
            }
        }        
    }
    //This class serializes into the Json payload required by Teams Incoming WebHooks
    public class AlertCard
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
