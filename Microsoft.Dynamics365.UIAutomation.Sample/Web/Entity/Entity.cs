﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    using Microsoft.Dynamics365.UIAutomation.Api;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [TestClass]
    public class Entity : TestBase
    {
        [TestInitialize]
        public override void TestSetup()
        {
            XrmTestBrowser.ThinkTime(500);
            OpenEntity("Sales", "Contacts", "Active Contacts");
        }

        [TestMethod]
        public void WEBTestNavigateUp()
        {
            XrmTestBrowser.Grid.OpenRecord(1);
            XrmTestBrowser.Entity.NavigateUp();
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestNavigateDown()
        {
            XrmTestBrowser.Grid.OpenRecord(0);
            XrmTestBrowser.Entity.NavigateDown();
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestGetRecordGuid()
        {
            XrmTestBrowser.Grid.OpenRecord(0);
            var recordGuid = XrmTestBrowser.Entity.GetRecordGuid().Value;
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestSetHeaderValues()
        {
            XrmTestBrowser.Grid.OpenRecord(0); // Account

            XrmTestBrowser.Entity.SetHeaderValue("numberofemployees", "5000"); // Text Field
            //var getNumberOfEmployees = XrmTestBrowser.Entity.GetHeaderValue("numberofemployees");
            //XrmTestBrowser.Entity.ClearHeaderValue("numberofemployees");

            XrmTestBrowser.Entity.SetHeaderValue(new LookupItem { Name = "owner", Index = 0 }); // Lookup Field
            //var getOwner = XrmTestBrowser.Entity.GetHeaderValue(new LookupItem { Name = "owner"}); // Lookup Field
            //XrmTestBrowser.Entity.ClearHeaderValue(new LookupItem { Name = "owner"}); // Lookup Field

            OpenEntity("Sales", "Leads", "All Leads");

            XrmTestBrowser.Grid.OpenRecord(0); // Lead

            XrmTestBrowser.Entity.SetHeaderValue(new OptionSet { Name = "leadsourcecode", Value = "Advertisement" }); // OptionSet Field
            //var getLeadSourceCode = XrmTestBrowser.Entity.GetHeaderValue(new OptionSet { Name = "leadsourcecode"}); // OptionSet Field
            //XrmTestBrowser.Entity.ClearHeaderValue(new OptionSet { Name = "leadsourcecode"}); // OptionSet Field

            OpenEntity("Sales", "Opportunities", "All Opportunities");

            XrmTestBrowser.Grid.OpenRecord(0); // Opportunity

            XrmTestBrowser.Entity.SetHeaderValue(new DateTimeControl { Name = "estimatedclosedate", Value = DateTime.Parse("06/15/2019") }); // DateTime Field
            //var getEstCloseDate = XrmTestBrowser.Entity.GetHeaderValue(new DateTimeControl { Name = "estimatedclosedate"}); // DateTime Field
            //XrmTestBrowser.Entity.ClearHeaderValue(new DateTimeControl { Name = "estimatedclosedate"}); // DateTime Field

            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestSetGetClearCompositeValues()
        {
            XrmTestBrowser.CommandBar.ClickCommand("New");

            List<Field> fields = new List<Field>
                {
                    new Field() {Id = "firstname", Value = "EasyRepro"},
                    new Field() {Id = "lastname", Value = "Entity Test"}
                };

            XrmTestBrowser.Entity.SetValue(new CompositeControl { Id = "fullname", Fields = fields}); //Composite Field
            var fullNameValue = XrmTestBrowser.Entity.GetValue(new CompositeControl { Id = "fullname", Fields = fields }).Value; //Composite Field
            var fullNameTest = string.Join(" ", fields.Select(x => x.Value).ToArray());
            Assert.IsTrue(fullNameValue.Equals(fullNameTest));
            
            // The code below is an example of using ClearValue on a CompositeControl 
            // In this example, one of the fields is required and as a result we cannot clear the composite field.
            // Throw exception on the test to alert this issue

            // XrmTestBrowser.Entity.ClearValue(new CompositeControl { Id = "fullname", Fields = fields }); //Composite Required Field Example --> EXCEPTION

            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestSetGetClearRecordValues()
        {
            OpenEntity("Sales", "Quotes", "All Quotes");
            XrmTestBrowser.CommandBar.ClickCommand("New");

            XrmTestBrowser.Entity.SetValue("name", "EasyRepro Quote"); //Text Field
            var quoteName = XrmTestBrowser.Entity.GetValue("name").Value; //Text field
            Assert.IsNotNull(quoteName);
            XrmTestBrowser.Entity.ClearValue("name"); //Text field

            XrmTestBrowser.Entity.SetValue(new LookupItem {Name = "opportunityid", Index = 0 }); //Lookup Field
            var opportunityLookup = XrmTestBrowser.Entity.GetValue(new LookupItem { Name = "opportunityid" }).Value; //Lookup Field
            Assert.IsNotNull(opportunityLookup);
            XrmTestBrowser.Entity.ClearValue(new LookupItem { Name = "opportunityid" }); //Lookup Field

            XrmTestBrowser.Entity.SetValue(new DateTimeControl { Name = "effectiveto", Value = System.DateTime.Parse("06/15/2019") }); //DateTime Field
            var quoteExpiration = XrmTestBrowser.Entity.GetValue(new DateTimeControl { Name = "effectiveto", Value = System.DateTime.Parse("06/15/2019") }).Value; //DateTime Field
            Assert.AreEqual(System.DateTime.Parse(quoteExpiration), System.DateTime.Parse("06/15/2019"));
            XrmTestBrowser.Entity.ClearValue(new DateTimeControl { Name = "effectiveto" }); //DateTime Field

            XrmTestBrowser.Entity.SetValue(new OptionSet { Name = "shippingmethodcode", Value = "Postal Mail" }); //OptionSet field
            var optionSetValue = XrmTestBrowser.Entity.GetValue(new OptionSet { Name = "shippingmethodcode" }).Value; //OptionSet field
            Assert.IsNotNull(optionSetValue);
            XrmTestBrowser.Entity.ClearValue(new OptionSet { Name = "shippingmethodcode" }); //OptionSet field

            XrmTestBrowser.Entity.SetValue(new TwoOption { Name = "willcall", Value = "Will Call" }); //TwoOption field
            var twoOptionValue = XrmTestBrowser.Entity.GetValue(new TwoOption { Name = "willcall" }).Value; //TwoOption field --> 0 (Address) = False, 1 (WillCall) = True. 
            Assert.IsTrue(twoOptionValue);
            XrmTestBrowser.Entity.ClearValue(new TwoOption { Name = "willcall" }); //TwoOption field

            XrmTestBrowser.ThinkTime(5000);
        }
    }
}