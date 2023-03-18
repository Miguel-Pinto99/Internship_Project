using Project1.Data;
using Project1.Models;
using FluentAssertions.Specialized;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Project1.Persistance;

namespace PVSDashboard.Tests.Persistance.AbsentRepositoryTests
{
    public class GetAbsentsTests : AbsentRepositoryTestsBase
    {
        [Fact(DisplayName = "GetAbsent should be called on AbsentRepository")]
        public async Task GetAbsentShouldReturnGetdUser_WhenRepositoryIsCalled()
        {
            // Arrange

            Guid id = userInDb.Id;
            var absent = new Absent
            {
                Id = id,
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
            };

            // Act
            var response = await repository.GetAbsentAsync(id, CancellationToken.None);

            // Assert
            response.Should().BeEquivalentTo(absent);
        }

        [Fact(DisplayName = "GetAbsent should be called on AbsentRepository")]
        public async Task GetAbsentShouldReturnNullWhenWrongIdIsCalled_WhenRepositoryIsCalled()
        {
            // Arrange

            Guid id = new Guid("00000000-0000-0000-0000-000000000001");

            // Act
            var response = await repository.GetAbsentAsync(id, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }
    }
}
