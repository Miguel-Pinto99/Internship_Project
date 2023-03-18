using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Project1.Application.Uns.EventHandlers;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;
using Project1.Timers;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace PVSDashboard.Tests.Application.EventHandlers.UnsEventHandlers
{
    public class PublishLocationEventHandlerTests
    {
        private readonly Mock<IUnsService> _unsServiceMock;
        private readonly Mock<ITimerService> _timerServiceMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PublishLocationEventHandler _handler;

        public PublishLocationEventHandlerTests()
        {
            _unsServiceMock = new Mock<IUnsService>(MockBehavior.Strict);
            _handler = new PublishLocationEventHandler(_unsServiceMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            //Arrange

            Func<Task> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'notification')");
            //Act

            //Assert
        }

        [Fact(DisplayName = "Handle should call PublishWorkPatternEventAsync on ApplicationUserRepository")]
        public async Task HandleShouldCallPublishWorkPatternEventAsyncOnApplicationUserRepository_WhenCommandIsSet()
        {
            // Arrange

            _unsServiceMock.Setup(x => x.PublishLocationAsync(It.IsAny<UsersEachLocation>(), It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new PublishLocationEvent(new UsersEachLocation(1, new List<int>{1,2,3}),1);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _unsServiceMock
                .Verify(x => x.PublishLocationAsync(It.IsAny<UsersEachLocation>(), It.IsAny<int>(), CancellationToken.None), Times.Once);
        }
    }
}
