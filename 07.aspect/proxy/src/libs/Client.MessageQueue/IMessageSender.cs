using System.Threading.Tasks;
using Client.MessageQueue.Messages;
using Client.MessageQueue.Messages.Base;

namespace Client.MessageQueue
{
    public interface IMessageSender
    {
        Task SendAsync(MessageBase message);
    }
}
