using System.Threading.Tasks;
using Service.MessageQueue.Messages.Base;

namespace Service.MessageQueue.Processing
{
    public interface IMessageHandler<in TMessage>
        where TMessage : MessageBase
    {
        Task HandleAsync(TMessage message);
    }
}