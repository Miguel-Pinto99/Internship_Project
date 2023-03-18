using FluentAssertions.Specialized;
using FluentAssertions;
using Moq;
using Project1.Events.UnsEvents;
using Project1.Models;
using Project1.Timers;
using Xunit;
using Project1.Application.EventHandlers.UnsEventHandlers;

namespace PVSDashboard.Tests.Application.EventHandlers.UnsEventHandlers
{
    public class StopTimerEventHandlerEventHandler
    {
        private readonly Mock<ITimerService> _timerServiceMock;
        private readonly StopTimerEventHandler _handler;

        public StopTimerEventHandlerEventHandler()
        {
            _timerServiceMock = new Mock<ITimerService>(MockBehavior.Strict);
            _handler = new StopTimerEventHandler(_timerServiceMock.Object);
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

        [Fact(DisplayName = "Handle should call StopTimerEventAsync on TimerService")]
        public async Task HandleShouldCallStopTimerEventAsyncOnTimerService_WhenCommandIsSet()
        {
            // Arrange

            var listWorkPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = 0
                },
            };

            var listWorkPatterns = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = 1,
                    Id = new Guid(),
                    StartDate = new DateTime(2022,1,1,0,0,0),
                    EndDate = new DateTime(2022,1,1,0,0,0),
                    Parts = listWorkPatternParts
                },
            };

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 1,
                WorkPatterns = listWorkPatterns
            };



            _timerServiceMock.Setup(x => x.ReinitializeTimersAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new StopTimerEvent(applicationUser);
            ;

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _timerServiceMock
                .Verify(x => x.ReinitializeTimersAsync(It.IsAny<ApplicationUser>(), CancellationToken.None), Times.Once);
        }
    }
}
