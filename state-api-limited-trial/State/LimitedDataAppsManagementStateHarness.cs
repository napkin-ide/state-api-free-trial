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

namespace LCU.State.API.NapkinIDE.NapkinIDE.LimitedTrial.State
{
    public class LimitedDataAppsManagementStateHarness : LCUStateHarness<LimitedDataAppsManagementState>
    {
        #region Fields
        protected readonly List<DAFApplication> dafApps;
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
        public virtual void Mock(string entLookup, string host)
        {
            if (State.Applications.IsNullOrEmpty())
            {
                State.Applications = new List<Application>{
                    new Application(){
                        Container = "lcu-data-apps",
                        EnterpriseLookup = entLookup,
                        Hosts = new string[]{
                            host,
                        },
                        Name = "Hello World",
                        PathRegex = "/helloworld*",
                        Priority = 10000,
                        ID = new Guid("00000000-0000-0000-0000-000000000001")
                    },

                    new Application(){
                        Container = "lcu-data-apps",
                        EnterpriseLookup = entLookup,
                        Hosts = new string[]{
                            host,
                        },
                        Name = "Fathym Forecast",
                        PathRegex = "/forecast*",
                        Priority = 10500,
                        ID = new Guid("00000000-0000-0000-0000-000000000002")
                    },

                    // new Application(){
                    //     Container = "lcu-data-apps",
                    //     EnterpriseLookup = entLookup,
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
                        EnterpriseLookup = entLookup,
                        Hosts = new string[]{
                            host,
                        },
                        Name = "Trial Dashboard",
                        PathRegex = "/dashboard*",
                        Priority = 11500,
                        ID = new Guid("00000000-0000-0000-0000-000000000004")
                    }
                };

                State.DAFApps = new List<DAFApplication>(){
                    new DAFApplication(){
                        ApplicationID = "00000000-0000-0000-0000-000000000004",
                        ID = new Guid("a0000000-0000-0000-0000-000000000004"),
                        Lookup = null,
                        Priority = 500,
                        Details = new DAFViewApplicationDetails()
                        {
                            BaseHref = "/dashboard/",
                            NPMPackage = "@lowcodeunit-dashboards/lcu-fathym-dashboard-getting-started",
                            PackageVersion = "1.1.23-integration"
                        }
                    },

                    new DAFApplication(){
                        ApplicationID = "00000000-0000-0000-0000-000000000001",
                        ID = new Guid("a0000000-0000-0000-0000-000000000001"),
                        Lookup = null,
                        Priority = 500,
                        Details = new DAFViewApplicationDetails(){
                        BaseHref = "/helloworld/",
                        NPMPackage = "@fathym-it/hello-world-demo",
                        PackageVersion = "1.3.15"
                        }
                    },

                    new DAFApplication(){
                        ApplicationID = "00000000-0000-0000-0000-000000000002",
                        ID = new Guid("a0000000-0000-0000-0000-000000000002"),
                        Lookup = null,
                        Priority = 500,
                        Details = new DAFViewApplicationDetails(){
                        BaseHref = "/forecast/",
                        NPMPackage = "@habistack/lcu-fathym-forecast-demo",
                        PackageVersion = "1.1.1"
                        }
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

        public virtual async Task DeleteDataApp(string entLookup, string appID)
        {
            var appToDelete = State.Applications.FirstOrDefault(a => a.ID.ToString() == appID);

            State.Applications.Remove(appToDelete);

            var item = State.DAFApps.FirstOrDefault(da => da.ApplicationID.ToString() == appID);

            State.DAFApps.Remove(item);

            State.ActiveApp = null;

            State.ActiveDAFApp = null;

        }

        public virtual async Task LoadAppView(string entLookup)
        {
            if (State.ActiveApp != null)
            {
                var dafApps = State.DAFApps;

                State.ActiveDAFApp = State.DAFApps?.FirstOrDefault(da => da.ApplicationID == State.ActiveApp.ID.ToString())?.JSONConvert<DAFApplication>();

                State.ActiveDAFAPIs = null;

            }
            else
                State.ActiveDAFApp = null;
        }

        public virtual async Task SaveDAFApp(string entLookup, DAFApplication dafApp)
        {
            if (State.ActiveApp != null)
            {
                if (State.AppType != DAFAppTypes.API)
                {
                    if (dafApp.Details.Metadata.ContainsKey("APIRoot"))
                        dafApp.Details.Metadata.Remove("APIRoot");

                    if (dafApp.Details.Metadata.ContainsKey("InboundPath"))
                        dafApp.Details.Metadata.Remove("InboundPath");

                    if (dafApp.Details.Metadata.ContainsKey("Methods"))
                        dafApp.Details.Metadata.Remove("Methods");

                    if (dafApp.Details.Metadata.ContainsKey("Security"))
                        dafApp.Details.Metadata.Remove("Security");
                }

                if (State.AppType != DAFAppTypes.Redirect)
                {
                    if (dafApp.Details.Metadata.ContainsKey("Redirect"))
                        dafApp.Details.Metadata.Remove("Redirect");
                }

                if (State.AppType != DAFAppTypes.View)
                {
                    if (dafApp.Details.Metadata.ContainsKey("BaseHref"))
                        dafApp.Details.Metadata.Remove("BaseHref");

                    if (dafApp.Details.Metadata.ContainsKey("NPMPackage"))
                        dafApp.Details.Metadata.Remove("NPMPackage");

                    if (dafApp.Details.Metadata.ContainsKey("PackageVersion"))
                        dafApp.Details.Metadata.Remove("PackageVersion");
                }

                if (dafApp.Details.Metadata.ContainsKey("PackageVersion") && dafApp.Details.Metadata["PackageVersion"].ToString() == "latest")
                {
                    dafApp.Details.Metadata["PackageVersion"] = State.VersionLookups[dafApp.Details.Metadata["NPMPackage"].ToString()].ElementAt(1);
                }

                if (dafApp.ApplicationID.IsNullOrEmpty())
                {
                    dafApp.ApplicationID = randomizeGuid().ToString();
                }

                var dafAppToSave = State.DAFApps.FirstOrDefault(da => da.ApplicationID == dafApp.ApplicationID);

                if (dafAppToSave != null)
                {
                    State.DAFApps.Remove(dafAppToSave);
                }

                State.DAFApps.Add(dafApp);

                State.ActiveDAFApp = dafApp;
            }

            await SetActiveApp(entLookup, State.ActiveApp);
        }

        public virtual async Task SaveDataApp(string entLookup, string host, Application app)
        {
            if (app.ID.IsEmpty())
            {
                app.ID = randomizeGuid();
            }

            var appToSave = State.Applications.FirstOrDefault(a => a.ID == app.ID);

            if (appToSave != null)
            {
                State.Applications.Remove(appToSave);
            }

            State.Applications.Add(app);

            var newDafApp = new DAFApplication()
            {
                ApplicationID = app.ID.ToString(),
                ID = Guid.NewGuid(),
                Lookup = null,
                Priority = 500,
                Details = new DAFViewApplicationDetails()
                {
                    NPMPackage = "@lowcodeunit/lcu-charts-demo",
                    BaseHref = "/charts/",
                    PackageVersion = null
                }
            };

            await SetActiveApp(entLookup, app);

            await SaveDAFApp(entLookup, newDafApp);

            State.AddingApp = false;
        }

        public virtual async Task SetActiveApp(string entLookup, Application app)
        {
            //await ToggleAddNew();

            State.ActiveApp = app;

            await LoadAppView(entLookup);
        }

        public virtual async Task ToggleAddNew(bool New)
        {
            State.ActiveApp = null;

            State.AddingApp = New;
        }

        protected virtual Guid randomizeGuid()
        {
            return Guid.NewGuid();
        }

        #endregion
    }
}
