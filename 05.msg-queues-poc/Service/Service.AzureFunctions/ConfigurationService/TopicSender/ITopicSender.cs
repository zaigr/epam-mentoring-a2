using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace Service.AzureFunctions.ConfigurationService.TopicSender
{
    public interface ITopicSender
    {
        Task SendAsync(Message message);
    }
}
