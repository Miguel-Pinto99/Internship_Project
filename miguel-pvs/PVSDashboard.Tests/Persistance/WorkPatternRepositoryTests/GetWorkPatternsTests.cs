using Project1.Models;
using FluentAssertions;
using Xunit;

namespace PVSDashboard.Tests.Persistance.WorkPatternRepositoryTests
{
    public class GetWorkPatternsTests : WorkPatternRepositoryTestsBase
    {
        [Fact(DisplayName = "GetWorkPattern should be called on WorkPatternRepository")]
        public async Task GetWorkPatternShouldReturnGetdUser_WhenRepositoryIsCalled()
        {
            // Arrange

            Guid id = userInDb.Id;
            var workPattern = new WorkPattern
            {
                Id = id,
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
                Parts = new List<WorkPatternPart>()
            };

            // Act
            var response = await repository.GetWorkPatternAsync(id, CancellationToken.None);

            // Assert
            response.Should().BeEquivalentTo(workPattern);
        }

        [Fact(DisplayName = "GetWorkPattern should be called on WorkPatternRepository")]
        public async Task GetWorkPatternShouldReturnNullWhenWrongIdIsCalled_WhenRepositoryIsCalled()
        {
            // Arrange

            Guid id = new Guid("00000000-0000-0000-0000-000000000001");

            // Act
            var response = await repository.GetWorkPatternAsync(id, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }
    }
}
