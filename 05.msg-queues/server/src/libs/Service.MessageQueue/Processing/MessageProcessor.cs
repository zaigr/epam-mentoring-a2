using System;
using System.Threading.Tasks;
using Service.MessageQueue.Messages.Base;

namespace Service.MessageQueue.Processing
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public MessageProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task HandleAsync<TMessage>(TMessage message)
            where TMessage : MessageBase
        {
            var wrapperType = typeof(MessageHandlerWrapper<>).MakeGenericType(message.GetType());
            await ((IMessageHandlerWrapper)Activator.CreateInstance(wrapperType, message)).HandleAsync(_serviceProvider);
        }
    }
}