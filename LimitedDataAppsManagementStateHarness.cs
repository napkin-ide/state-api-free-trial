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

        }
        #endregion

        #region API Methods
        public virtual void Mock(string entApiKey, string host)
        {
            if (State.Applications.IsNullOrEmpty())
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
                    }
                };

                State.DAFApps = new List<DAFApplicationConfiguration>(){
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
        }

        public virtual async Task DeleteDataApp(string entApiKey, string appID)
        {
            var appToDelete = State.Applications.FirstOrDefault(a => a.ID.ToString() == appID);
            
            State.Applications.Remove(appToDelete);
        }

        public virtual async Task LoadAppView(string entApiKey)
        {
            if (State.ActiveApp != null)
            {
                var dafApps = State.DAFApps;

                State.ActiveDAFApp = State.DAFApps?.FirstOrDefault(da => da.ApplicationID == State.ActiveApp.ID)?.JSONConvert<DAFApplicationConfiguration>();

                State.ActiveDAFAPIs = null;

            }
            else
                State.ActiveDAFApp = null;
        }

        public virtual async Task SaveDAFApp(string entApiKey, DAFApplicationConfiguration dafApp)
        {
            if (State.ActiveApp != null)
            {
                if (State.AppType != DAFAppTypes.API)
                {
                    if (dafApp.Metadata.ContainsKey("APIRoot"))
                        dafApp.Metadata.Remove("APIRoot");

                    if (dafApp.Metadata.ContainsKey("InboundPath"))
                        dafApp.Metadata.Remove("InboundPath");

                    if (dafApp.Metadata.ContainsKey("Methods"))
                        dafApp.Metadata.Remove("Methods");

                    if (dafApp.Metadata.ContainsKey("Security"))
                        dafApp.Metadata.Remove("Security");
                }

                if (State.AppType != DAFAppTypes.Redirect)
                {
                    if (dafApp.Metadata.ContainsKey("Redirect"))
                        dafApp.Metadata.Remove("Redirect");
                }

                if (State.AppType != DAFAppTypes.View)
                {
                    if (dafApp.Metadata.ContainsKey("BaseHref"))
                        dafApp.Metadata.Remove("BaseHref");

                    if (dafApp.Metadata.ContainsKey("NPMPackage"))
                        dafApp.Metadata.Remove("NPMPackage");

                    if (dafApp.Metadata.ContainsKey("PackageVersion"))
                        dafApp.Metadata.Remove("PackageVersion");
                }

                State.ActiveDAFApp = dafApp;
            }

            await SetActiveApp(entApiKey, State.ActiveApp);
        }


        public virtual async Task SaveDataApp(string entApiKey, string host,
            Application app)
        {
            State.Applications = State.Applications.Where(a => a.ID != app.ID).ToList();

            State.Applications.Add(app);

            await SetActiveApp(entApiKey, app);
        }

        public virtual async Task SetActiveApp(string entApiKey, Application app)
        {
            //await ToggleAddNew();

            State.ActiveApp = app;

            await LoadAppView(entApiKey);
        }

        public virtual async Task ToggleAddNew()
        {
            State.ActiveApp = null;

            if(State.AddingApp == true){
                State.AddingApp = false;
            }
            
            else{
                State.AddingApp = true;
            }

        }
        #endregion
    }
}
