using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Fathym;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.Storage.Blob;
using LCU.StateAPI.Utilities;
using LCU.Personas.Client.Applications;
using LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial.State;

namespace LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial.DataApps
{
    [Serializable]
    [DataContract]
    public class DeleteDataAppRequest
    {
        [DataMember]
        public virtual string AppID { get; set; }

    }

    public class DeleteDataApp
    {
        [FunctionName("DeleteDataApp")]
        public virtual async Task<Status> Run([HttpTrigger] HttpRequest req, ILogger log,
            [SignalR(HubName = LimitedTrialState.HUB_NAME)]IAsyncCollector<SignalRMessage> signalRMessages,
            [Blob("state-api/{headers.lcu-ent-lookup}/{headers.lcu-hub-name}/{headers.x-ms-client-principal-id}/{headers.lcu-state-key}", FileAccess.ReadWrite)] CloudBlockBlob stateBlob)
        {
            return await stateBlob.WithStateHarness<LimitedDataAppsManagementState, DeleteDataAppRequest, LimitedDataAppsManagementStateHarness>(req, signalRMessages, log,
                async (harness, reqData, actReq) =>
            {
                log.LogInformation($"Deleting Data Flow: {reqData.AppID}");
                
                var stateDetails = StateUtils.LoadStateDetails(req);

                await harness.DeleteDataApp(stateDetails.EnterpriseLookup, reqData.AppID);

                return Status.Success;
            });
        }
    }
}
