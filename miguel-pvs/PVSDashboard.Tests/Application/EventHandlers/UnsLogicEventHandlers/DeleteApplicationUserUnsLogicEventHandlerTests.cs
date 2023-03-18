using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Application.ApplicationUsers.Queries.GetLocation;
using Project1.Application.Uns.UnsLogicEventHandlers;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;
using Project1.Infrastructure;
using Project1.Models;
using Xunit;

namespace PVSDashboard.Tests.Application.EventHandlers.UnsLogicEventHandlers
{
    public class DeleteApplicationUserUnsLogicEventHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly DeleteApplicationUserUnsLogicEventHandler _handler;

        public DeleteApplicationUserUnsLogicEventHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _handler = new DeleteApplicationUserUnsLogicEventHandler(_mediatorMock.Object);
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

        [Fact(DisplayName = "Handle should call DeleteWorkPatternUnsLogicEventAsync on WorkPatternRepository")]
        public async Task HandleShouldCallDeleteWorkPatternUnsLogicEventAsyncOnWorkPatternRepository_WhenCommandIsSet()
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

            _mediatorMock.Setup(x => x.Publish(It.IsAny<DeleteTopicApplicationUserEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<PublishLocationEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<RemoveTimerEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new DeleteApplicationUserLogicEvent(applicationUser,usersEachLocation);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<DeleteTopicApplicationUserEvent>(), CancellationToken.None), Times.Once);
            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<PublishLocationEvent>(), CancellationToken.None), Times.Once);
            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<RemoveTimerEvent>(), CancellationToken.None), Times.Once);
        }
    }
}
