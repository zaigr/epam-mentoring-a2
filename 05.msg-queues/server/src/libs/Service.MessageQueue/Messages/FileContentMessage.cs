using Service.MessageQueue.Messages.Base;
using Service.MessageQueue.Messages.Enums;

namespace Service.MessageQueue.Messages
{
    public class FileContentMessage : ContentMessageBase
    {
        public override MessageType MessageType => MessageType.FileContentMessage;

        public string FileName { get; set; }
    }
}
