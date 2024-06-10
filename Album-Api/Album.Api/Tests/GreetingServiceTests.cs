using Xunit;
using Album.Api.Services;

namespace Album.Api.Tests
{
    public class GreetingServiceTests
    {
        private readonly GreetingService _greetingService;

        public GreetingServiceTests()
        {
            _greetingService = new GreetingService();
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
