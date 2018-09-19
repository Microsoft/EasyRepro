﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Win32;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium;
=======
using System.IO;
>>>>>>> origin/releases/v8.1

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class BrowserOptions
    {
        public BrowserOptions()
        {
            this.DriversPath = Path.Combine(Directory.GetCurrentDirectory()); //, @"Drivers\");
            this.BrowserType = BrowserType.IE;
            this.PageLoadTimeout = new TimeSpan(0, 3, 0);
            this.CommandTimeout = new TimeSpan(0, 10, 0);
            this.StartMaximized = true;
            this.FireEvents = false;
            this.TraceSource = Constants.DefaultTraceSource;
            this.EnableRecording = false;
            this.RecordingScanInterval = TimeSpan.FromMilliseconds(Constants.Browser.Recording.DefaultScanInterval);
            this.Credentials = BrowserCredentials.Default;
            this.HideDiagnosticWindow = true;
<<<<<<< HEAD
            this.TimeFactor = 1.0f;
        }

        public float TimeFactor { get; set; }
=======
        }

>>>>>>> origin/releases/v8.1
        public BrowserType BrowserType { get; set; }
        public BrowserCredentials Credentials { get; set; }
        public string DriversPath { get; set; }
        public bool PrivateMode { get; set; }
        public bool CleanSession { get; set; }
        public TimeSpan PageLoadTimeout { get; set; }
        public TimeSpan CommandTimeout { get; set; }
        public bool StartMaximized { get; set; }
        public bool FireEvents { get; set; }
        public bool EnableRecording { get; set; }
        public TimeSpan RecordingScanInterval { get; set; }
        public string TraceSource { get; set; }
        public bool HideDiagnosticWindow { get; set; }
<<<<<<< HEAD
        public bool Headless { get; set; }
        public bool UserAgent { get; set; }
        public string UserAgentValue { get; set; }
        public string[] ExtraChromeArguments { get; set; }

        public virtual ChromeOptions ToChrome()
=======
        /// <summary>
        /// Setting that will run the browser in a headless manner.  This setting is only valid for Chrome. 
        /// </summary>
        public bool Headless { get; set; }

        public ChromeOptions ToChrome()
>>>>>>> origin/releases/v8.1
        {
            var options = new ChromeOptions();

            if (this.StartMaximized)
            {
                options.AddArgument("--start-maximized");
            }

            if (this.PrivateMode)
            {
                options.AddArgument("--incognito");
            }
<<<<<<< HEAD

=======
>>>>>>> origin/releases/v8.1
            if (this.Headless)
            {
                options.AddArgument("--headless");
            }

<<<<<<< HEAD
            if (UserAgent && !string.IsNullOrEmpty(UserAgentValue))
            {
                options.AddArgument("--user-agent=" + UserAgentValue);
            }

            if (ExtraChromeArguments != null)
                foreach (var argument in ExtraChromeArguments)
                {
                    options.AddArgument(argument);
                }

            return options;
        }
        
        public virtual InternetExplorerOptions ToInternetExplorer()
=======
            return options;
        }

        public InternetExplorerOptions ToInternetExplorer()
>>>>>>> origin/releases/v8.1
        {

            // For IE, TabProcGrowth must be set to 0 if we want to initiate through
            // the CreateProcess API, which is required for InPrivate mode.
            if (this.PrivateMode)
            {
                var value = Registry.GetValue(Constants.Browser.IESettingsRegistryHive, Constants.Browser.IESettingsTabProcGrowthKey, null);

                if (value == null || value.ToString() != "0")
                {
                    Registry.SetValue(Constants.Browser.IESettingsRegistryHive, Constants.Browser.IESettingsTabProcGrowthKey, 0);
                }
            }

            var options = new InternetExplorerOptions()
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                EnsureCleanSession = this.CleanSession,
                ForceCreateProcessApi = this.PrivateMode,
                //page = InternetExplorerPageLoadStrategy.Eager,
                IgnoreZoomLevel = true,
                EnablePersistentHover = true,
                BrowserCommandLineArguments = this.PrivateMode ? "-private" : ""
               
            };
            
            return options;
        }

<<<<<<< HEAD
        public  virtual FirefoxOptions ToFireFox()
=======
        public FirefoxOptions ToFireFox()
>>>>>>> origin/releases/v8.1
        {
            var options = new FirefoxOptions()
            {
                UseLegacyImplementation = false
            };

            return options;
        }

<<<<<<< HEAD
        public virtual EdgeOptions ToEdge()
        {
            var options = new EdgeOptions()
            {
                PageLoadStrategy = PageLoadStrategy.Normal
=======
        public EdgeOptions ToEdge()
        {
            var options = new EdgeOptions()
            {
                PageLoadStrategy = EdgePageLoadStrategy.Normal
>>>>>>> origin/releases/v8.1
            };

            return options;
        }
    }
}