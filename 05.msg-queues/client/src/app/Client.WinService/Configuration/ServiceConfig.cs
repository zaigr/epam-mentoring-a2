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

        [AppSetting("dataQueueConnectionString")]
        public string DataQueueConnectionString { get; set; }

        [AppSetting("dataQueueName")]
        public string DataQueueName { get; set; }
    }
}
