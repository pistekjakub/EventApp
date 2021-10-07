using EventApp.Services;
using System;
using Xunit;

namespace EventApp.Tests.Services
{
    public class AuthorizationServiceTests
    {
        [Theory]
        [InlineData(null, "a")]
        [InlineData("a", null)]
        [InlineData("", "a")]
        [InlineData("a", "")]
        public void IsEventCreator_ArgumentExceptions(string login, string password) 
        {
            // arrange
            var sut = new AuthorizationService();

            // act && assert
            Assert.Throws<ArgumentException>(() => sut.IsEventCreator(login, password));
        }
    }
}
