using System;
using Client.MessageQueue.Messages.Base;
using Client.MessageQueue.Messages.Enums;

namespace Client.MessageQueue.Messages
{
    public class ClientAvailabilityMessage : MessageBase
    {
        public override MessageType MessageType => MessageType.ClientAvailabilityMessage;

        public string ClientId { get; set; }

        public ClientServiceStatus Status { get; set; }

        public string Message { get; set; }

        public DateTimeOffset TimeStamp { get; set; }
    }
}
