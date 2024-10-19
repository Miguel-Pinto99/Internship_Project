using Project1.Models;

namespace Project1.Infrastructure
{
    public interface IUnsService
    {
        public Task CallEachCheckInAsync(List<ApplicationUser> applicationUser, DateTime now, CancellationToken cancellationToken);
        public Task PublishCheckInAsync(ApplicationUser applicationUser, DateTime now, CancellationToken cancellationToken);


        public Task CallEachApplicationUserAsync(List<ApplicationUser> applicationUser, CancellationToken cancellationToken);
        public Task CallListWorkPatternAsync(ApplicationUser applicationUser, CancellationToken cancellationToken);
        public Task CallEachWorkPatternAsync(List<WorkPattern> workPattern, CancellationToken cancellationToken);
        public Task CallListWorkPatternPartAsync(WorkPattern workPattern, CancellationToken cancellationToken);


        public Task PublishLocationAsync(UsersEachLocation location, int officeLocationId, CancellationToken cancellationToken);
        public Task PostAllLocationAsync(List<UsersEachLocation> location, CancellationToken cancellationToken);


        public Task ScheduledToWorkNowAsync(WorkPatternPart workPatternPart, CancellationToken cancellationToken);
        public Task ScheduledTodayAsync(WorkPatternPart workPatternPart, CancellationToken cancellationToken);
        public Task CheckTodayAbsentsAsync(List<Models.Absent> listAllAbsents,CancellationToken cancellationToken);

        public Task DeleteTopicApplicationUserAsync(ApplicationUser applicationUser, CancellationToken cancellationToken);

    }
}
