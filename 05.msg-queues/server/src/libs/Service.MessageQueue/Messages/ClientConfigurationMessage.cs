using Service.MessageQueue.Messages.Base;
using Service.MessageQueue.Messages.Enums;

namespace Service.MessageQueue.Messages
{
    public class ClientConfigurationMessage : MessageBase
    {
        public override MessageType MessageType => MessageType.ClientConfigurationMessage;

        public int MaxClientMessageSizeBytes { get; set; }
    }
}
