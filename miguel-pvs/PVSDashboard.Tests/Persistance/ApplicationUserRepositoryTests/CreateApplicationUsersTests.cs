using Project1.Data;
using Project1.Models;
using FluentAssertions.Specialized;
using FluentAssertions;
using Moq;
using Project1.Events.AbsentLogicEvents;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Project1.Infrastructure;
using Project1.Persistance;
using Project1.Application.ApplicationUsers.Commands.CreateApplicationUser;
using System.Reflection.Metadata;

namespace PVSDashboard.Tests.Persistance.ApplicationUserRepositoryTests
{
    public class CreateApplicationUsersTests : ApplicationUserRepositoryTestsBase
   {

        [Fact(DisplayName = "CreateApplicationUser should be called on ApplicationUserRepository")]
        public async Task CreateApplicationUserShouldReturnCreatedUser_WhenRepositoryIsCalled()
        {
            // Arrange
            var applicationUser = new ApplicationUser
            {
                Id = 2,
                FirstName = "Miguel",
                CheckedIn = true,
                OfficeLocation = 1,
                WorkPatterns = new List<WorkPattern>()
            };

            // Act
            var response = await repository.CreateApplicationUserAsync(applicationUser, CancellationToken.None);
            var newUserInDb = context.Users.FirstOrDefaultAsync(x => x.Id == applicationUser.Id);
            applicationUser.WorkPatterns = null;
            // Assert
            newUserInDb.Should().NotBeNull();
            newUserInDb.Result.Should().BeEquivalentTo(applicationUser);
            response.Should().BeEquivalentTo(applicationUser);

        }


        [Fact(DisplayName = "CreateApplicationUser should call Exception when input is null")]
        public async Task CreateApplicationUserShouldReturnNull_WhenRepositoryIsCalled()
        {
            // Arrange;

            Func<Task<ApplicationUser>> act = async () => await repository.CreateApplicationUserAsync(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'user')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "CreateApplicationUser should be called on ApplicationUserRepository")]
        public async Task CreateApplicationUserShouldReturnErrorWhenTryingToCreateUserWithExistingId_WhenRepositoryIsCalled()
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
            var response = await repository.CreateApplicationUserAsync(applicationUser, CancellationToken.None);
            applicationUser.WorkPatterns = null;
            // Assert
            response.Should().BeNull();
        }

        [Fact(DisplayName = "CreateApplicationUser should be called on ApplicationUserRepository")]
        public async Task CreateApplicationUserShouldReturnNull_WhenFakeRepositoryIsCalled()
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
            context.Database.EnsureDeleted();
            // Act
            var response = await repository.CreateApplicationUserAsync(applicationUser, CancellationToken.None);
            applicationUser.WorkPatterns = null;
            // Assert
            response.Should().BeNull();           
        }
    }
}
