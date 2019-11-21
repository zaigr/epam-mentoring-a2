using System;

namespace Client.ScanService.Configuration
{
    public class ServiceConfig
    {
        [AppSetting("messageMaxSizeKb")]
        public int MessageMaxSize { get; set; }

        [AppSetting("logFilePath")]
        public string LogFilePath { get; set; }

        [AppSetting("monitoringFolderConfigFile")]
        public string MonitoringFoldersConfigFile { get; set; }

        [AppSetting("folderScanFrequencySeconds")]
        public int FolderScanFrequencySeconds { get; set; }
    }
}
