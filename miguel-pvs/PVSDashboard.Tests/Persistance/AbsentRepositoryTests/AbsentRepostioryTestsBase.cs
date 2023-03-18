using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project1.Data;
using Project1.Models;
using Project1.Persistance;
using Xunit;
using Xunit.Abstractions;

namespace PVSDashboard.Tests.Persistance.AbsentRepositoryTests
{
    public class AbsentRepositoryTestsBase : RepositoryTestsBase
    {
        protected readonly AbsentRepository repository;
        public Absent userInDb;
        public AbsentRepositoryTestsBase()
        {
            repository = new AbsentRepository(context);
            SetupDb(repository);
        }

        public async void SetupDb(AbsentRepository repository)
        {
            var absent = new Absent
            {
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),

            };

            userInDb = await repository.CreateAbsentAsync(absent, CancellationToken.None);
            

        }

    }
}
