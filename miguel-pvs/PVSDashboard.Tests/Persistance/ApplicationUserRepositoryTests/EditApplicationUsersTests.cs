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
    public class UpdateApplicationUsersTests : ApplicationUserRepositoryTestsBase
    {
        [Fact(DisplayName = "UpdateApplicationUser should be called on ApplicationUserRepository")]
        public async Task UpdateApplicationUserShouldReturnUpdatedUser_WhenRepositoryIsCalled()
        {
            // Arrange

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 2,
                WorkPatterns = new List<WorkPattern>()
            };

            // Act
            var response = await repository.UpdateApplicationUserAsync(applicationUser, CancellationToken.None);
            var newUserInDb = context.Users.FirstOrDefaultAsync(x => x.Id == applicationUser.Id);

            // Assert
            newUserInDb.Should().NotBeNull();
            newUserInDb.Result.Should().BeEquivalentTo(applicationUser);
            response.Should().BeEquivalentTo(applicationUser);
        }


        [Fact(DisplayName = "UpdateApplicationUser should call Exception when input is null")]
        public async Task UpdateApplicationUserShouldReturnNull_WhenRepositoryIsCalled()
        {
            // Arrange;

            Func<Task<ApplicationUser>> act = async () => await repository.UpdateApplicationUserAsync(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'user')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "UpdateApplicationUser should be called on ApplicationUserRepository")]
        public async Task UpdateApplicationUserShouldReturnNullWhenUsingWhrongId_WhenRepositoryIsCalled()
        {
            // Arrange

            var applicationUser = new ApplicationUser
            {
                Id = 2,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 2,
                WorkPatterns = new List<WorkPattern>()
            };

            // Act
            var response = await repository.UpdateApplicationUserAsync(applicationUser, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }

        [Fact(DisplayName = "UpdateApplicationUser should be called on ApplicationUserRepository")]
        public async Task UpdateApplicationUserShouldReturnNull_WhenFakeRepositoryIsCalled()
        {
            // Arrange

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 2,
                WorkPatterns = new List<WorkPattern>()
            };

            context.Database.EnsureDeleted();
            // Act
            var response = await repository.UpdateApplicationUserAsync(applicationUser, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }
    }
}

