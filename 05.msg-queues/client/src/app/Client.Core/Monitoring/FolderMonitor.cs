using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Client.Core.Monitoring.EventArgs;
using Client.Data;
using Client.Data.Entities;
using Serilog;

namespace Client.Core.Monitoring
{
    public class FolderMonitor : IResourceMonitor
    {
        private readonly int _scanFrequencySeconds;

        private readonly IScanServiceContext _context;

        public FolderMonitor(IScanServiceContext context, int scanFrequencySeconds)
        {
            _context = context;
            _scanFrequencySeconds = scanFrequencySeconds;
        }

        public event EventHandler<ResourceAddedEventArgs> ResourceAdded;

        public event EventHandler<ResourceChangedEventArgs> ResourceChanged;

        public Task StartMonitoring(IEnumerable<string> resourcePaths)
        {
            Log.Debug($"Start timer with interval '{_scanFrequencySeconds}' seconds.");

            var timer = new Timer(
                _ => FolderScanner(resourcePaths),
                state: null,
                dueTime: 0,
                period: (int)TimeSpan.FromSeconds(_scanFrequencySeconds).TotalMilliseconds);

            return Task.CompletedTask;
        }

        private void FolderScanner(IEnumerable<string> folderPaths)
        {
            Log.Debug($"Start folders scanning.");

            foreach (var path in folderPaths)
            {
                Log.Debug($"Scan '{path}' folder.");

                if (!FolderExists(path))
                {
                    Log.Information($"Folder '{path}' does not exists.");
                    continue;
                }

                ScanFolderFiles(path);
            }
        }

        private bool FolderExists(string path) => Directory.Exists(path);

        private void ScanFolderFiles(string directoryPath)
        {
            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                Log.Debug($"Process file '{filePath}'.");

                var fileInfo = new FileInfo(filePath);
                var resource = _context.Resources
                    .FirstOrDefault(r =>
                        (r.Name == (fileInfo.Name + fileInfo.Extension)) && 
                        (r.Path == fileInfo.DirectoryName));

                if (resource == null)
                {
                    Log.Debug($"Add new resource '{fileInfo.Name + fileInfo.Extension}'.");
                    AddResource(fileInfo);
                }
                else if (resource.Hash != GetFileHash(fileInfo))
                {
                    Log.Debug($"Update resource '{resource.Name}'.");
                    UpdateResource(resource, fileInfo);
                }
            }
        }

        private void AddResource(FileInfo fileInfo)
        {
            var resource = new Resource
            {
                Name = fileInfo.Name + fileInfo.Extension,
                Path = fileInfo.DirectoryName,
                Hash = GetFileHash(fileInfo),
                Size = fileInfo.Length,
            };

            _context.Resources.Add(resource);
            _context.SaveChanges();

            OnResourceAdded(new ResourceAddedEventArgs
            {
                ResourceName = resource.Name,
                ResourcePath = resource.Path,
                ResourceBytesSize = resource.Size,
            });
        }

        private void UpdateResource(Resource resource, FileInfo fileInfo)
        {
            resource.Hash = GetFileHash(fileInfo);
            resource.Size = fileInfo.Length;

            _context.Resources.Update(resource);
            _context.SaveChanges();

            OnResourceChanged(new ResourceChangedEventArgs
            {
                ResourceName = resource.Name,
                ResourcePath = resource.Path,
                ResourceBytesSize = resource.Size,
            });
        }

        private string GetFileHash(FileInfo fileInfo)
        {
            using (var stream = new BufferedStream(fileInfo.Open(FileMode.Open), bufferSize: 120000))
            {
                var sha256Managed = new SHA256Managed();
                var hash = sha256Managed.ComputeHash(stream);

                return BitConverter.ToString(hash);
            }
        }

        protected virtual void OnResourceAdded(ResourceAddedEventArgs e)
        {
            ResourceAdded?.Invoke(this, e);
        }

        protected virtual void OnResourceChanged(ResourceChangedEventArgs e)
        {
            ResourceChanged?.Invoke(this, e);
        }
    }
}
