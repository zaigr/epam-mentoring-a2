using System;

namespace Service.Data.Entities
{
    public class ClientAvailability
    {
        public int Id { get; set; }

        public string ClientId { get; set; }

        public string Status { get; set; }

        public string Message { get; set; }

        public DateTimeOffset TimeStamp { get; set; }
    }
}
