using System;
using Service.MessageQueue.Messages.Base;
using Service.MessageQueue.Messages.Enums;

namespace Service.MessageQueue.Messages
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
