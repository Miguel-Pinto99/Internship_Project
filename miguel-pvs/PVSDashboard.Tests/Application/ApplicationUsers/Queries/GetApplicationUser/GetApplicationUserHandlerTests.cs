using FluentAssertions.Specialized;
using FluentAssertions;
using Moq;
using Project1.Persistance;
using Xunit;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using MediatR;
using Project1.Models;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Queries.GetApplicationUser
{
    public class GetApplicationUserHandlerTests
    {
        private readonly Mock<IApplicationUsersRepository> _applicationUserRepositoryMock;
        private readonly GetApplicationUserHandler _handler;

        public GetApplicationUserHandlerTests()
        {
            _applicationUserRepositoryMock = new Mock<IApplicationUsersRepository>(MockBehavior.Strict);
            _handler = new GetApplicationUserHandler(_applicationUserRepositoryMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<GetApplicationUserResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call GetApplicationUserAsync on ApplicationUserRepository")]
        public async Task HandleShouldCallGetApplicationUserAsyncOnApplicationUserRepository_WhenCommandIsSet()
        {
            // Arrange
            var applicationUser = new ApplicationUser
            {

            };

            _applicationUserRepositoryMock
                .Setup(x => x.GetApplicationUserAsync(2, CancellationToken.None))
                .ReturnsAsync(applicationUser);

            var command = new GetApplicationUserCommand(2);

            // Act
            GetApplicationUserResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.ApplicationUser.Should().NotBeNull();

            _applicationUserRepositoryMock
                .Verify(x => x.GetApplicationUserAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);
        }
    }
}
