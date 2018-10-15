﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    ///  The Home page.
    ///  </summary>
    public class CommandBar
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public CommandBar(InteractiveBrowser browser)
            : base(browser)
        {
        }

        public BrowserCommandResult<bool> ClickCommand(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions("Click CommandBar Command"), driver =>
            {
                ClickCommand(name, "", thinkTime);
                return true;
            });
        }
        public BrowserCommandResult<bool> ClickCommand(string name, string subButton = "", int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Click CommandBar Command"), driver =>
            {
                ClickCommandButton(name, subButton, false);
                return true;
            });
        }
        public BrowserCommandResult<bool> VerifyButtonIsClickable(string name, string subButton, bool throwExceptionIfVisible, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Verify Button is Clickable"), driver =>
            {
                bool isDisabled = IsButtonDisabled(name, subButton);

                if(throwExceptionIfVisible && !isDisabled)
                    throw new InvalidOperationException($"SubButton '{name}' should not be visible.");

                return true;
            });
        }
        public BrowserCommandResult<bool> DownloadResults(string solutionName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Download Results"), driver =>
            {

                
                string currentStatus = GetCurrentStatus(solutionName);

                //Download results if/when complete
                if (currentStatus.Contains("Results", StringComparison.OrdinalIgnoreCase))
                {
                    //Click off the current record and back onto this one before downloading results
                    ClickCommandButton("Solution Checker", "Download last results");
                }
                

                ClickCommandButton("Solution Checker", "Download last results");

                return true;
            });
        }

        /// <summary>
        /// Gets the Commands
        /// </summary>
        /// <param name="moreCommands">The MoreCommands</param>
        /// <example></example>
        public BrowserCommandResult<ReadOnlyCollection<IWebElement>> GetCommands(bool moreCommands = false)
        {
            return this.Execute(GetOptions("Get Command Bar Buttons"), driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.CommandBar.OverflowContainer]), new TimeSpan(0, 0, 5));

                IWebElement ribbon = null;
                if (moreCommands)
                    ribbon = driver.FindElement(By.XPath(Elements.Xpath[Reference.CommandBar.ContextualMenuList]));
                //else
                  //  ribbon = driver.FindElement(By.XPath(Elements.Xpath[Reference.CommandBar.RibbonManager]));

                var items = ribbon.FindElements(By.TagName("li"));

                return items;//.Where(item => item.Text.Length > 0).ToDictionary(item => item.Text, item => item.GetAttribute("id"));
            });
        }

        internal bool ClickCommandButton(string name, string subButton = "", bool throwExceptionIfVisible = false)
        {
            var driver = Browser.Driver;

            int nestedSubContainer = 0;

            //First button
            var commandBarContainer = driver.FindElement(By.XPath(Elements.Xpath[Reference.CommandBar.Container]));
            var buttons = commandBarContainer.FindElements(By.TagName("button"));
            var button = buttons.FirstOrDefault(b => b.Text.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (button == null)
            {
                
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.CommandBar.OverflowContainer]));
                buttons = GetCommands(true).Value;
                button = buttons.FirstOrDefault(x => x.Text.Contains(name, StringComparison.OrdinalIgnoreCase));
                nestedSubContainer = 1;
            }

            if (button == null)
                throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar.");

            button.Click(true);

            Browser.ThinkTime(1500);
            
            //Sub Button
            if (!string.IsNullOrEmpty(subButton))
            {
                //found = false;
                var subButtonContainer = driver.FindElements(By.XPath(Elements.Xpath[Reference.CommandBar.SubButtonContainer]));
                var subButtons = subButtonContainer[nestedSubContainer].FindElements(By.TagName("button"));

                var sButton = subButtons.FirstOrDefault(b => b.Text.Contains(subButton, StringComparison.OrdinalIgnoreCase));

                if (sButton == null)
                    throw new InvalidOperationException($"No subButton with the name '{subButton}' exists inside of Commandbar.");

                //Is the button visible?
                bool isDisabled;
                var currentVisibleStatus = sButton.GetAttribute("aria-disabled");
                bool.TryParse(currentVisibleStatus, out isDisabled);

                if (!isDisabled)
                    sButton.Click(true);
            }

            return true;
        }
        internal bool IsButtonDisabled (string name, string subButton = "")
        {
            var driver = Browser.Driver;
            bool isDisabled = true;

            ClickCommandButton(name, "");

            var subButtonContainer = driver.FindElements(By.XPath(Elements.Xpath[Reference.CommandBar.SubButtonContainer]));

            if (subButtonContainer.Count == 0)
                Console.WriteLine("SubButton container is empty");

            var subButtons = subButtonContainer[0].FindElements(By.TagName("button"));
            var sButton = subButtons.FirstOrDefault(b => b.Text.Equals(subButton, StringComparison.OrdinalIgnoreCase));

            bool.TryParse(sButton.GetAttribute("aria-disabled"), out isDisabled);

            ClickCommandButton(name, "");

            return isDisabled;
        }
        internal string GetCurrentStatus(string solutionName)
        {
            var driver = Browser.Driver;
            //Retrieve current status.  XPath was misbehaving trying to traverse rows and columns so we have to cheat.
            var solutionNames = driver.FindElements(By.XPath(Elements.Xpath[Reference.CommandBar.GridSolutionNameColumn]));
            var solutionStatuses = driver.FindElements(By.XPath(Elements.Xpath[Reference.CommandBar.GridSolutionStatusColumn]));

            int rowNumber = -1;
            int cnt = 0;

            foreach (var row in solutionNames)
            {
                if (row.Text.Contains(solutionName, StringComparison.OrdinalIgnoreCase))
                {
                    rowNumber = cnt;
                    break;
                }

                cnt++;
            }

            if (rowNumber == -1)
                Console.WriteLine("Could not find status for this solution");
            return solutionStatuses[rowNumber].Text;
        }
    }
}
