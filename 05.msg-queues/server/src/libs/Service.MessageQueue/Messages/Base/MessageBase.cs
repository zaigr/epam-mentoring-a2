using Service.MessageQueue.Messages.Enums;

namespace Service.MessageQueue.Messages.Base
{
    public abstract class MessageBase
    {
        public abstract MessageType MessageType { get; }

        public string SessionId { get; set; }
    }
}
