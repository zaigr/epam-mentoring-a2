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

        private Timer _timer;

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

            _timer = new Timer(
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

                    RemoveFolderResources(path);
                    continue;
                }

                ScanFolderFiles(path);
            }
        }

        private bool FolderExists(string path) => Directory.Exists(path);

        private void RemoveFolderResources(string directoryPath)
        {
            var directoryResources = _context.Resources
                .Where(r => r.Path == directoryPath);

            _context.Resources.RemoveRange(directoryResources);
            _context.SaveChanges();
        }

        private void ScanFolderFiles(string directoryPath)
        {
            var directoryResources = _context.Resources
                .Where(r => r.Path == directoryPath)
                .ToList();

            var directoryFiles = Directory.GetFiles(directoryPath);

            ScanExistingFiles(directoryFiles, directoryResources);

            RemoveMissingFiles(directoryFiles, directoryResources);
        }

        private void ScanExistingFiles(IList<string> directoryFiles, IList<Resource> directoryResources)
        {
            foreach (var filePath in directoryFiles)
            {
                Log.Debug($"Process file '{filePath}'.");

                var fileInfo = new FileInfo(filePath);
                var resource = directoryResources
                    .FirstOrDefault(r => r.Name == fileInfo.Name);

                if (resource == null)
                {
                    Log.Debug($"Add new resource '{fileInfo.Name}'.");
                    AddResource(fileInfo);
                }
                else if (resource.Hash != GetFileHash(fileInfo))
                {
                    Log.Debug($"Update resource '{resource.Name}'.");
                    UpdateResource(resource, fileInfo);
                }
            }
        }

        private void RemoveMissingFiles(IList<string> directoryFiles, IList<Resource> directoryResources)
        {
            foreach (var resource in directoryResources)
            {
                var resourceFullPath = Path.Combine(resource.Path, resource.Name);
                if (!directoryFiles.Contains(resourceFullPath))
                {
                    _context.Resources.Remove(resource);
                    _context.SaveChanges();
                }
            }
        }

        private void AddResource(FileInfo fileInfo)
        {
            var resource = new Resource
            {
                Name = fileInfo.Name,
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
            using (var sha256Managed = new SHA256Managed())
            {
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

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
