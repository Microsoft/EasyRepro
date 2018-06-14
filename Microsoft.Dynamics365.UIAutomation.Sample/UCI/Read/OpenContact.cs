﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenContactUCI
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestOpenActiveContact()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp("UCI");

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");
                
                xrmApp.Grid.Search("Anthony");

                xrmApp.Grid.OpenRecord(0);
                
            }
            
        }

        [TestMethod]
        public void UCITestOpenRecordSetNavigator()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp("UCI");

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                // open record set navigator and go to 3rd record
                xrmApp.Entity.OpenRecordSetNavigator(2);

                xrmApp.Entity.CloseRecordSetNavigator();

                // open record set navigator and go to 1st record
                xrmApp.Entity.OpenRecordSetNavigator();

                // without closing the record set navigator go to 2nd record
                xrmApp.Entity.OpenRecordSetNavigator(1);

                //xrmApp.Entity.CloseRecordSetNavigator();
            }
        }
    }
}