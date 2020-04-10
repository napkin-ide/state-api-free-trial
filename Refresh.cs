using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Fathym;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Runtime.Serialization;
using Fathym.API;
using System.Collections.Generic;
using System.Linq;
using LCU.Personas.Client.Applications;
using LCU.StateAPI.Utilities;
using System.Security.Claims;
using LCU.Personas.Client.Enterprises;

namespace LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial
{
    [Serializable]
    [DataContract]
    public class RefreshRequest : BaseRequest
    { }

    public class Refresh
    {
        #region Fields
        protected ApplicationDeveloperClient appDev;

        protected ApplicationManagerClient appMgr;

        protected EnterpriseManagerClient entMgr;

        #endregion

        #region Constructors
        public Refresh(EnterpriseManagerClient entMgr, ApplicationDeveloperClient appDev, ApplicationManagerClient appMgr)
        {
            this.appDev = appDev;

            this.appMgr = appMgr;

            this.entMgr = entMgr;
        }
        #endregion

        #region API Methods
        [FunctionName("Refresh")]
        public virtual async Task<Status> Run([HttpTrigger] HttpRequest req, ILogger log,
            [SignalR(HubName = LimitedTrialState.HUB_NAME)]IAsyncCollector<SignalRMessage> signalRMessages,
            [Blob("state-api/{headers.lcu-ent-api-key}/{headers.lcu-hub-name}/{headers.x-ms-client-principal-id}/{headers.lcu-state-key}", FileAccess.ReadWrite)] CloudBlockBlob stateBlob)
        {
            var stateDetails = StateUtils.LoadStateDetails(req);

            if (stateDetails.StateKey == "data-apps")
                return await stateBlob.WithStateHarness<LimitedDataAppsManagementState, RefreshRequest, LimitedDataAppsStateHarness>(req, signalRMessages, log,
                    async (harness, refreshReq, actReq) =>
                {
                    log.LogInformation($"Refreshing data applications state");

                    return await refreshDataApps(harness, log, stateDetails);
                });
            else if (stateDetails.StateKey == "data-flow")
                return await stateBlob.WithStateHarness<LimitedDataFlowManagementState, RefreshRequest, LimitedDataFlowStateHarness>(req, signalRMessages, log,
                    async (harness, refreshReq, actReq) =>
                {
                    log.LogInformation($"Refreshing data flow state");

                    return await refreshDataFlow(harness, log, stateDetails);
                });
            else
                throw new Exception("A valid State Key must be provided (data-apps, data-flow).");
        }
        #endregion

        #region Helpers
        protected virtual async Task<Status> refreshDataApps(LimitedDataAppsStateHarness harness, ILogger log, StateDetails stateDetails)
        {
            harness.Mock(stateDetails.EnterpriseAPIKey, stateDetails.Host);

            return Status.Success;
        }

        protected virtual async Task<Status> refreshDataFlow(LimitedDataFlowStateHarness harness, ILogger log, StateDetails stateDetails)
        {
            await harness.Mock(appMgr, entMgr, stateDetails.EnterpriseAPIKey, stateDetails.Host);
            
            return Status.Success;
        }
        #endregion
    }
}
