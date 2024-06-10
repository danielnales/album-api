using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Album.Api.Services;

namespace Album.Api.Tests
{
    public class GreetingServiceTests
    {
        private readonly GreetingService _greetingService;
        private readonly Mock<ILogger<GreetingService>> _mockLogger;

        public GreetingServiceTests()
        {
            _mockLogger = new Mock<ILogger<GreetingService>>();
            _greetingService = new GreetingService(_mockLogger.Object);
        }

        [Fact]
        public void GetGreeting_WithName_ReturnsGreetingWithName()
        {
            var result = _greetingService.GetGreeting("John");
            Assert.Equal("Hello, John!", result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetGreeting_NullOrEmptyOrWhitespace_ReturnsHelloWorld(string name)
        {
            var result = _greetingService.GetGreeting(name);
            Assert.Equal("Hello, World!", result);
        }
    }
}
