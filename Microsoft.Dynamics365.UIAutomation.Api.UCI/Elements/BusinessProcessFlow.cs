﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class BusinessProcessFlow : Element
    {
        private readonly WebClient _client;

        public BusinessProcessFlow(WebClient client)
        {
            _client = client;
        }

        public void SetActive(string stageName = "")
        {
            // This makes the assumption that SelectStage() has already been called
            _client.SetActive(stageName);
        }

        public void NextStage(string stageName, Field businessProcessFlowField = null)
        {
            //Keeping code here for now so I don't check-in and overwrite changes to WebClient.cs
            _client.NextStage(stageName, businessProcessFlowField);
        }

        public void SelectStage(string stageName)
        {
            _client.SelectStage(stageName);
        }

        public void SetValue(string field, string value)
        {
            _client.BPFSetValue(field, value);
        }

        public void SetValue(OptionSet optionSet)
        {
            _client.BPFSetValue(optionSet);
        }

        public void SetValue(BooleanItem optionSet)
        {
            _client.BPFSetValue(optionSet);
        }

        public void SetValue(LookupItem control, int index = 0)
        {
            _client.SetValue(control, index);
        }




        public void SetValue(string field, DateTime date, string format = "MM dd yyyy")
        {
            _client.SetValue(field, date, format);
        }

        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            _client.SetValue(option, removeExistingValues);
        }
    }
}
