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
    public class LimitedDataAppsManagementStateHarness : LCUStateHarness<LimitedDataAppsManagementState>
    {
        #region Fields
        protected readonly List<DAFApplicationConfiguration> dafApps;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public LimitedDataAppsManagementStateHarness(LimitedDataAppsManagementState state)
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
                        Name = "Hello World",
                        PathRegex = "/helloworld*",
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

                    // new Application(){
                    //     Container = "lcu-data-apps",
                    //     EnterpriseAPIKey = entApiKey,
                    //     Hosts = new List<string>{
                    //         host,
                    //     },
                    //     Name = "LCU Charts",
                    //     PathRegex = "/charts*",
                    //     Priority = 11000,
                    //     ID = new Guid("00000000-0000-0000-0000-000000000003")
                    // },

                    new Application(){
                        Container = "lcu-data-apps",
                        EnterpriseAPIKey = entApiKey,
                        Hosts = new List<string>{
                            host,
                        },
                        Name = "Trial Dashboard",
                        PathRegex = "/dashboard*",
                        Priority = 11500,
                        ID = new Guid("00000000-0000-0000-0000-000000000004")
                    }
                };

                State.DAFApps = new List<DAFApplicationConfiguration>(){
                    new DAFViewConfiguration(){
                        ApplicationID = new Guid("00000000-0000-0000-0000-000000000004"),
                        ID = new Guid("a0000000-0000-0000-0000-000000000004"),
                        Lookup = null,
                        Priority = 500,
                        BaseHref = "/dashboard/",
                        NPMPackage = "@lowcodeunit-dashboards/lcu-fathym-dashboard-getting-started",
                        PackageVersion = "1.1.23-integration",
                    },
                    
                    new DAFViewConfiguration(){
                        ApplicationID = new Guid("00000000-0000-0000-0000-000000000001"),
                        ID = new Guid("a0000000-0000-0000-0000-000000000001"),
                        Lookup = null,
                        Priority = 500,
                        BaseHref = "/helloworld/",
                        NPMPackage = "@fathym-it/hello-world-demo",
                        PackageVersion = "1.3.15",
                    },

                    new DAFViewConfiguration(){
                        ApplicationID = new Guid("00000000-0000-0000-0000-000000000002"),
                        ID = new Guid("a0000000-0000-0000-0000-000000000002"),
                        Lookup = null,
                        Priority = 500,
                        BaseHref = "/forecast/",
                        NPMPackage = "@habistack/lcu-fathym-forecast-demo",
                        PackageVersion = "1.1.1",
                    },
                    

                    // new DAFViewConfiguration(){
                    //     ApplicationID = new Guid("00000000-0000-0000-0000-000000000003"),
                    //     ID = new Guid("a0000000-0000-0000-0000-000000000003"),
                    //     Lookup = null,
                    //     Priority = 500,
                    //     BaseHref = "/charts/",
                    //     NPMPackage = "@lowcodeunit/lcu-charts-demo",
                    //     PackageVersion = "1.7.6",
                    // },
                };
                State.AppType = DAFAppTypes.View;

                State.VersionLookups = new Dictionary<string, List<string>>();

                //Note: Make sure to always have "latest" at the top of these lists, and the actual latest version as the second list entry

                State.VersionLookups["@habistack/lcu-fathym-forecast-demo"] = new List<string>(){
                    "latest",
                    "1.1.1",
                    "0.9.163-fathym-hackathon-lcu-charts-utilization"
                };

                State.VersionLookups["@fathym-it/hello-world-demo"] = new List<string>(){
                    "latest",
                    "1.3.15",
                    "1.2.13-integration",
                    "1.1.1"
                };

                State.VersionLookups["@semanticjs/freeboard"] = new List<string>(){
                    "latest",
                    "0.0.6",
                    "0.0.5",
                };

                State.VersionLookups["@lowcodeunit/lcu-charts-demo"] = new List<string>(){
                    "latest",
                    "1.7.6",
                    "1.7.15-fathym-hackathon",
                    "1.6.1",
                };

                State.VersionLookups["@lowcodeunit-dashboards/lcu-fathym-dashboard-getting-started"] = new List<string>(){
                    "latest",
                    "1.1.23-integration",
                    "1.1.22-integration",
                };

                State.PathLookups = new Dictionary<string, string>();

                State.PathLookups["@fathym-it/hello-world-demo@1.3.15"] = "/helloworld";

                State.PathLookups["@fathym-it/hello-world-demo@1.2.13-integration"] = "/helloworld/1213-integration";

                State.PathLookups["@fathym-it/hello-world-demo@1.1.1"] = "/helloworld/111";

                State.PathLookups["@habistack/lcu-fathym-forecast-demo@1.1.1"] = "/forecast";

                State.PathLookups["@habistack/lcu-fathym-forecast-demo@0.9.163-fathym-hackathon-lcu-charts-utilization"] = "/forecast/charts";

                State.PathLookups["@habistack/lcu-fathym-forecast-demo@0.9.169-bill-precip-adjust"] = "/forecast/precip";

                State.PathLookups["@semanticjs/freeboard@0.0.6"] = "/freeboard";

                State.PathLookups["@semanticjs/freeboard@0.0.5"] = "/freeboard/5";

                State.PathLookups["@lowcodeunit/lcu-charts-demo@1.7.6"] = "/charts";

                State.PathLookups["@lowcodeunit/lcu-charts-demo@1.7.15-fathym-hackathon"] = "/charts/hackathon";

                State.PathLookups["@lowcodeunit/lcu-charts-demo@1.6.1"] = "/charts/161";
            }
        }

        public virtual async Task DeleteDataApp(string entApiKey, string appID)
        {
            var appToDelete = State.Applications.FirstOrDefault(a => a.ID.ToString() == appID);
            
            State.Applications.Remove(appToDelete);

            var item = State.DAFApps.FirstOrDefault(da => da.ApplicationID.ToString() == appID);

            State.DAFApps.Remove(item);

            State.ActiveApp = null;

            State.ActiveDAFApp = null;
  
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

                if (dafApp.Metadata.ContainsKey("PackageVersion") && dafApp.Metadata["PackageVersion"].ToString() == "latest"){
                    dafApp.Metadata["PackageVersion"] = State.VersionLookups[dafApp.Metadata["NPMPackage"].ToString()].ElementAt(1);
                }

                if(dafApp.ApplicationID.IsEmpty()){
                    dafApp.ApplicationID = randomizeGuid();
                }

                var dafAppToSave = State.DAFApps.FirstOrDefault(da => da.ApplicationID == dafApp.ApplicationID);
                
                if (dafAppToSave != null){
                    State.DAFApps.Remove(dafAppToSave);
                }

                State.DAFApps.Add(dafApp);
                
                State.ActiveDAFApp = dafApp;
            }

            await SetActiveApp(entApiKey, State.ActiveApp);
        }

        public virtual async Task SaveDataApp(string entApiKey, string host, Application app)
        {
            if(app.ID.IsEmpty()){
                app.ID = randomizeGuid();
            }

            var appToSave = State.Applications.FirstOrDefault(a => a.ID == app.ID);

            if (appToSave != null){
                State.Applications.Remove(appToSave);
            }

            State.Applications.Add(app);
            
            var newDafApp = new DAFViewConfiguration(){
                ApplicationID = app.ID,
                ID = Guid.NewGuid(),
                Lookup = null,
                NPMPackage = "@lowcodeunit/lcu-charts-demo",
                Priority = 500,
                BaseHref = "/charts/",
                PackageVersion = null
            };

            await SetActiveApp(entApiKey, app);

            await SaveDAFApp(entApiKey, newDafApp);

            State.AddingApp = false;          
        }

        public virtual async Task SetActiveApp(string entApiKey, Application app)
        {
            //await ToggleAddNew();

            State.ActiveApp = app;

            await LoadAppView(entApiKey);
        }

        public virtual async Task ToggleAddNew(bool New)
        {
            State.ActiveApp = null;

            State.AddingApp = New;
        }

        protected virtual Guid randomizeGuid(){
            return Guid.NewGuid();
        }
        
        #endregion
    }
}
