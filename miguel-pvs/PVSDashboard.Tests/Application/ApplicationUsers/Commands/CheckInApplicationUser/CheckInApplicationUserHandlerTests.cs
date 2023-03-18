using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using Project1.Persistance;
using Project1.Application.ApplicationUsers.Queries.CheckInApplicationUser;
using Project1.Events.UnsEvents;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Commands.CheckInApplicationUser
{
    public class CheckInApplicationUserHandlerTests
    {
        private readonly Mock<IApplicationUsersRepository> _applicationUserRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CheckInApplicationUserHandler _handler;

        public CheckInApplicationUserHandlerTests()
        {
            _applicationUserRepositoryMock = new Mock<IApplicationUsersRepository>(MockBehavior.Strict);
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _handler = new CheckInApplicationUserHandler(_applicationUserRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<CheckInApplicationUserResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call CheckInApplicationUserAsync on ApplicationUserRepository")]
        public async Task HandleShouldCallCheckInApplicationUserAsyncOnApplicationUserRepository_WhenCommandIsSet()
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
                .Setup(x => x.CheckInApplicationUserAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(applicationUser);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<PublishCheckInEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new CheckInApplicationUserCommand(1);
            // Act
            CheckInApplicationUserResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.ApplicationUser.Should().NotBeNull();

            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<PublishCheckInEvent>(), CancellationToken.None), Times.Once);

            _applicationUserRepositoryMock
                .Verify(x => x.CheckInApplicationUserAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);
        }
    }
}
