// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Microsoft.Azure.WebJobs.Script.Config
{
    public class ScriptSettingsManager
    {
        private static ScriptSettingsManager _instance = new ScriptSettingsManager();

        public static ScriptSettingsManager Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        protected ScriptSettingsManager()
        {
        }

        public virtual string GetSetting(string settingKey)
        {
            return Environment.GetEnvironmentVariable(settingKey);
        }

        public virtual void SetSetting(string settingKey, string settingValue)
        {
            if (!string.IsNullOrEmpty(settingKey))
            {
                Environment.SetEnvironmentVariable(settingKey, settingValue);
            }
        }
    }
}