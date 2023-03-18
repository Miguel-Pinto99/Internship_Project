using FluentAssertions.Specialized;
using FluentAssertions;
using Moq;
using Xunit;
using Project1.Application.ApplicationUsers.Queries.GetAllApplicationUser;
using Project1.Persistance;
using Project1.Models;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Queries.GetAllApplicationUser
{
    public class GetAllApplicationUsersHandlerTests
    {
        private readonly Mock<IApplicationUsersRepository> _applicationUserRepositoryMock;
        private readonly GetAllApplicationUserHandler _handler;

        public GetAllApplicationUsersHandlerTests()
        {
            _applicationUserRepositoryMock = new Mock<IApplicationUsersRepository>(MockBehavior.Strict);
            _handler = new GetAllApplicationUserHandler(_applicationUserRepositoryMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<GetAllApplicationUserResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call GetAllApplicationUserAsync on AllApplicationUserRepository")]
        public async Task HandleShouldCallGetAllApplicationUserAsyncOnAllApplicationUserRepository_WhenCommandIsSet()
        {
            // Arrange
            List<ApplicationUser> allApplicationUser = new List<ApplicationUser>
            {
            };

            _applicationUserRepositoryMock
                .Setup(x => x.GetAllApplicationUserAsync(CancellationToken.None))
                .ReturnsAsync(allApplicationUser);

            var command = new GetAllApplicationUserCommand();

            // Act
            GetAllApplicationUserResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.listApplicationUser.Should().NotBeNull();
        }
    }
}
