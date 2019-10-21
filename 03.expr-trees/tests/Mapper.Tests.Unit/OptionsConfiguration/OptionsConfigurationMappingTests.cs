using Mapper.Factory;
using Mapper.Tests.Unit.OptionsConfiguration.Classes;
using Xunit;

namespace Mapper.Tests.Unit.OptionsConfiguration
{
    public class OptionsConfigurationMappingTests
    {
        private readonly IMapper _mapper = SetupMapper();

        [Fact]
        public void GivenSameProps_WhenIgnoreOptionSet_ThenPropsNotMapped()
        {
            // Arrange
            var source = new Source
            {
                Identified = 100,
                Name = "source-name",
                Type = "type2"
            };

            var destName = "dest-name";
            var dest = new Dest { Name = destName, Type = "type1" };

            // Act
            var result = _mapper.Map<Source, Dest>(source, dest);

            // Assert
            Assert.Equal(source.Identified, result.Id);
            Assert.Equal(destName, result.Name);
            Assert.Equal(source.Type, dest.Type);
        }

        private static IMapper SetupMapper()
        {
            var factory = new MapperFactory();

            return factory.Create();
        }
    }
}
