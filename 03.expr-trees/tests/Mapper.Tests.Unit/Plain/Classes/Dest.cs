using System;

namespace Mapper.Tests.Unit.Plain.Classes
{
    public class Dest
    {
        public int Id { get; set; }

        public string Name { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string Type { get; set; }

        public decimal Cost { get; set; }
    }
}
