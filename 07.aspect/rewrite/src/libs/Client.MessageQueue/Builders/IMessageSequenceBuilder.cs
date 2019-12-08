using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Client.MessageQueue.Messages.Base;

namespace Client.MessageQueue.Builders
{
    public interface IMessageSequenceBuilder
    {
        Task<IEnumerable<TMessage>> CreateSequenceAsync<TMessage>(Stream contentStream, int messageSizeBytes)
            where TMessage : ContentMessageBase, new();
    }
}
