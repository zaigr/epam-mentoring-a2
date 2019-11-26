using Client.MessageQueue.Messages.Base;
using Client.MessageQueue.Messages.Enums;

namespace Client.MessageQueue.Messages
{
    public class ClientConfigurationMessage : MessageBase
    {
        public override MessageType MessageType => MessageType.ClientConfigurationMessage;

        public int MaxClientMessageSizeBytes { get; set; }
    }
}
