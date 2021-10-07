using EventApp.Data.Dtos;
using EventApp.Data.Repositories;
using EventApp.Models.Requests;
using EventApp.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EventApp.Tests.Services
{
    public class RegistrationServiceTests
    {
        [Fact]
        public async Task Registration_Inserted() 
        {
            // arrange
            var registrationRepositoryMock = new Mock<IRegistrationRepository>();
            registrationRepositoryMock.Setup(erm => erm.InsertRegistration(It.IsAny<RegistrationDto>(), It.IsAny<string>())).ReturnsAsync(new RegistrationDto { Id = 13 });
            registrationRepositoryMock.Setup(erm => erm.CheckUniqueRegistration(It.IsAny<RegistrationDto>(), It.IsAny<string>())).ReturnsAsync(true);

            var sut = new RegistrationService(registrationRepositoryMock.Object);
            var insertRegistrationRequest = new InsertRegistrationRequest{ Name = "   test_name   ", Email = " testemail@mail.com ", EventName = " test event name ", Phone = " 888 555 666 " };

            // act
            var result = await sut.InsertRegistration(insertRegistrationRequest);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Inserted);
        }
    }
}
