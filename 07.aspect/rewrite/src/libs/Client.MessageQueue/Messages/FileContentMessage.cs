using Client.MessageQueue.Messages.Base;
using Client.MessageQueue.Messages.Enums;

namespace Client.MessageQueue.Messages
{
    public class FileContentMessage : ContentMessageBase
    {
        public override MessageType MessageType => MessageType.FileContentMessage;

        public string FileName { get; set; }
    }
}
