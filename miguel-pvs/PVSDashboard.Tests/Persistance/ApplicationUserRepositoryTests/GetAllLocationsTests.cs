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
    public class GetAllLocationsTests : ApplicationUserRepositoryTestsBase
    {

        [Fact(DisplayName = "GetAllLocation should be called on ApplicationUserRepository")]
        public async Task GetAllLocationShouldReturnCreatedUser_WhenRepositoryIsCalled()
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

            var usersEachLocation = new UsersEachLocation(1, new List<int>{1});
            var listUsersEachLocation = new List<UsersEachLocation>();
            listUsersEachLocation.Add(usersEachLocation);
            // Act
            var response = await repository.GetAllLocationsAsync(CancellationToken.None);

            // Assert
            response.Should().BeEquivalentTo(listUsersEachLocation);

        }
    }
}
