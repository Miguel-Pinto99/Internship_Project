using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Project1.Application.ApplicationUsers.Queries.GetLocation;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Timers;
using Xunit;
using Project1.Application.EventHandlers.UnsLogicEventHandlers;

namespace PVSDashboard.Tests.Application.EventHandlers.UnsLogicEventHandlers
{
    public class EditApplicationUserUnsLogicEventHandlerTests
    {

        private readonly Mock<IMediator> _mediatorMock;
        private readonly EditApplicationUserUnsLogicEventHandler _handler;

        public EditApplicationUserUnsLogicEventHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _handler = new EditApplicationUserUnsLogicEventHandler(_mediatorMock.Object);
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

        [Fact(DisplayName = "Handle should call EditApplicationUserLogicEventAsync on ApplicationUserRepository")]
        public async Task HandleShouldCallEditApplicationUserLogicEventAsyncOnApplicationUserRepository_WhenCommandIsSet()
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

            var applicationUser1 = new ApplicationUser
            {
                Id = 1,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 1,
                WorkPatterns = listWorkPatterns
            };
            var applicationUser2 = new ApplicationUser
            {
                Id = 2,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 2,
                WorkPatterns = listWorkPatterns
            };

            var listAllEditApplicationUsers = new List<Project1.Models.WorkPattern>
            {
                new Project1.Models.WorkPattern
                {
                    UserId= 1,
                    Id = new Guid(),
                    StartDate = DateTime.Now,

                }
            };

            UsersEachLocation usersEachLocation1 = new UsersEachLocation(1, new List<int> { 1, 2, 3 });
            UsersEachLocation usersEachLocation2 = new UsersEachLocation(2, new List<int> { 3, 2, 1 });

            _mediatorMock.Setup(x => x.Publish(It.IsAny<DeleteTopicApplicationUserEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetLocationCommand>(), CancellationToken.None))
                .ReturnsAsync(new GetLocationResponse { UserEachLocation = usersEachLocation1 });

            _mediatorMock.Setup(x => x.Publish(It.IsAny<PublishLocationEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<PublishCheckInEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<DeleteTopicApplicationUserEvent>(), CancellationToken.None))
                 .Returns(Task.CompletedTask);

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetLocationCommand>(), CancellationToken.None))
                .ReturnsAsync(new GetLocationResponse { UserEachLocation = usersEachLocation2 });

            _mediatorMock.Setup(x => x.Publish(It.IsAny<PublishLocationEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<PublishCheckInEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<PublishWorkPatternEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<StopTimerEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new EditApplicationUserLogicEvent(applicationUser1,applicationUser2);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<DeleteTopicApplicationUserEvent>(), CancellationToken.None), Times.Exactly(2));
            _mediatorMock
                .Verify(x => x.Send(It.IsAny<GetLocationCommand>(), CancellationToken.None),Times.Exactly(2));
            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<PublishLocationEvent>(), CancellationToken.None),Times.Exactly(2));
            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<PublishCheckInEvent>(), CancellationToken.None), Times.Once);
            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<PublishWorkPatternEvent>(), CancellationToken.None), Times.Once);
            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<StopTimerEvent>(), CancellationToken.None), Times.Once);
        }
    }
}
