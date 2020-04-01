using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Fathym;
using LCU.Presentation.State.ReqRes;
using LCU.StateAPI.Utilities;
using LCU.StateAPI;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Collections.Generic;
using LCU.Personas.Client.Enterprises;
using LCU.Personas.Client.DevOps;
using LCU.Personas.Enterprises;
using LCU.Personas.Client.Applications;
using Fathym.API;
using LCU.Graphs.Registry.Enterprises.Apps;

namespace LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial
{
    public class LimitedDataAppsStateHarness : LCUStateHarness<LimitedDataAppsManagementState>
    {
        #region Fields
        protected readonly List<DAFApplicationConfiguration> dafApps;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public LimitedDataAppsStateHarness(LimitedDataAppsManagementState state)
            : base(state ?? new LimitedDataAppsManagementState())
        { 
            dafApps = new List<DAFApplicationConfiguration>(){
                new DAFViewConfiguration(){
                    ApplicationID = new Guid("00000000-0000-0000-0000-000000000001"),
                    ID = new Guid("a0000000-0000-0000-0000-000000000001"),
                    Lookup = null,
                    Priority = 500,
                    BaseHref = "/freeboard/",
                    NPMPackage = "@semanticjs/freeboard",
                    PackageVersion = "0.0.6",
                },

                new DAFViewConfiguration(){
                    ApplicationID = new Guid("00000000-0000-0000-0000-000000000002"),
                    ID = new Guid("a0000000-0000-0000-0000-000000000002"),
                    Lookup = null,
                    Priority = 500,
                    BaseHref = "/forecast/",
                    NPMPackage = "@habistack/lcu-fathym-forecast-demo",
                    PackageVersion = "1.1.1",
                }
            };
        }
        #endregion

        #region API Methods
        public virtual void Mock(string entApiKey, string host)
        {
            State.Applications = new List<Application>{
                new Application(){
                    Container = "lcu-data-apps",
                    EnterpriseAPIKey = entApiKey,
                    Hosts = new List<string>{
                        host,
                    },
                    Name = "Freeboard",
                    PathRegex = "/freeboard*",
                    Priority = 10000,
                    ID = new Guid("00000000-0000-0000-0000-000000000001")
                },
                new Application(){
                    Container = "lcu-data-apps",
                    EnterpriseAPIKey = entApiKey,
                    Hosts = new List<string>{
                        host,
                    },
                    Name = "Fathym Forecast",
                    PathRegex = "/forecast*",
                    Priority = 10500,
                    ID = new Guid("00000000-0000-0000-0000-000000000002")
                },  
                new Application(){
                    Container = "lcu-data-apps",
                    EnterpriseAPIKey = entApiKey,
                    Hosts = new List<string>{
                        host,
                    },
                    Name = "Freeboard",
                    PathRegex = "/freeboard*",
                    Priority = 10000,
                    ID = new Guid("00000000-0000-0000-0000-000000000001")
                } 
            };

            State.AppType = DAFAppTypes.View;
            
            State.VersionLookups = new Dictionary<string, List<string>>();

            State.VersionLookups["@habistack/lcu-fathym-forecast-demo"] = new List<string>(){
                "latest",
                "1.1.1",
                "0.9.163-fathym-hackathon-lcu-charts-utilization"
            };

            State.VersionLookups["@semanticjs/freeboard"] = new List<string>(){
                "latest",
                "0.0.6",
                "0.0.5",
            };

        }
        #endregion
    }
}
