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
    public class GetApplicationUsersTests : ApplicationUserRepositoryTestsBase
    {

        [Fact(DisplayName = "GetApplicationUser should be called on ApplicationUserRepository")]
        public async Task GetApplicationUserShouldReturnGetdUser_WhenRepositoryIsCalled()
        {
            // Arrange


            var applicationUser = new ApplicationUser
            {
                Id = 1,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 1,
                WorkPatterns = new List<WorkPattern>()
            };

            // Act
            var response = await repository.GetApplicationUserAsync(applicationUser.Id,CancellationToken.None);

            // Assert
            response.Should().BeEquivalentTo(applicationUser);
        }

        [Fact(DisplayName = "GetApplicationUser should be called on ApplicationUserRepository")]
        public async Task GetApplicationUserShouldReturnNullWhenWrongIdIsCalled_WhenRepositoryIsCalled()
        {
            // Arrange

            int id = 2;

            // Act
            var response = await repository.GetApplicationUserAsync(id, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }
    }
}
