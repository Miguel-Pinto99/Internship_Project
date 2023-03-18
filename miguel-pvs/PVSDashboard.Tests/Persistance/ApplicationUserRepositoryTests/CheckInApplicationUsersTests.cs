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
    public class CheckInApplicationUsersTests : ApplicationUserRepositoryTestsBase
    {
   
        [Fact(DisplayName = "CheckInApplicationUser should be called on ApplicationUserRepository")]
        public async Task CheckInApplicationUserShouldReturnCheckIndUser_WhenRepositoryIsCalled()
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
            var response = await repository.CheckInApplicationUserAsync(applicationUser.Id, CancellationToken.None);
            var newUserInDb = context.Users.FirstOrDefaultAsync(x => x.Id == applicationUser.Id);
            applicationUser.CheckedIn = true;
            // Assert
            newUserInDb.Should().NotBeNull();
            newUserInDb.Result.Should().BeEquivalentTo(applicationUser);
            response.Should().BeEquivalentTo(applicationUser);
        }
        [Fact(DisplayName = "CheckInApplicationUser should be called on ApplicationUserRepository")]
        public async Task CheckInApplicationUserShouldReturnNullWhenUsedIdIsNonExistent_WhenRepositoryIsCalled()
        {
            int id = 2;
            // Act
            var response = await repository.CheckInApplicationUserAsync(id, CancellationToken.None);
            // Assert
            response.Should().BeNull();
        }
        [Fact(DisplayName = "CheckInApplicationUser should be called on ApplicationUserRepository")]
        public async Task CheckInApplicationUserShouldReturnNull_WhenFakeRepositoryIsCalled()
        {
            int id = 1;

            context.Database.EnsureDeleted();
            // Act
            var response = await repository.CheckInApplicationUserAsync(id, CancellationToken.None);
            // Assert
            response.Should().BeNull();
        }
    }
}
