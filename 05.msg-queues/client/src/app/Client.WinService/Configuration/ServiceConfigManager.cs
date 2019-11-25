using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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

        public static void UpdateConfigSource<TProp>(
            Expression<Func<ServiceConfig, TProp>> property,
            TProp value)
        {
            EnsureIsMemberExpression(property.Body);

            var appSettingAttribute = ((MemberExpression)property.Body).Member
                .GetCustomAttribute<AppSettingAttribute>();

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[appSettingAttribute.KeyName].Value = value.ToString();
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
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

        private static void EnsureIsMemberExpression(Expression expr)
        {
            if (!(expr is MemberExpression memberExpression &&
                  memberExpression.Member is PropertyInfo))
            {
                throw new ArgumentException($"{expr} should be member expression.");
            }
        }
    }
}
