using System;
using System.Threading.Tasks;
using Client.MessageQueue.Messages.Base;

namespace Client.MessageQueue
{
    public interface IMessageReceiver
    {
        void RegisterHandler<TMessage>(Func<TMessage, Task> handler, Action<Exception> exceptionHandler)
            where TMessage : MessageBase;
    }
}
