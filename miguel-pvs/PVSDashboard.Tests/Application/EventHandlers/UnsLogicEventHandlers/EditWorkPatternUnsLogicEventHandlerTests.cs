using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Application.Uns.UnsLogicEventHandlers;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;
using Project1.Models;
using Project1.Timers;
using Xunit;

namespace PVSDashboard.Tests.Application.EventHandlers.UnsLogicEventHandlers
{
    public class EditWorkPatternUnsLogicEventHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly EditWorkPatternUnsLogicEventHandler _handler;

        public EditWorkPatternUnsLogicEventHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _handler = new EditWorkPatternUnsLogicEventHandler(_mediatorMock.Object);
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

        [Fact(DisplayName = "Handle should call EditWorkPatternUnsLogicEventAsync on WorkPatternRepository")]
        public async Task HandleShouldCallEditWorkPatternUnsLogicEventAsyncOnWorkPatternRepository_WhenCommandIsSet()
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

            var listAllAbsents = new List<Project1.Models.Absent>
            {
                new Project1.Models.Absent
                {
                    UserId= 1,
                    Id = new Guid(),
                    StartDate = DateTime.Now,

                }
            };

            UsersEachLocation usersEachLocation = new UsersEachLocation(1, new List<int> { 1, 2, 3 });

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetApplicationUserCommand>(), CancellationToken.None))
                .ReturnsAsync(new GetApplicationUserResponse { ApplicationUser = applicationUser });

            _mediatorMock.Setup(x => x.Publish(It.IsAny<PublishWorkPatternEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<StopTimerEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new EditWorkPatternLogicEvent(listWorkPatterns[0]);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorMock
                .Verify(x => x.Send(It.IsAny<GetApplicationUserCommand>(), CancellationToken.None), Times.Once);
            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<PublishWorkPatternEvent>(), CancellationToken.None), Times.Once);
            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<StopTimerEvent>(), CancellationToken.None), Times.Once);
        }
    }
}
