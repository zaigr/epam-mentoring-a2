using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Service.MessageQueue.Messages.Base;

namespace Service.MessageQueue.Processing
{
    public class MessageHandlerWrapper<TMessage> : IMessageHandlerWrapper
        where TMessage : MessageBase
    {
        private readonly TMessage _message;

        public MessageHandlerWrapper(TMessage message)
        {
            _message = message;
        }

        public async Task HandleAsync(IServiceProvider serviceProvider)
        {
            await serviceProvider.GetService<IMessageHandler<TMessage>>().HandleAsync(_message);
        }
    }
}