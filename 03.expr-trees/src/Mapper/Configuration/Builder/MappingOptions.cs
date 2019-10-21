namespace Mapper.Configuration.Builder
{
    public class MappingOptions
    {
        internal MappingOptions()
        {
        }

        internal bool IsPropIgnored { get; private set; }

        public void Ignore()
        {
            IsPropIgnored = true;
        }
    }
}
