using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Project1.Application.Absent.Queries.GetAllAbsent;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Timers;
using Xunit;
using Project1.Application.Uns.UnsLogicEventHandlers;

namespace PVSDashboard.Tests.Application.EventHandlers.UnsEventHandlers
{
    public class UpdateTimerEventHandlerEventHandler
    {
        private readonly Mock<IUnsService> _unsServiceMock;
        private readonly Mock<ITimerService> _timerServiceMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UpdateTimerEventHandler _handler;

        public UpdateTimerEventHandlerEventHandler()
        {
            _timerServiceMock = new Mock<ITimerService>(MockBehavior.Strict);
            _handler = new UpdateTimerEventHandler(_timerServiceMock.Object);
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

        [Fact(DisplayName = "Handle should call UpdateTimerEventAsync on TimerService")]
        public async Task HandleShouldCallUpdateTimerEventAsyncOnTimerService_WhenCommandIsSet()
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



            _timerServiceMock.Setup(x => x.CalculateDelayAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new UpdateTimerEvent(applicationUser);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _timerServiceMock
                .Verify(x => x.CalculateDelayAsync(It.IsAny<ApplicationUser>(), CancellationToken.None), Times.Once);
        }
    }
}