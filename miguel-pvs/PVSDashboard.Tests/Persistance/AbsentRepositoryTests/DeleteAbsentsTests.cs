using Project1.Data;
using Project1.Models;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Project1.Persistance;

namespace PVSDashboard.Tests.Persistance.AbsentRepositoryTests
{
    public class DeleteAbsentsTests : AbsentRepositoryTestsBase
    {
        [Fact(DisplayName = "DeleteAbsent should be called on AbsentRepository")]
        public async Task DeleteAbsentShouldReturnDeletedUser_WhenRepositoryIsCalled()
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


            //Act
            var response = await repository.DeleteAbsentAsync(id, CancellationToken.None);
            var newUserInDb = await context.Absent.FirstOrDefaultAsync(x => x.Id == absent.Id);
            // Assert
            newUserInDb.Should().BeNull();
            response.Should().BeEquivalentTo(absent);
        }

        [Fact(DisplayName = "DeleteAbsent should be called on AbsentRepository")]
        public async Task DeleteAbsentShouldReturnNullWhenUsingIdNotInDatabase_WhenRepositoryIsCalled()
        {
            // Arrange

            var absent = new Absent
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
            };
            //Act
            var response = await repository.DeleteAbsentAsync(absent.Id, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }

        [Fact(DisplayName = "DeleteAbsent should be called on AbsentRepository")]
        public async Task DeleteAbsentShouldReturnNull_WhenFakeRepositoryIsCalled()
        {
            // Arrange

            var absent = new Absent
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
            };

            context.Database.EnsureDeleted();

            //Act
            var response = await repository.DeleteAbsentAsync(absent.Id, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }
    }
}
