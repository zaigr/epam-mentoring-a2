using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Client.ScanService.Configuration
{
    public static class ServiceConfigManager
    {
        public static IEnumerable<string> GetScanFoldersConfig(ServiceConfig serviceConfig)
        {
            var configFile = serviceConfig.MonitoringFoldersConfigFile;
            if (!File.Exists(configFile))
            {
                throw new ConfigurationErrorsException($"'{configFile}' config file does not exists.");
            }

            return File.ReadAllLines(configFile);
        }

        public static ServiceConfig GetConfig()
        {
            var appSetting = ConfigurationManager.AppSettings;

            var serviceConfig = new ServiceConfig();
            foreach (var property in GetAppSettingProps())
            {
                var appSettingAttribute = property.GetCustomAttribute<AppSettingAttribute>();
                if (!appSetting.AllKeys.Contains(appSettingAttribute.KeyName))
                {
                    throw new InvalidOperationException(
                        $"'{appSettingAttribute.KeyName}' is not presented in AppSettings.");
                }

                var configValue = Convert
                    .ChangeType(appSetting.Get(appSettingAttribute.KeyName), property.PropertyType);

                property.SetValue(serviceConfig, configValue);
            }

            return serviceConfig;
        }

        private static IEnumerable<PropertyInfo> GetAppSettingProps()
        {
            return typeof(ServiceConfig)
                .GetProperties()
                .Where(p => p.GetCustomAttribute<AppSettingAttribute>() != null);
        }
    }
}
