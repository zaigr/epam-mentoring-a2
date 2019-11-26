using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Service.Storage
{
    public class AzureStorageClient : IStorageClient
    {
        private readonly CloudBlobContainer _container;

        public AzureStorageClient(string connectionString, string containerName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException(nameof(containerName));
            }

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudBlobClient();

            _container = client.GetContainerReference(containerName);
        }

        public async Task<bool> ExistsAsync(string fileName)
        {
            var blob = _container.GetAppendBlobReference(fileName);

            return await blob.ExistsAsync();
        }

        public async Task CreateOrReplaceAsync(string fileName)
        {
            var blob = _container.GetAppendBlobReference(fileName);

            await blob.CreateOrReplaceAsync();
        }

        public async Task AppendAsync(string fileName, byte[] content)
        {
            var blob = _container.GetAppendBlobReference(fileName);

            await blob.AppendFromByteArrayAsync(content, 0, content.Length);
        }
    }
}
