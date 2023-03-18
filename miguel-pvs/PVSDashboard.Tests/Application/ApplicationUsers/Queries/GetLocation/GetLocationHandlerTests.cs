using FluentAssertions.Specialized;
using FluentAssertions;
using Moq;
using Xunit;
using Project1.Application.ApplicationUsers.Queries.GetLocation;
using Project1.Persistance;
using Project1.Models;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Queries.GetLocation
{
    public class GetLocationHandlerTests
    {
        private readonly Mock<IApplicationUsersRepository> _applicationUsersRepositoryMock;
        private readonly GetLocationHandler _handler;

        public GetLocationHandlerTests()
        {
            _applicationUsersRepositoryMock = new Mock<IApplicationUsersRepository>(MockBehavior.Strict);
            _handler = new GetLocationHandler(_applicationUsersRepositoryMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<GetLocationResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call GetLocationAsync on LocationRepository")]
        public async Task HandleShouldCallGetLocationAsyncOnLocationRepository_WhenCommandIsSet()
        {
            // Arrange
            UsersEachLocation location = new UsersEachLocation(2, new List<int>());

            _applicationUsersRepositoryMock
                .Setup(x => x.GetLocationAsync(2, CancellationToken.None))
                .ReturnsAsync(location);

            var command = new GetLocationCommand(2);

            // Act
            GetLocationResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.UserEachLocation.Should().NotBeNull();

            _applicationUsersRepositoryMock
                .Verify(x => x.GetLocationAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);
        }
    }
}
