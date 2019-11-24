using System.Threading.Tasks;
using Service.MessageQueue.Messages.Base;

namespace Service.MessageQueue.Processing
{
    public interface IMessageProcessor
    {
        Task HandleAsync<TMessage>(TMessage message)
            where TMessage : MessageBase;
    }
}