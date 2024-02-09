using Contoso.Pizza.Data.Contracts;
using Contoso.Pizza.Data.Models;
using Moq;
using System.Reflection.Metadata;
using Xunit;

namespace Contoso.Pizza.Data.Tests
{
    public class SauceRepositoryTests
    {
        private readonly Mock<ISauceRepository> _mockRepo;

        public SauceRepositoryTests()
        {
            _mockRepo = new Mock<ISauceRepository>();
        }

        [Fact]
        public async Task GetAll_ReturnsAllSauces()
        {
            // Arrange
            var sauce1Id = Guid.NewGuid();
            var expectedSauces = new List<Sauce> { new Sauce() { Id = sauce1Id, Name = "Sauce1", Created = DateTime.UtcNow } }; 
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedSauces);

            // Act
            var result = await _mockRepo.Object.GetAllAsync();

            // Assert
            Assert.Equal(expectedSauces, result);
        }
    }
}
