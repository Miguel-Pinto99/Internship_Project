using Project1.Models;
using Project1.Persistance;

namespace PVSDashboard.Tests.Persistance.WorkPatternRepositoryTests
{
    public class WorkPatternRepositoryTestsBase : RepositoryTestsBase
    {
        protected readonly WorkPatternRepository repository;

        public WorkPattern userInDb;
        public WorkPatternRepositoryTestsBase()
        {
            repository = new WorkPatternRepository(context);
            SetupDb(repository);
        }

        public async void SetupDb(WorkPatternRepository repository)
        {
            var workPattern = new WorkPattern
            {
                Id = new Guid(),
                UserId = 1,
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
                Parts = new List<WorkPatternPart>()
            };

            userInDb = await repository.CreateWorkPatternAsync(workPattern, CancellationToken.None);

        }

    }
}
