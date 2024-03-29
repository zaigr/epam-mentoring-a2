﻿using System;
using System.Text;
using Client.MessageQueue.Messages;
using Client.MessageQueue.Messages.Base;
using Client.MessageQueue.Messages.Enums;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Client.MessageQueue.Extensions
{
    public static class MessageExtensions
    {
        public static MessageBase Read(this Message message)
        {
            if (!message.UserProperties.TryGetValue("MessageType", out var messageTypeObject) ||
                (messageTypeObject == null))
            {
                throw new InvalidOperationException($"User property 'MessageType' should be defined to process the message.");
            }

            if (!Enum.TryParse<MessageType>(messageTypeObject.ToString(), out var messageType))
            {
                throw new InvalidOperationException($"Unable to find message type '{messageTypeObject}'.");
            }

            switch (messageType)
            {
                case MessageType.FileContentMessage:
                    return DeserializeMessage<FileContentMessage>(Encoding.UTF8.GetString(message.Body));
                case MessageType.ClientConfigurationMessage:
                    return DeserializeMessage<ClientConfigurationMessage>(Encoding.UTF8.GetString(message.Body));
                default:
                    throw new InvalidOperationException($"Message type '{messageType}' could not be deserialized.");
            }
        }

        private static T DeserializeMessage<T>(string message)
            where T : MessageBase
        {
            return JsonConvert.DeserializeObject<T>(
                message,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}
