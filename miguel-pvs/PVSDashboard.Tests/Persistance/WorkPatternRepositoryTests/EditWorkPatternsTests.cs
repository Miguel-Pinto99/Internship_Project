using Project1.Data;
using Project1.Models;
using FluentAssertions.Specialized;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Project1.Persistance;

namespace PVSDashboard.Tests.Persistance.WorkPatternRepositoryTests
{
    public class UpdateWorkPatternsTests : WorkPatternRepositoryTestsBase
    {
        [Fact(DisplayName = "UpdateWorkPattern should be called on WorkPatternRepository")]
        public async Task UpdateWorkPatternShouldReturnUpdatedUser_WhenRepositoryIsCalled()
        {
            // Arrange
            Guid id = userInDb.Id;
            var workPattern = new WorkPattern
            {
                Id = id,
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
            };

            // Act
            var response = await repository.UpdateWorkPatternAsync(workPattern, CancellationToken.None);
            var newWorkPatternInDb = context.WorkPattern.FirstOrDefaultAsync(x => x.Id == workPattern.Id);
            // Assert
            newWorkPatternInDb.Should().NotBeNull();
            newWorkPatternInDb.Result.Should().BeEquivalentTo(workPattern);
            response.Should().BeEquivalentTo(workPattern);
        }


        [Fact(DisplayName = "UpdateWorkPattern should call Exception when input is null")]
        public async Task UpdateWorkPatternShouldReturnNull_WhenRepositoryIsCalled()
        {
            // Arrange;

            Func<Task<WorkPattern>> act = async () => await repository.UpdateWorkPatternAsync(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'WorkPattern')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "UpdateWorkPattern should be called on WorkPatternRepository")]
        public async Task UpdateWorkPatternShouldReturnNullWhenUsingWhrongId_WhenRepositoryIsCalled()
        {
            // Arrange

            var workPattern = new WorkPattern
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
            };

            // Act
            var response = await repository.UpdateWorkPatternAsync(workPattern, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }

        [Fact(DisplayName = "UpdateWorkPattern should be called on WorkPatternRepository")]
        public async Task UpdateWorkPatternShouldReturnNull_WhenFakeRepositoryIsCalled()
        {
            // Arrange
            Guid id = userInDb.Id;
            var workPattern = new WorkPattern
            {
                Id = id,
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
            };

            context.Database.EnsureDeleted();
            // Act
            var response = await repository.UpdateWorkPatternAsync(workPattern, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }
    }
}
