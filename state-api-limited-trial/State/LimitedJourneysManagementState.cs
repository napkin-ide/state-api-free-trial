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

namespace LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial.State
{
    [Serializable]
    [DataContract]
    public class LimitedJourneysManagementState
    {
        [DataMember]
        public virtual bool Loading { get; set; }

        [DataMember]
        public virtual List<JourneyOption> Journeys { get; set; }
    }

    [DataContract]
    public class JourneyOption
    {
        [DataMember]
        public virtual bool Active { get; set; }

        [DataMember]
        public virtual bool ComingSoon { get; set; }

        [DataMember]
        public virtual string ContentURL { get; set; }

        [DataMember]
        public virtual string Description { get; set; }

        [DataMember]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual JourneyContentTypes ContentType { get; set; }

        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        [JsonProperty("Roles", ItemConverterType = typeof(StringEnumConverter))]
        public virtual List<JourneyRoleTypes> Roles { get; set; }

        [DataMember]
        public virtual List<string> Uses { get; set; }
    }

    [DataContract]
    public enum JourneyContentTypes
    {
        [EnumMember]
        Image,

        [EnumMember]
        Video
    }

    [DataContract]
    public enum JourneyRoleTypes
    {
        [EnumMember]
        Administrator,

        [EnumMember]
        Designer,

        [EnumMember]
        Developer
    }
}
