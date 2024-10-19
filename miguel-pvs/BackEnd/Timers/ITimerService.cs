using Project1.Models;

namespace Project1.Timers
{
    public interface ITimerService
    {
        public Task SetInitialAbsentAsync(List<Absent> listAllAbsentss, CancellationToken cancellationToken);
        public Task RemoveAbsentsAsync(List<Absent> listAllAbsentss, CancellationToken cancellationToken);
        public Task SetAbsentsAsync(List<Absent> listAllAbsents, CancellationToken cancellationToken);
        public Task CalculateDelayAsync(ApplicationUser applicationUser, CancellationToken cancellationToken);
        public Task ReinitializeTimersAsync(ApplicationUser applicationUser, CancellationToken cancellationToken);
        public Task RemoveTimersAsync(ApplicationUser applicationUser, CancellationToken cancellationToken);
        public Task SetInitialTimerAsync(List<ApplicationUser> applicationUsers, CancellationToken cancellationToken);
    }
}
