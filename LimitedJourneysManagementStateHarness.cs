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
    public class LimitedJourneysManagementStateHarness : LCUStateHarness<LimitedJourneysManagementState>
    {
        #region Fields
        protected readonly List<DAFApplicationConfiguration> dafApps;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public LimitedJourneysManagementStateHarness(LimitedJourneysManagementState state)
            : base(state ?? new LimitedJourneysManagementState())
        {

        }
        #endregion

        #region API Methods
        public virtual async Task LoadJourneyOptions()
        {
            State.Journeys = new List<JourneyOption>()
            {
                new JourneyOption()
                {
                    Name = "IOT - To the Edge and Beyond!",
                    ContentURL = "https://player.vimeo.com/video/403508452",
                    ContentType = JourneyContentTypes.Video,
                    Uses = new List<string>() { "Devices", "Data Flow", "Data Science" },
                    Description = "Build and connect edge devices, securely manage, visualize and analyze your data, and take action on your intelligence.",
                    Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                    Active = true
                },
                new JourneyOption()
                {
                    Name = "Application Development",
                    ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                    ContentType = JourneyContentTypes.Image,
                    Uses = new List<string>() { "JS Apps", "Security", "Dev Tools" },
                    Description = "Develop JavaScript applications in the framework of your choosing and easily deploy, secure, and manage at scale.",
                    Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                    ComingSoon = true,
                    Active = true
                },
                new JourneyOption()
                {
                    Name = "Cloud Development",
                    ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                    ContentType = JourneyContentTypes.Image,
                    Uses = new List<string>() { "DevOps", "IaC", "Data Flow" },
                    Description =  "Rapidly set up and manage enterprise grade, best practice cloud infrastructures and leverage them to build apps and APIs.",
                    Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                    ComingSoon = true,
                    Active = true
                },
                new JourneyOption()
                {
                    Name = "Data Development",
                    ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                    ContentType = JourneyContentTypes.Image,
                    Uses = new List<string>() { "AI/ML", "Analytics", "Reporting" },
                    Description =  "Develop data applications from existing and new enterprise data. Leverage existing tools with new at a rapid pace.",
                    Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                    ComingSoon = true,
                    Active = true
                },
                new JourneyOption()
                {
                    Name = "Cloud Orchestration",
                    ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                    ContentType = JourneyContentTypes.Image,
                    Uses = new List<string>() { "DevOps", "IaC", "Data Flow" },
                    Description =  "Rapidly set up and manage enterprise grade, best practice cloud infrastructures and leverage them to build apps and APIs.",
                    Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                    ComingSoon = true,
                    Active = true
                },
                new JourneyOption()
                {
                    Name = "Enterprise Intranets",
                    ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                    ContentType = JourneyContentTypes.Image,
                    Uses = new List<string>() { "Dashboards", "Reporting", "Identity" },
                    Description =  "Leverage our Enterprise IDE to rapidly pull together open source and custom LCUs to drive value in your organization.",
                    Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                    ComingSoon = true,
                    Active = true
                },
                new JourneyOption()
                {
                    Name = "Designer Tools",
                    ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                    ContentType = JourneyContentTypes.Image,
                    Uses = new List<string>() { "AI/ML", "Analytics", "Reporting" },
                    Description =  "Develop data applications from existing and new enterprise data. Leverage existing tools with new at a rapid pace.",
                    Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Designer },
                    ComingSoon = true,
                    Active = true
                },
                new JourneyOption()
                {
                    Name = "Admin Tools",
                    ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                    ContentType = JourneyContentTypes.Image,
                    Uses = new List<string>() { "AI/ML", "Analytics", "Reporting" },
                    Description =  "Develop data applications from existing and new enterprise data. Leverage existing tools with new at a rapid pace.",
                    Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Administrator },
                    ComingSoon = true,
                    Active = true
                }
            };
        }

        public virtual async Task Refresh()
        {
            await LoadJourneyOptions();
        }
        #endregion
    }
}
