using System;
using System.IO;
using System.Linq;
using System.Threading;
using Client.Core.Monitoring;
using Client.Core.Monitoring.EventArgs;
using Client.Data.Entities;
using Xunit;
using Xunit.Sdk;

namespace Client.Core.Tests.Unit.Monitoring
{
    public class FolderMonitorTests
    {
        [Fact]
        public void GivenFolder_WhenFolderEmpty_ThenFolderScanned()
        {
            // Arrange
            var directory = Directory.CreateDirectory(Guid.NewGuid().ToString());

            var context = DbContextFactory.CreateImMemory();
            var monitor = new FolderMonitor(context, scanFrequencySeconds: 20);

            // Act
            var recorded = Record.Exception(
                () => Assert.Raises<ResourceAddedEventArgs>(
                    handler => monitor.ResourceAdded += handler,
                    handler => monitor.ResourceAdded -= handler,
                    () =>
                    {
                        using (monitor)
                        {
                            monitor.StartMonitoring(new[] { directory.FullName });
                            Thread.Sleep((int)TimeSpan.FromSeconds(5).TotalMilliseconds);
                        }
                    }));

            // Assert
            Assert.IsType<RaisesException>(recorded);
            Assert.Empty(context.Resources);

            directory.Delete();
        }

        [Fact]
        public void GivenFolders_WhenOneFolderNotEmpty_ThenNewResourcesAdded()
        {
            // Arrange
            var emptyDir = Directory.CreateDirectory(Guid.NewGuid().ToString());
            var notEmptyDir = Directory.CreateDirectory(Guid.NewGuid().ToString());

            var filePath = Path.Combine(notEmptyDir.FullName, Guid.NewGuid().ToString());
            using (var writer = File.CreateText(filePath))
            {
                writer.WriteLine("Hello world!");
            }

            // Act
            var context = DbContextFactory.CreateImMemory();
            var monitor = new FolderMonitor(context, scanFrequencySeconds: 20);

            var raised = Assert.Raises<ResourceAddedEventArgs>(
                handler => monitor.ResourceAdded += handler,
                handler => monitor.ResourceAdded -= handler,
                () =>
                {
                    using (monitor)
                    {
                        monitor.StartMonitoring(
                            new[] { emptyDir.FullName, notEmptyDir.FullName });
                        Thread.Sleep((int) TimeSpan.FromSeconds(5).TotalMilliseconds);
                    }
                });

            // Assert
            Assert.NotNull(raised);
            Assert.Equal(monitor, raised.Sender);

            var fileInfo = new FileInfo(filePath);
            AssertEventArgs(fileInfo, raised.Arguments);

            var resource = context.Resources.FirstOrDefault(r => r.Name == fileInfo.Name);
            Assert.NotNull(resource);
            AssertResource(fileInfo, resource);

            fileInfo.Delete();
            notEmptyDir.Delete();
            emptyDir.Delete();
        }

        [Fact]
        public void GivenTrackedFile_WhenFileChanged_ThenFileUpdated()
        {
            // Arrange
            var directory = Directory.CreateDirectory(Guid.NewGuid().ToString());

            var fileName = Guid.NewGuid().ToString();
            using (var writer = File.CreateText(Path.Combine(directory.FullName, fileName)))
            {
                writer.WriteLine("Hello world!");
            }

            var resource = new Resource
            {
                Name = fileName,
                Path = directory.FullName,
                Hash = Guid.NewGuid().ToString(),
                Size = 0
            };

            var context = DbContextFactory.CreateImMemory();
            context.Resources.Add(resource);
            context.SaveChanges();

            // Act
            var monitor = new FolderMonitor(context, scanFrequencySeconds: 20);

            var raised = Assert.Raises<ResourceChangedEventArgs>(
                handler => monitor.ResourceChanged += handler,
                handler => monitor.ResourceChanged -= handler,
                () =>
                {
                    using (monitor)
                    {
                        monitor.StartMonitoring(
                            new[] { directory.FullName });
                        Thread.Sleep((int)TimeSpan.FromSeconds(5).TotalMilliseconds);
                    }
                });

            // Assert
            Assert.NotNull(raised);
            Assert.Equal(monitor, raised.Sender);

            var fileInfo = new FileInfo(Path.Combine(directory.FullName, fileName));
            AssertEventArgs(fileInfo, raised.Arguments);

            resource = context.Resources.FirstOrDefault(r => r.Name == fileInfo.Name);
            Assert.NotNull(resource);
            AssertResource(fileInfo, resource);

            fileInfo.Delete();
            directory.Delete();
        }

        [Fact]
        public void GivenResource_WhenFolderRemoved_ThenResourceDeleted()
        {
            // Arrange
            var resource = new Resource
            {
                Name = Guid.NewGuid().ToString(),
                Path = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString()),
                Hash = Guid.NewGuid().ToString(),
                Size = 200
            };

            var context = DbContextFactory.CreateImMemory();
            context.Resources.Add(resource);
            context.SaveChanges();

            var directory = Directory.CreateDirectory(Guid.NewGuid().ToString());

            // Act
            using (var monitor = new FolderMonitor(context, scanFrequencySeconds: 20))
            {
                monitor.StartMonitoring(
                    new[] { directory.FullName, resource.Path });
                Thread.Sleep((int)TimeSpan.FromSeconds(10).TotalMilliseconds);
            }

            // Assert
            Assert.Empty(context.Resources);

            directory.Delete();
        }

        [Fact]
        public void GivenResource_WhenFileRemoved_ThenResourceDeleted()
        {
            // Arrange
            var directory = Directory.CreateDirectory(Guid.NewGuid().ToString());
            var resource = new Resource
            {
                Name = Guid.NewGuid().ToString(),
                Path = directory.FullName,
                Hash = Guid.NewGuid().ToString(),
                Size = 200
            };

            var context = DbContextFactory.CreateImMemory();
            context.Resources.Add(resource);
            context.SaveChanges();

            // Act
            using (var monitor = new FolderMonitor(context, scanFrequencySeconds: 20))
            {
                monitor.StartMonitoring(
                    new[] { directory.FullName });
                Thread.Sleep((int)TimeSpan.FromSeconds(10).TotalMilliseconds);
            }

            // Assert
            Assert.Empty(context.Resources);

            directory.Delete();
        }

        private static void AssertEventArgs(FileInfo fileInfo, ResourceEventArgsBase args)
        {
            Assert.Equal(fileInfo.Name, args.ResourceName);
            Assert.Equal(fileInfo.Directory?.FullName, args.ResourcePath);
            Assert.Equal(fileInfo.Length, args.ResourceBytesSize);
        }

        private static void AssertResource(FileInfo fileInfo, Resource resource)
        {
            Assert.Equal(fileInfo.Name, resource.Name);
            Assert.Equal(fileInfo.Directory?.FullName, resource.Path);
            Assert.Equal(fileInfo.Length, resource.Size);
            Assert.NotNull(resource.Hash);
        }
    }
}
