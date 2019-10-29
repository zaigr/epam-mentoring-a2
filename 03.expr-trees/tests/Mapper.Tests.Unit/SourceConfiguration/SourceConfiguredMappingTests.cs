using System;
using Mapper.Factory;
using Mapper.Tests.Unit.SourceConfiguration.Classes;
using Xunit;

namespace Mapper.Tests.Unit.SourceConfiguration
{
    public class SourceConfiguredMappingTests
    {
        private readonly IMapper _mapper = SetupMapper();

        [Fact]
        public void GivenSource_ThenMappingConfigured_ThenAllPropertiesMapped()
        {
            // Arrange
            var source = new Source
            {
                Id = Guid.NewGuid(),
                FirstName = "First Name",
                Type = 1
            };

            // Act
            var result = _mapper.Map<Source, Dest>(source);

            // Assert
            Assert.Null(result.Id);
            Assert.Null(result.Type);
            Assert.Equal(source.FirstName, result.Name);
        }

        private static IMapper SetupMapper()
        {
            var factory = new MapperFactory();

            return factory.Create();
        }
    }
}
