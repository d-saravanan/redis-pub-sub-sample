using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// The Application Settings Manager
    /// </summary>
    public static class AppSettingsManager
    {
        /// <summary>
        /// Gets the application setting value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetAppSettingValue(string key)
            => System.Configuration.ConfigurationManager.AppSettings[key];
    }
}
