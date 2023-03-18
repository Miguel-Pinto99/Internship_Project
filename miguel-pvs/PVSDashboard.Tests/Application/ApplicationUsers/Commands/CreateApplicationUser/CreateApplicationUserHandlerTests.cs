using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using Project1.Persistance;
using Project1.Application.ApplicationUsers.Commands.CreateApplicationUser;
using Project1.Events.UnsLogicEvents;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreatreApplicationUserTests
    {
        private readonly Mock<IApplicationUsersRepository> _applicationUserRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CreateApplicationUserHandler _handler;

        public CreatreApplicationUserTests()
        {
            _applicationUserRepositoryMock = new Mock<IApplicationUsersRepository>(MockBehavior.Strict);
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _handler = new CreateApplicationUserHandler(_applicationUserRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<CreateApplicationUserResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call AbsentLogicEventAsync on ApplicationUserRepository")]
        public async Task HandleShouldCallAbsentLogicEventAsyncOnApplicationUserRepository_WhenCommandIsSet()
        {
            // Arrange
            var applicationUser = new Project1.Models.ApplicationUser
            {
                Id = 1,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 1,
                WorkPatterns = new List<Project1.Models.WorkPattern>()
            };

            _applicationUserRepositoryMock
                .Setup(x => x.CreateApplicationUserAsync(It.IsAny<Project1.Models.ApplicationUser>(), CancellationToken.None))
                .ReturnsAsync(applicationUser);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<CreateApplicationUserLogicEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new CreateApplicationUserCommand(1, new CreateApplicationUserCommandBody
            {
                FirstName = "Miguel",
                OfficeLocation = 1
            });

            // Act
            CreateApplicationUserResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.ApplicationUser.Should().NotBeNull();

            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<CreateApplicationUserLogicEvent>(), CancellationToken.None), Times.Once);

            _applicationUserRepositoryMock
                .Verify(x => x.CreateApplicationUserAsync(It.IsAny<Project1.Models.ApplicationUser>(), CancellationToken.None), Times.Once);
        }
    }
}
