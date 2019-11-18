using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Client.Service.QueueSender
{
    public interface IQueueSender
    {
        Task SendAsync(Message message);
    }
}
