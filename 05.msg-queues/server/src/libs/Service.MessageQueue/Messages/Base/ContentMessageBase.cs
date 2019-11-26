namespace Service.MessageQueue.Messages.Base
{
    public abstract class ContentMessageBase : MessageBase
    {
        public byte[] Content { get; set; }
    }
}
