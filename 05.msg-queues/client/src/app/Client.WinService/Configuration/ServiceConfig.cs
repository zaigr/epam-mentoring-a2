namespace Client.ScanService.Configuration
{
    public class ServiceConfig
    {
        [AppSetting("messageMaxSizeBytes")]
        public int MessageMaxSizeBytes { get; set; }

        [AppSetting("logFilePath")]
        public string LogFilePath { get; set; }

        [AppSetting("monitoringFolderConfigFile")]
        public string MonitoringFoldersConfigFile { get; set; }

        [AppSetting("folderScanFrequencySeconds")]
        public int FolderScanFrequencySeconds { get; set; }

        [AppSetting("availabilityCheckFrequencySeconds")]
        public int AvailabilityCheckFrequencySeconds { get; set; }

        [AppSetting("clientId")]
        public string ClientId { get; set; }

        [AppSetting("dataQueueConnectionString")]
        public string DataQueueConnectionString { get; set; }

        [AppSetting("dataQueueName")]
        public string DataQueueName { get; set; }

        [AppSetting("monitoringQueueConnectionString")]
        public string MonitoringQueueConnectionString { get; set; }

        [AppSetting("monitoringQueueName")]
        public string MonitoringQueueName { get; set; }

        [AppSetting("configTopicConnectionString")]
        public string ConfigTopicConnectionString { get; set; }

        [AppSetting("configTopicName")]
        public string ConfigTopicName { get; set; }

        [AppSetting("configTopicSubscriptionName")]
        public string ConfigTopicSubscriptionName { get; set; }
    }
}
