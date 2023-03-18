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
    public class CheckOutApplicationUsersTests : ApplicationUserRepositoryTestsBase
    {
        [Fact(DisplayName = "CheckOutApplicationUser should be called on ApplicationUserRepository")]
        public async Task CheckOutApplicationUserShouldReturnCheckOutdUser_WhenRepositoryIsCalled()
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
            var response = await repository.CheckOutApplicationUserAsync(applicationUser.Id, CancellationToken.None);
            var newUserInDb = context.Users.FirstOrDefaultAsync(x => x.Id == applicationUser.Id);
            applicationUser.CheckedIn = false;
            // Assert
            newUserInDb.Should().NotBeNull();
            newUserInDb.Result.Should().BeEquivalentTo(applicationUser);
            response.Should().BeEquivalentTo(applicationUser);
        }
        [Fact(DisplayName = "CheckOutApplicationUser should be called on ApplicationUserRepository")]
        public async Task CheckOutApplicationUserShouldReturnNullWhenUsedIdIsNonExistent_WhenRepositoryIsCalled()
        {
            int id = 2;
            // Act
            var response = await repository.CheckOutApplicationUserAsync(id, CancellationToken.None);
            // Assert
            response.Should().BeNull();
        }
        [Fact(DisplayName = "CheckOutApplicationUser should be called on ApplicationUserRepository")]
        public async Task CheckOutApplicationUserShouldReturnNull_WhenFakeRepositoryIsCalled()
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
