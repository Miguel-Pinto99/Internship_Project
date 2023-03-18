using Project1.Data;
using Project1.Models;
using FluentAssertions.Specialized;
using FluentAssertions;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Project1.Persistance;

namespace PVSDashboard.Tests.Persistance.AbsentRepositoryTests
{
    public class CreateAbsentsTests : AbsentRepositoryTestsBase
    {

        [Fact(DisplayName = "CreateAbsent should be called on AbsentRepository")]
        public async Task CreateAbsentShouldReturnCreatedUser_WhenRepositoryIsCalled()
        {
            // Arrange

            var absent = new Absent
            {
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2022, 1, 1, 0, 0, 0),
            };

            // Act
            var response = await repository.CreateAbsentAsync(absent, CancellationToken.None);
            var newUserInDb = await context.Absent.FirstOrDefaultAsync(x => x.Id == absent.Id);
            // Assert
            newUserInDb.Should().NotBeNull();
            newUserInDb.Should().BeEquivalentTo(absent);
            response.Should().Be(absent);

        }


        [Fact(DisplayName = "CreateAbsent should call Exception when input is null")]
        public async Task CreateAbsentShouldReturnNull_WhenRepositoryIsCalled()
        {
            // Arrange;

            Func<Task<Absent>> act = async () => await repository.CreateAbsentAsync(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'absent')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "CreateAbsent should call Exception when input is null")]
        public async Task CreateAbsentShouldReturnNull_WhenFakeRepositoryIsCalled()
        {
            // Arrange

            var absent = new Absent
            {
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2022, 1, 1, 0, 0, 0),
            };

            context.Dispose();
            // Act
            var response = await repository.CreateAbsentAsync(absent, CancellationToken.None);
            // Assert
            response.Should().BeNull();
        }
    }
}
