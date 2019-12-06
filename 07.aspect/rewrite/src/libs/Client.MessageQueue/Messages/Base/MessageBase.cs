using Client.MessageQueue.Messages.Enums;

namespace Client.MessageQueue.Messages.Base
{
    public abstract class MessageBase
    {
        public abstract MessageType MessageType { get; }

        public string SessionId { get; set; }
    }
}
