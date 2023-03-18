using Project1.Data;
using Project1.Models;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Project1.Persistance;
using FluentAssertions.Specialized;
using System.Linq.Expressions;

namespace PVSDashboard.Tests.Persistance.ApplicationUserRepositoryTests
{
    public class DeleteApplicationUsersTests : ApplicationUserRepositoryTestsBase
    {
        [Fact(DisplayName = "DeleteApplicationUser should be called on ApplicationUserRepository")]
        public async Task DeleteApplicationUserShouldReturnDeletedUser_WhenRepositoryIsCalled()
        {
            // Arrange

            int id = 1;

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 1,
                WorkPatterns = new List<WorkPattern>()
            };


            //Act
            var response = await repository.DeleteApplicationUserAsync(id, CancellationToken.None);
            var newUserInDb = context.Users.FirstOrDefaultAsync(x => x.Id == applicationUser.Id);

            // Assert
            newUserInDb.Result.Should().BeNull();
            response.Should().BeEquivalentTo(applicationUser);
        }

        [Fact(DisplayName = "DeleteApplicationUser should be called on ApplicationUserRepository")]
        public async Task DeleteApplicationUserShouldReturnNullWhenUsingIdNotInDatabase_WhenRepositoryIsCalled()
        {
            // Arrange

            int id = 2;
            //Act
            var response = await repository.DeleteApplicationUserAsync(id, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }

        [Fact(DisplayName = "DeleteApplicationUser should be called on ApplicationUserRepository")]
        public async Task DeleteApplicationUserShouldReturnNull_WhenFakeRepositoryIsCalled()
        {
            // Arrange

            int id = 1;

            context.Database.EnsureDeleted();

            //Act
            var response = await repository.DeleteApplicationUserAsync(id, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }
    }
}
