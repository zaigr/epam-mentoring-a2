using System;

namespace Client.ScanService.Configuration
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AppSettingAttribute : Attribute
    {
        public AppSettingAttribute(string keyName)
        {
            KeyName = keyName;
        }

        public string KeyName { get; }
    }
}
