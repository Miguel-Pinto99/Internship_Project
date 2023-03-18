using Project1.Models;
using FluentAssertions.Specialized;
using FluentAssertions;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace PVSDashboard.Tests.Persistance.WorkPatternRepositoryTests
{
    public class CreateWorkPatternsTests : WorkPatternRepositoryTestsBase 
    {
        [Fact(DisplayName = "CreateWorkPattern should be called on WorkPatternRepository")]
        public async Task CreateWorkPatternShouldReturnCreatedUser_WhenRepositoryIsCalled()
        {
            // Arrange
            Guid id = userInDb.Id;
            var workPattern = new WorkPattern
            {
                UserId = 1,
                Id = id,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
                Parts = new List<WorkPatternPart>()
            };

            // Act
            var response = await repository.CreateWorkPatternAsync(workPattern, CancellationToken.None);
            var newWorkPatternInDb = context.WorkPattern.FirstOrDefaultAsync(x => x.Id == workPattern.Id);
            // Assert
            newWorkPatternInDb.Should().NotBeNull();
            newWorkPatternInDb.Result.Should().BeEquivalentTo(workPattern);
            response.Should().BeEquivalentTo(workPattern);

        }

        [Fact(DisplayName = "CreateWorkPattern should call Exception when input is null")]
        public async Task CreateWorkPatternShouldReturnNull_WhenRepositoryIsCalled()
        {
            // Arrange;

            Func<Task<WorkPattern>> act = async () => await repository.CreateWorkPatternAsync(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'WorkPattern')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "CreateWorkPattern should be called on WorkPatternRepository")]
        public async Task CreateWorkPatternShouldReturnNull_WhenFakeRepositoryIsCalled()
        {
            // Arrange
            Guid id = userInDb.Id;
            var workPattern = new WorkPattern
            {
                UserId = 1,
                Id = id,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
                Parts = new List<WorkPatternPart>()
            };

            context.Dispose();

            // Act
            var response = await repository.CreateWorkPatternAsync(workPattern, CancellationToken.None);

            // Assert
            response.Should().BeNull();

        }
    }
}
