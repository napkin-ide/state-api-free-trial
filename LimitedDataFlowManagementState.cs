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
using LCU.Graphs.Registry.Enterprises.DataFlows;
using LCU.Presentation.State.ReqRes;
using LCU.StateAPI.Utilities;
using LCU.StateAPI;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Collections.Generic;
using LCU.Personas.Enterprises;

namespace LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial
{
    [Serializable]
    [DataContract]
    public class LimitedDataFlowManagementState
    {
        [DataMember]
        public virtual DataFlow ActiveDataFlow { get; set; }

        [DataMember]
        public bool AllowCreationModules { get; set; }
        
        [DataMember]
        public virtual List<DataFlow> DataFlows { get; set; }

        [DataMember]
        public virtual List<DataFlow> EmulatedDataFlows { get; set; }

        [DataMember]
        public virtual string EnvironmentLookup { get; set; }

        [DataMember]
        public virtual List<InfrastructureDetails> InfrastructureDetails { get; set; }

        [DataMember]
        public virtual bool IsCreating { get; set; }

        [DataMember]
        public virtual bool Loading { get; set; }

        [DataMember]
        public virtual List<ModulePack> ModulePacks { get; set; }
        
        [DataMember]
        public virtual List<ModuleOption> ModuleOptions { get; set; }
        
        [DataMember]
        public virtual List<ModuleDisplay> ModuleDisplays { get; set; }

        [DataMember]
        public virtual string OrganizationLookup { get; set; }

        [DataMember]
        public virtual Status Status { get; set; }
    }
}
