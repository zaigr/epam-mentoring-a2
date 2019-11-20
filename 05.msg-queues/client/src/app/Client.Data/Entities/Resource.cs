namespace Client.Data.Entities
{
    public class Resource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public long Size { get; set; }

        public string Hash { get; set; }
    }
}
