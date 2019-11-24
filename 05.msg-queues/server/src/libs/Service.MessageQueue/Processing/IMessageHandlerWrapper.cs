using System;
using System.Threading.Tasks;

namespace Service.MessageQueue.Processing
{
    public interface IMessageHandlerWrapper
    {
        Task HandleAsync(IServiceProvider serviceProvider);
    }
}