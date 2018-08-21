using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.WebServiceClient;
using Microsoft.Xrm.Tooling.Connector;
using Scantegra.Core.Entities;
using Scantegra.Core.Functions.Common;

namespace Scantegra.Core.Functions.AlertCreated
{
    public static class AlertCreated
    {
        [FunctionName("AlertCreated")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("AlertCreated Function Triggered Via Webhook.");

            // Get the remote execution context
            string jsonContext = await req.Content.ReadAsStringAsync();
            log.Info("Read context: " + jsonContext);

            jsonContext = FormatJson(jsonContext);
            log.Info("Formatted JSON Context string: " + jsonContext);

            Microsoft.Xrm.Sdk.RemoteExecutionContext context = DeserializeJsonString<Microsoft.Xrm.Sdk.RemoteExecutionContext>(jsonContext);

            //var context = await req.Content.ReadAsAsync<RemoteExecutionContext>();

            // Ensure this function was called on the create message of the scan_alert entity
            if (context.MessageName != "Create" && context.PrimaryEntityName != "scan_alert")
            {
                var err = "AlertCreated Context Execution Is Invalid";
                log.Error(err);
                return req.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            var alert = context.PostEntityImages.FirstOrDefault().Value.ToEntity<scan_alert>();
            if (alert == null)
            {
                var err = "Invalid Alert Post Entity Image";
                log.Error(err);
                return req.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            log.Info("Obtained Valid Context");
            log.Info("Processing Alert " + alert.Id.ToString());

            string orgUrl = ConfigurationManager.AppSettings["ScantegraOrgUrl"];
            Uri orgUri = new Uri(orgUrl);

            log.Info(orgUri.ToString());
            //var proxyClient = new OrganizationWebProxyClient(orgUri, true);
            CrmServiceClient.AuthOverrideHook = new AuthHook();
            //CrmServiceClient test = new CrmServiceClient()
            CrmServiceClient crmSvc = new CrmServiceClient(orgUri, true);
            if (crmSvc.IsReady)
            {
                log.Info("Connected To Dynamics 365");
            }
            //crmSvc.OrganizationWebProxyClient.EnableProxyTypes();
            
            var scantegraContext = new ScantegraServiceContext(crmSvc.OrganizationWebProxyClient);


            // Pull the alert to get the full entity
            alert = scantegraContext.scan_alertSet.Where(a => a.Id == alert.Id).FirstOrDefault();            
            

            // Pull the alert notification group from the rule
            var alertNotificationGroup = scantegraContext.scan_alertnotificationgroupSet.Where(ang => ang.Id == alert.scan_AlertNotificationGroup.Id).FirstOrDefault();
            log.Info("ANG ID " + alertNotificationGroup.Id.ToString());

            // Pull the alert notification actions from the intersection table
            var alertNotificationActionAssociations = scantegraContext.scan_alertgroupalertnotificationassociationSet.Where(a => a.scan_AlertNotificationGroup.Id == alertNotificationGroup.Id).ToList();

            // Iterate through the alert notification actions and process accordingly
            foreach (var association in alertNotificationActionAssociations)
            {
                var alertNotificationAction = scantegraContext.scan_alertnotificationactionSet.Where(aa => aa.Id == association.scan_AlertNotificationAction.Id).FirstOrDefault();
                log.Info("Processing action " + alertNotificationAction.Id.ToString());
                switch (alertNotificationAction.scan_NotificationActionType.Value)
                {
                    case (int)scan_AlertNotificationActionType.PostToMicrosoftTeams:
                        var client = new TeamsClient(alertNotificationAction.scan_TeamsWebhookUrl);
                        var card = new AlertCard()
                        {
                            Title = alert.scan_name,
                            Text = $"{alert.FormattedValues["scan_alertseverity"]} - {alert.scan_name}"
                        };
                        client.PostMessage(card);
                        break;
                }

            }

            return req.CreateResponse(HttpStatusCode.OK);
            
        }

        /// <summary>
        /// Function to deserialize JSON string using DataContractJsonSerializer
        /// </summary>
        /// <typeparam name="RemoteContextType">RemoteContextType Generic Type</typeparam>
        /// <param name="jsonString">string jsonString</param>
        /// <returns>Generic RemoteContextType object</returns>
        public static RemoteContextType DeserializeJsonString<RemoteContextType>(string jsonString)
        {
            //create an instance of generic type object
            RemoteContextType obj = Activator.CreateInstance<RemoteContextType>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString));
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            obj = (RemoteContextType)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }

        /// <summary>
        /// Function to convert the unformatted Json string to formatted Json string
        /// </summary>
        /// <param name="unformattedJson"></param>
        /// <returns>string formattedJsonString</returns>
        public static string FormatJson(string unformattedJson)
        {
            string formattedJson = string.Empty;
            try
            {
                formattedJson = unformattedJson.Trim('"');
                formattedJson = System.Text.RegularExpressions.Regex.Unescape(formattedJson);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return formattedJson;
        }
    }
}
