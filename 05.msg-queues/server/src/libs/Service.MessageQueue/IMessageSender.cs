using System.Threading.Tasks;
using Service.MessageQueue.Messages.Base;

namespace Service.MessageQueue
{
    public interface IMessageSender
    {
        Task SendAsync(MessageBase message);
    }
}
