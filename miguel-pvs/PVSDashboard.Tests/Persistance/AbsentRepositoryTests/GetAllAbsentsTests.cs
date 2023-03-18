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
    public class GetAllAbsentsTests : AbsentRepositoryTestsBase
    {

        [Fact(DisplayName = "GetAllAbsent should be called on AbsentRepository")]
        public async Task GetAllAbsentShouldReturnGetAlldUser_WhenRepositoryIsCalled()
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
            var listAbsents = new List<Absent>{absent};


            // Act
            var response = await repository.GetAllAbsentAsync(CancellationToken.None);

            // Assert
            response.Should().BeEquivalentTo(listAbsents);
        }
    }
}
