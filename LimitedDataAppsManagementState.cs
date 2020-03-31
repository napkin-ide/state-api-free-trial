using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Fathym;
using LCU.Graphs.Registry.Enterprises.Apps;
using LCU.Presentation.State.ReqRes;
using LCU.StateAPI.Utilities;
using LCU.StateAPI;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial
{
    [Serializable]
    [DataContract]
    public class LimitedDataAppsManagementState
    {
        #region Constants
        public const string HUB_NAME = "limiteddataappsmanagement";
        #endregion

        [DataMember]
        public virtual List<Application> Applications { get; set; }
        [DataMember]
        public virtual bool Loading { get; set; }

        [DataMember]
        public virtual string OrganizationLookup { get; set; }

        [DataMember]
        public virtual Status Status { get; set; }
    }
}
