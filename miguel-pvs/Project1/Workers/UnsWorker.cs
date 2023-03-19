using Project1.Infrastructure;
using Project1.Persistance;
using Project1.Timers;

namespace Project1.Workers
{
    public class UnsWorker : BackgroundService
    {
        private readonly IUnsService _unsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITimerService _timerService;

        public UnsWorker(IUnsService unsService,
            IServiceProvider serviceProvider,
            ITimerService timerService)

        {
            _unsService = unsService;
            _serviceProvider = serviceProvider;
            _timerService = timerService;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            using var scope = _serviceProvider.CreateScope();
            var applicationUsersRepository = scope.ServiceProvider.GetRequiredService<IApplicationUsersRepository>();
            var workPatternRepository = scope.ServiceProvider.GetRequiredService<IWorkPatternRepository>();
            var absentRepository = scope.ServiceProvider.GetRequiredService<IAbsentRepository>();


            //Init
            var now = DateTime.UtcNow;

            var users = await applicationUsersRepository.GetAllApplicationUserAsync(stoppingToken);
            var wP = await workPatternRepository.GetAllWorkPatternsAsync(stoppingToken);
            var location = await applicationUsersRepository.GetAllLocationsAsync(stoppingToken);
            var absent = await absentRepository.GetAllAbsentAsync(stoppingToken);

            var updateAbsentsTask = _timerService.SetInitialAbsentAsync(absent, stoppingToken);
            var updateWorkPatternsTask = _timerService.SetInitialTimerAsync(users, stoppingToken);
            await _unsService.CallEachCheckInAsync(users,now , stoppingToken);
            await _unsService.CallEachWorkPatternAsync(wP, stoppingToken);
            await _unsService.PostAllLocationAsync(location, stoppingToken);
            await _unsService.CallEachApplicationUserAsync(users, stoppingToken);

           

            await Task.WhenAll(updateAbsentsTask, updateWorkPatternsTask);


            //var commandGetAllApplicationUser = new GetAllApplicationUserCommand();
            //var responseGetAllApplicationUser = await _mediator.Send(commandGetAllApplicationUser, stoppingToken);
            //var listUsers = responseGetAllApplicationUser.listApplicationUser;

            //var commandGetAllWorkPattern = new GetAllWorkPatternCommand();
            //var responseGetAllWorkPattern = await _mediator.Send(commandGetAllWorkPattern, stoppingToken);
            //var listWorkPatterns = responseGetAllApplicationUser.listApplicationUser;

            //var commandGetAllLocation = new GetAllLocationCommand();
            //var responseGetAllLocation = await _mediator.Send(commandGetAllLocation, stoppingToken);
            //var listLocations = responseGetAllApplicationUser.listApplicationUser;




            //while (!stoppingToken.IsCancellationRequested)
            //{
                //var Users = await applicationUsersRepository.GetAllApplicationUserAsync(stoppingToken);
                //var WP = await workPatternRepository.GetAllWorkPatternsAsync(stoppingToken);
                //var Location = await applicationUsersRepository.GetAllLocationsAsync(stoppingToken);

                //var updateWorkPatternsTask = _timerService.SetInitialTimer(Users, stoppingToken);
                //await _unsService.CallEachCheckInAsync(Users, stoppingToken);
                //await _unsService.CallEachWorkPatternAsync(WP, stoppingToken);
                //await _unsService.PostAllLocationAsync(Location, stoppingToken);
                //await _unsService.CallEachApplicationUserAsync(Users, stoppingToken);
                //await Task.Delay(2000, stoppingToken);
            //}



            // On Stop

        }
    }
}
