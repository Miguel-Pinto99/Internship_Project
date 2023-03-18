using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using Project1.Persistance;
using Project1.Application.ApplicationUsers.Queries.CheckOutApplicationUser;
using Project1.Events.UnsEvents;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Commands.CheckOutApplicationUser
{
    public class CheckOutApplicationUserHandlerTests
    {
        private readonly Mock<IApplicationUsersRepository> _applicationUserRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CheckOutApplicationUserHandler _handler;

        public CheckOutApplicationUserHandlerTests()
        {
            _applicationUserRepositoryMock = new Mock<IApplicationUsersRepository>(MockBehavior.Strict);
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _handler = new CheckOutApplicationUserHandler(_applicationUserRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<CheckOutApplicationUserResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call CheckOutApplicationUserAsync on ApplicationUserRepository")]
        public async Task HandleShouldCallCheckOutApplicationUserAsyncOnApplicationUserRepository_WhenCommandIsSet()
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
                .Setup(x => x.CheckOutApplicationUserAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(applicationUser);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<PublishCheckInEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new CheckOutApplicationUserCommand(1);

            // Act
            CheckOutApplicationUserResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.ApplicationUser.Should().NotBeNull();

            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<PublishCheckInEvent>(), CancellationToken.None), Times.Once);

            _applicationUserRepositoryMock
                .Verify(x => x.CheckOutApplicationUserAsync(It.IsAny<int>(),CancellationToken.None), Times.Once);
        }
    }
}
