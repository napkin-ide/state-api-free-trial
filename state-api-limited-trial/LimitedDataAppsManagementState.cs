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
        [DataMember]
        public virtual Application ActiveApp { get; set; }

        [DataMember]
        public List<DAFAPIConfiguration> ActiveDAFAPIs { get; set; }

        [DataMember]
        public virtual DAFApplicationConfiguration ActiveDAFApp { get; set; }

        [DataMember]
        public virtual bool AddingApp { get; set; }

        [DataMember]
        public virtual List<Application> Applications { get; set; }

        [DataMember]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual DAFAppTypes? AppType { get; set; }

        [DataMember]
        public virtual List<Application> DefaultApps { get; set; }

        [DataMember]
        public virtual Status DefaultAppsEnabled { get; set; }

        [DataMember]
        public virtual List<string> HostOptions { get; set; }

        [DataMember]
        public virtual bool Loading { get; set; }

        [DataMember]
        public virtual Dictionary<string, string> PathLookups { get; set; }

        [DataMember]
        public virtual Dictionary<string,List<string>> VersionLookups { get; set; }

        [DataMember]
        public virtual List<DAFApplicationConfiguration> DAFApps { get; set; }
    }

    [DataContract]
    public enum DAFAppTypes
    {
        [EnumMember]
        View,
        
        [EnumMember]
        API,
        
        [EnumMember]
        Redirect
    }
}
