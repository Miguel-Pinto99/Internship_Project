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
    public class UpdateAbsentsTests : AbsentRepositoryTestsBase
    {
        [Fact(DisplayName = "UpdateAbsent should be called on AbsentRepository")]
        public async Task UpdateAbsentShouldReturnUpdatedUser_WhenRepositoryIsCalled()
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
            var response = await repository.UpdateAbsentAsync(absent, CancellationToken.None);
            var newUserInDb = await context.Absent.FirstOrDefaultAsync(x => x.Id == absent.Id);
            // Assert
            newUserInDb.Should().NotBeNull();
            newUserInDb.Should().BeEquivalentTo(absent);
            response.Should().BeEquivalentTo(absent);
        }


        [Fact(DisplayName = "UpdateAbsent should call Exception when input is null")]
        public async Task UpdateAbsentShouldReturnNull_WhenRepositoryIsCalled()
        {
            // Arrange;

            Func<Task<Absent>> act = async () => await repository.UpdateAbsentAsync(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'absent')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "UpdateAbsent should be called on AbsentRepository")]
        public async Task UpdateAbsentShouldReturnNullWhenUsingWhrongId_WhenRepositoryIsCalled()
        {
            // Arrange

            var absent = new Absent
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
            };

            // Act
            var response = await repository.UpdateAbsentAsync(absent, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }

        [Fact(DisplayName = "UpdateAbsent should be called on AbsentRepository")]
        public async Task UpdateAbsentShouldReturnNull_WhenFakeRepositoryIsCalled()
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
            // Act
            var response = await repository.UpdateAbsentAsync(absent, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }
    }
}

