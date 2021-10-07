using System;
using Xunit;
using Moq;
using EventApp.Data.Repositories;
using EventApp.Data.Dtos;
using EventApp.Services;
using EventApp.Models.Requests;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;

namespace EventApp.Tests.Services
{
    public class EventServiceTests
    {
        [Fact]
        public async Task InsertEventTest_Inserted()
        {
            // arrange
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(erm => erm.InsertEvent(It.IsAny<EventDto>())).ReturnsAsync(new EventDto { Id = 13 });
            eventRepositoryMock.Setup(erm => erm.CheckUniqueEvent(It.IsAny<string>())).ReturnsAsync(true);
            var sut = new EventService(eventRepositoryMock.Object);
            var insertEventRequest = new InsertEventRequest { Name = "test_insert", Description = "test description", Location = "Munich", StartTime = new DateTime(2020, 9, 9), EndTime = new DateTime(2021, 10, 10) };

            // act
            var result = await sut.InsertEvent(insertEventRequest);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Inserted);
        }

        [Fact]
        public async Task InsertEventTest_Not_Unique()
        {
            // arrange
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(erm => erm.CheckUniqueEvent(It.IsAny<string>())).ReturnsAsync(false);
            var sut = new EventService(eventRepositoryMock.Object);
            var insertEventRequest = new InsertEventRequest { Name = "test_insert", Description = "test description", Location = "Munich", StartTime = new DateTime(2020, 9, 9), EndTime = new DateTime(2021, 10, 10) };

            // act && assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.InsertEvent(insertEventRequest));
        }

        [Fact]
        public async Task GetEvents_Any()
        {
            // arrange
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(erm => erm.GetEvents()).ReturnsAsync(
                new List<EventDto> {
                    new EventDto {
                        Id = 1,
                        Name = "Some Event 1",
                        Description = "Test Description 1",
                        StartTime = new DateTime(1990, 10, 10),
                        EndTime = new DateTime(2000, 10, 10),
                        Location = "Munich",

                    },
                    new EventDto {
                        Id = 1,
                        Name = "Some Event 2",
                        Description = "Test Description 2",
                        StartTime = new DateTime(1990, 3, 5),
                        EndTime = new DateTime(2000, 3, 5),
                        Location = "Berlin",
                    }
              });

            var sut = new EventService(eventRepositoryMock.Object);
            var request = new GetEventsRequest();

            // act
            var result = await sut.GetEvents(request);

            // assert
            result.Should().NotBeNull();
            result.Events.Should().NotBeNull().And.HaveCount(2);

            Assert.Equal("Some Event 1", result.Events[0].Name);
            Assert.Equal("Test Description 1", result.Events[0].Description);
            Assert.Equal(new DateTime(1990, 10, 10), result.Events[0].StartTime);
            Assert.Equal(new DateTime(2000, 10, 10), result.Events[0].EndTime);
            Assert.Equal("Munich", result.Events[0].Location);

            Assert.Equal("Some Event 2", result.Events[1].Name);
            Assert.Equal("Test Description 2", result.Events[1].Description);
            Assert.Equal(new DateTime(1990, 3, 5), result.Events[1].StartTime);
            Assert.Equal(new DateTime(2000, 3, 5), result.Events[1].EndTime);
            Assert.Equal("Berlin", result.Events[1].Location);
        }

        [Fact]
        public async Task GetEvents_Empty()
        {
            // arrange
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(erm => erm.GetEvents()).ReturnsAsync(new List<EventDto>());

            var sut = new EventService(eventRepositoryMock.Object);
            var request = new GetEventsRequest();

            // act
            var result = await sut.GetEvents(request);

            // assert
            result.Should().NotBeNull();
            result.Events.Should().NotBeNull().And.BeEmpty();
        }
    }
}
