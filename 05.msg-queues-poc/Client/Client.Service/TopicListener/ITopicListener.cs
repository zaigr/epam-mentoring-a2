using System;
using Microsoft.Azure.ServiceBus;

namespace Client.Service.TopicListener
{
    public interface ITopicListener
    {
        void StartListener(Action<Message> handler);
    }
}
