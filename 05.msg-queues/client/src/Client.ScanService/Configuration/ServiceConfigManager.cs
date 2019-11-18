using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Client.ScanService.Configuration
{
    public static class ServiceConfigManager
    {
        public static ServiceConfig GetConfig()
        {
            var appSetting = ConfigurationManager.AppSettings;

            var serviceConfig = new ServiceConfig();
            foreach (var property in GetAppSettingProps())
            {
                var appSettingAttribute = property.GetCustomAttribute<AppSettingAttribute>();
                if (appSetting.AllKeys.Contains(appSettingAttribute.KeyName))
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
