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
using Microsoft.Azure.Storage.Blob;
using System.Runtime.Serialization;
using Fathym.API;
using System.Collections.Generic;
using System.Linq;
using LCU.Personas.Client.Applications;
using LCU.StateAPI.Utilities;
using System.Security.Claims;
using LCU.Personas.Client.Enterprises;
using LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial.State;

namespace LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial.Host
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
            [Blob("state-api/{headers.lcu-ent-lookup}/{headers.lcu-hub-name}/{headers.x-ms-client-principal-id}/{headers.lcu-state-key}", FileAccess.ReadWrite)] CloudBlockBlob stateBlob)
        {
            var stateDetails = StateUtils.LoadStateDetails(req);

            if (stateDetails.StateKey == "data-apps")
                return await stateBlob.WithStateHarness<LimitedDataAppsManagementState, RefreshRequest, LimitedDataAppsManagementStateHarness>(req, signalRMessages, log,
                    async (harness, refreshReq, actReq) =>
                {
                    log.LogInformation($"Refreshing data applications state");

                    return await refreshDataApps(harness, log, stateDetails);
                });
            else if (stateDetails.StateKey == "data-flow")
                return await stateBlob.WithStateHarness<LimitedDataFlowManagementState, RefreshRequest, LimitedDataFlowManagementStateHarness>(req, signalRMessages, log,
                    async (harness, refreshReq, actReq) =>
                {
                    log.LogInformation($"Refreshing data flow state");

                    return await refreshDataFlow(harness, log, stateDetails);
                });
            else if (stateDetails.StateKey == "journeys")
                return await stateBlob.WithStateHarness<LimitedJourneysManagementState, RefreshRequest, LimitedJourneysManagementStateHarness>(req, signalRMessages, log,
                    async (harness, refreshReq, actReq) =>
                {
                    log.LogInformation($"Refreshing data flow state");

                    return await refreshJourneys(harness, log, stateDetails);
                });
            else
                throw new Exception("A valid State Key must be provided (data-apps, data-flow).");
        }
        #endregion

        #region Helpers
        protected virtual async Task<Status> refreshDataApps(LimitedDataAppsManagementStateHarness harness, ILogger log, StateDetails stateDetails)
        {
            harness.Mock(stateDetails.EnterpriseLookup, stateDetails.Host);

            return Status.Success;
        }

        protected virtual async Task<Status> refreshDataFlow(LimitedDataFlowManagementStateHarness harness, ILogger log, StateDetails stateDetails)
        {
            await harness.Mock(appMgr, appDev, entMgr, stateDetails.EnterpriseLookup, stateDetails.Host);

            return Status.Success;
        }

        protected virtual async Task<Status> refreshJourneys(LimitedJourneysManagementStateHarness harness, ILogger log, StateDetails stateDetails)
        {
            await harness.Refresh();

            return Status.Success;
        }
        #endregion
    }
}
