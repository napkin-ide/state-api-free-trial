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
using LCU.Graphs.Registry.Enterprises.Apps;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.WindowsAzure.Storage.Blob;
using Fathym;
using LCU.StateAPI.Utilities;
using LCU.Personas.Client.Applications;

namespace LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial
{
    [Serializable]
    [DataContract]
    public class SaveDAFAppRequest
    {
        [DataMember]
        public virtual DAFApplicationConfiguration DAFApp { get; set; }
    }

    public class SaveDAFApp
    {

        [FunctionName("SaveDAFApp")]
        public virtual async Task<Status> Run([HttpTrigger] HttpRequest req, ILogger log,
            [SignalR(HubName = LimitedTrialState.HUB_NAME)]IAsyncCollector<SignalRMessage> signalRMessages,
            [Blob("state-api/{headers.lcu-ent-api-key}/{headers.lcu-hub-name}/{headers.x-ms-client-principal-id}/{headers.lcu-state-key}", FileAccess.ReadWrite)] CloudBlockBlob stateBlob)
        {
            return await stateBlob.WithStateHarness<LimitedDataAppsManagementState, SaveDAFAppRequest, LimitedDataAppsStateHarness>(req, signalRMessages, log,
                async (harness, reqData, actReq) =>
            {
                log.LogInformation($"Saving DAF Application");

                var stateDetails = StateUtils.LoadStateDetails(req);

                await harness.SaveDAFApp(stateDetails.EnterpriseAPIKey, reqData.DAFApp);

                return Status.Success;
            });
        }
    }
}
