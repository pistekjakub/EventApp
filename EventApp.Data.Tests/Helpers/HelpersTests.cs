using Xunit;

namespace EventApp.Data.Tests.Helpers
{
    public class HelpersTests
    {
        [Theory]
        [InlineData(" hello", "hello")]
        [InlineData("hello ", "hello")]
        [InlineData("", "")]
        [InlineData("  ", "")]
        [InlineData(null, null)]
        public void TrimSafeTests(string input, string expectedOutput) 
        {            
            // arrange & act
            string result = EventApp.Data.Helpers.TrimSafe(input);

            // assert
            Assert.Equal(expectedOutput, result);
        }
    }
}
