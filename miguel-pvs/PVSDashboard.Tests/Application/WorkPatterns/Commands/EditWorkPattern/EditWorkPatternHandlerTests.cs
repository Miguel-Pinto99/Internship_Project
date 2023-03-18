using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using Project1.Application.WorkPatterns.Commands.EditWorkPattern;
using Project1.Persistance;
using Project1.Events.UnsLogicEvents;
using Project1.Application.WorkPatterns.Commands.CreateWorkPattern;
using Project1.Infrastructure;
using Project1.Application.ApplicationUsers.Queries.EditWorkPatterm;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Events.UnsEvents;

namespace PVSDashboard.Tests.Application.WorkPatterns.Commands.EditWorkPattern
{
    public class EditWorkPatternHandlerTests
    {
        private readonly Mock<IWorkPatternRepository> _workPatternRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUnsService> _unsServiceMock;
        private readonly EditWorkPatternHandler _handler;

        public EditWorkPatternHandlerTests()
        {
            _workPatternRepositoryMock = new Mock<IWorkPatternRepository>(MockBehavior.Strict);
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _handler = new EditWorkPatternHandler(_workPatternRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<EditWorkPatternResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call UpdateWorkPatternAsync on WorkPatternRepository")]
        public async Task HandleShouldCallEditWorkPatternAsyncOnWorkPatternRepository_WhenCommandIsSet()
        {
            // Arrange

            var workPattern = new Project1.Models.WorkPattern
            {
                UserId = 1,
                Id = new Guid(),
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),

            };

            var applicationUser = new Project1.Models.ApplicationUser
            {
                Id = 1,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 1,
                WorkPatterns = new List<Project1.Models.WorkPattern>{workPattern}
            };


            _workPatternRepositoryMock
                .Setup(x => x.UpdateWorkPatternAsync(It.IsAny<Project1.Models.WorkPattern>(), CancellationToken.None))
                .ReturnsAsync(workPattern);

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetApplicationUserCommand>(), CancellationToken.None))
                .ReturnsAsync(new GetApplicationUserResponse { ApplicationUser = applicationUser });

            _mediatorMock.Setup(x => x.Publish(It.IsAny<PublishWorkPatternEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new EditWorkPatternCommand(new Guid(), new EditWorkPatternCommandBody
            {
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
                Parts = new List<Project1.Models.WorkPatternPart>()
            });

            // Act
            EditWorkPatternResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.WorkPattern.Should().NotBeNull();

            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<PublishWorkPatternEvent>(), CancellationToken.None), Times.Once);
            _mediatorMock
                 .Verify(x => x.Send(It.IsAny<GetApplicationUserCommand>(), CancellationToken.None), Times.Once);
            _workPatternRepositoryMock
                .Verify(x => x.UpdateWorkPatternAsync(It.IsAny<Project1.Models.WorkPattern>(),CancellationToken.None), Times.Once);
        }
    }
}
