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
using LCU.Personas.Client.Applications;
using Fathym;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.Storage.Blob;
using LCU.StateAPI.Utilities;
using LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial.State;

namespace LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial.DataApps
{
    [Serializable]
    [DataContract]
    public class SaveDataAppRequest
    {
        [DataMember]
        public virtual Application App { get; set; }
    }

    public class SaveDataApp
    {
        [FunctionName("SaveDataApp")]
        public virtual async Task<Status> Run([HttpTrigger] HttpRequest req, ILogger log,
            [SignalR(HubName = LimitedTrialState.HUB_NAME)]IAsyncCollector<SignalRMessage> signalRMessages,
            [Blob("state-api/{headers.lcu-ent-lookup}/{headers.lcu-hub-name}/{headers.x-ms-client-principal-id}/{headers.lcu-state-key}", FileAccess.ReadWrite)] CloudBlockBlob stateBlob)
        {
            return await stateBlob.WithStateHarness<LimitedDataAppsManagementState, SaveDataAppRequest, LimitedDataAppsManagementStateHarness>(req, signalRMessages, log,
                async (harness, reqData, actReq) =>
            {
                var stateDetails = StateUtils.LoadStateDetails(req);

                log.LogInformation($"Saving Data App: {reqData.App.Name}");

                await harness.SaveDataApp(stateDetails.EnterpriseLookup, stateDetails.Host, reqData.App);

                return Status.Success;
            });
        }
    }
}
