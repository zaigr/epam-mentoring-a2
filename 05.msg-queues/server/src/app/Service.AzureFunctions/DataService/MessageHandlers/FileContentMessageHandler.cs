using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.MessageQueue.Messages;
using Service.MessageQueue.Processing;
using Service.Storage;

namespace Service.AzureFunctions.DataService.MessageHandlers
{
    public class FileContentMessageHandler : IMessageHandler<FileContentMessage>
    {
        private readonly IStorageClient _storageClient;

        private readonly ILogger _log;

        public FileContentMessageHandler(IStorageClient storageClient, ILogger log)
        {
            _storageClient = storageClient;
            _log = log;
        }

        public async Task HandleAsync(FileContentMessage message)
        {
            var blobName = $"{message.SessionId}_{message.FileName}";

            _log.LogInformation($"Process {nameof(FileContentMessage)} for blob '{blobName}'");

            if (!(await _storageClient.ExistsAsync(blobName)))
            {
                _log.LogDebug($"Blob '{blobName}' not exists. Create or replace blob.");

                await _storageClient.CreateOrReplaceAsync(blobName);
            }

            _log.LogDebug($"Append content to '{blobName}'.");

            await _storageClient.AppendAsync(blobName, message.Content);
        }
    }
}
