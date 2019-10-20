using System;
using System.Collections.Generic;
using Mapper.Factory;
using Mapper.Tests.Unit.Plain.Classes;
using Xunit;

namespace Mapper.Tests.Unit.Plain
{
    public class PlainMappingTests
    {
        private readonly IMapper _mapper = SetupMapper();

        [Fact]
        public void GivenTypes_WhenMappingNotConfigured_ThenExceptionRaised()
        {
            // Arrange
            var mapperFactory = new MapperFactory();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => mapperFactory.Create<Foo, Bar>());
        }

        [Theory]
        [MemberData(nameof(PlainMappingTestMemberData))]
        public void GivenNotEmptyDest_WhenAllSourceValuesSet_ThenAllValuesMappedToDest(Source source)
        {
            // Arrange
            var dest = new Dest { Id = 8, CreatedAt = DateTime.MaxValue };

            // Act
            var result = _mapper.Map(source, dest);

            // Assert
            Assert.Equal(source.Id, result.Id);
            Assert.Equal(source.Name, result.Name);
            Assert.Equal(source.CreatedAt, result.CreatedAt);
            Assert.Equal(default, result.Cost);
            Assert.Null(result.Type);
        }

        public static IEnumerable<object[]> PlainMappingTestMemberData()
        {
            return new List<object[]>
            {
                new[] { new Source { Id = 3, Name = "My-name", Cost = 10, CreatedAt = DateTime.Now }, },
                new[] { new Source { Id = 3, Name = null, Cost = 1 }, },
                new[] { new Source { }, },
            };
        }

        private static IMapper SetupMapper()
        {
            var mapperFactory = new MapperFactory();

            return mapperFactory.Create<Source, Dest>();
        }
    }
}
