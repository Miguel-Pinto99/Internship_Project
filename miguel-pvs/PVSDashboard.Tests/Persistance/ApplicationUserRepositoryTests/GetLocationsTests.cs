using Project1.Data;
using Project1.Models;
using FluentAssertions.Specialized;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Project1.Persistance;

namespace PVSDashboard.Tests.Persistance.ApplicationUserRepositoryTests
{
    public class GetLocationsTests : ApplicationUserRepositoryTestsBase
    {
        [Fact(DisplayName = "GetAllLocations should be called on ApplicationUserRepository")]
        public async Task GetLocationsShouldReturnUsersInTheSameLocation_WhenRepositoryIsCalled()
        {
            // Arrange
            UsersEachLocation location = new UsersEachLocation(1, new List<int> {1});
            // Act
            var response = await repository.GetLocationAsync(1,CancellationToken.None);

            // Assert
            response.Should().BeEquivalentTo(location);
        }
        [Fact(DisplayName = "GetAllLocations should be called on ApplicationUserRepository")]
        public async Task GetAllLocationsShouldReturnCreatedUser_WhenRepositoryIsCalled()
        {
            // Arrange
            int officeLocation = 2;
            // Act
            var response = await repository.GetLocationAsync(officeLocation, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }
    }
}
