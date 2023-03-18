using FluentAssertions.Specialized;
using FluentAssertions;
using Moq;
using Xunit;
using Project1.Application.ApplicationUsers.Queries.GetAllLocation;
using Project1.Persistance;
using System.Collections.Generic;
using Project1.Models;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Queries.GetAllLocation
{
    public class GetAllLocationsHandlerTests
    {
        private readonly Mock<IApplicationUsersRepository> _appicationUsersRepositoryMock;
        private readonly GetAllLocationHandler _handler;

        public GetAllLocationsHandlerTests()
        {
            _appicationUsersRepositoryMock = new Mock<IApplicationUsersRepository>(MockBehavior.Strict);
            _handler = new GetAllLocationHandler(_appicationUsersRepositoryMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<GetAllLocationResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call GetAllLocationAsync on AllLocationRepository")]
        public async Task HandleShouldCallGetAllLocationAsyncOnAllLocationRepository_WhenCommandIsSet()
        {
            // Arrange
            List<UsersEachLocation> allLocation = new List<UsersEachLocation>
            {
            };
            _appicationUsersRepositoryMock
                .Setup(x => x.GetAllLocationsAsync(CancellationToken.None))
                .ReturnsAsync(allLocation);

            var command = new GetAllLocationCommand();

            // Act
            GetAllLocationResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.ListUserEachLocation.Should().NotBeNull();

            _appicationUsersRepositoryMock
                .Verify(x => x.GetAllLocationsAsync(CancellationToken.None), Times.Once);
        }
    }
}
