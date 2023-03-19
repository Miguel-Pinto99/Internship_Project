using System;
using System.Collections.Generic;
using System.Threading;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Application.WorkPatterns.Queries.GetAllWorkPattern;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Timers
{
    public class TimerService : ITimerService, IDisposable
    {
        private readonly Dictionary<int, DateTime> _usersWithNextUpdateTime = new();
        private readonly IMediator _mediator;
        private readonly IUnsService _unsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAbsentRepository _AbsentRepository;

        private Task _taskTimers;
        private Task _taskAbsents;
        private CancellationTokenSource _cancellationTokenSourceTimers = new();
        private CancellationTokenSource _cancellationTokenSourceAbsents = new();
        private List<Absent> _listAllAbsents = new List<Absent>();
        private ApplicationUser _lastUpdatedUser;

        public TimerService(IMediator mediator,
            IUnsService unsService,
            IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _unsService = unsService;
            _serviceProvider = serviceProvider;
        }

        public async Task SetAbsentsAsync(List<Absent> listAllAbsents, CancellationToken token)
        {

            using var scope1 = _serviceProvider.CreateScope();
            var absentsRepository1 = scope1.ServiceProvider.GetRequiredService<IAbsentRepository>();
            var listAllAbsents1 = await absentsRepository1.GetAllAbsentAsync(token);
            var _listAllAbsents = listAllAbsents1;

            await _unsService.CheckTodayAbsentsAsync(listAllAbsents, token);
            TimeSpan midnight = DateTime.UtcNow.Date.TimeOfDay;
            TimeSpan hourNow = DateTime.UtcNow.TimeOfDay;
            TimeSpan hoursDay = TimeSpan.FromDays(1);

            if (hourNow != midnight)
            {
                await Task.Delay(hoursDay - hourNow, token);
            }
            else
            {
                await Task.Delay(hoursDay, token);
            }

            using var scope2 = _serviceProvider.CreateScope();
            var workPatternRepository2 = scope2.ServiceProvider.GetRequiredService<IAbsentRepository>();
            var listWorkPatterns2 = await workPatternRepository2.GetAllAbsentAsync(token);
            await SetAbsentsAsync(listWorkPatterns2, token);

        }

        public async Task SetInitialAbsentAsync(List<Absent> listAllAbsents, CancellationToken cancellationToken)
        {
            var token = _cancellationTokenSourceTimers.Token;
            _taskAbsents = SetAbsentsAsync(listAllAbsents, token);
        }
        public async Task RemoveAbsentsAsync(List<Absent> listAllAbsents, CancellationToken cancellationToken)
        {
            _cancellationTokenSourceAbsents.Cancel();
            _cancellationTokenSourceAbsents.Dispose();
            _cancellationTokenSourceAbsents = new CancellationTokenSource();


            var token = _cancellationTokenSourceAbsents.Token;
            _taskAbsents = SetAbsentsAsync(listAllAbsents, token);
        }

        public async Task SetInitialTimerAsync(List<ApplicationUser> applicationUsers, CancellationToken cancellationToken)
        {
            var token = _cancellationTokenSourceTimers.Token;
            foreach (var applicationUser in applicationUsers)
            {
                _lastUpdatedUser = applicationUser;
                await CalculateDelayWithoutSettingTaskAsync(applicationUser.Id, token);
            }
            _taskTimers = WaitForNextShiftAsync(token);

        }
        public async Task CalculateDelayWithoutSettingTaskAsync(int userId, CancellationToken token)
        {
            List<WorkPattern> listWorkPatterns = _lastUpdatedUser.WorkPatterns;

            try
            {

                DateTime now = DateTime.UtcNow;
                TimeSpan lowestDelay = await FindLowestDelayAsync(listWorkPatterns, now, token);
                DateTime nextUserShiftDateTime = now + lowestDelay;

                if (_usersWithNextUpdateTime.ContainsKey(userId))
                {
                    _usersWithNextUpdateTime[userId] = nextUserShiftDateTime;
                }
                else
                {
                    _usersWithNextUpdateTime.Add(userId, nextUserShiftDateTime);
                }
            }
            catch
            {
            }

        }
        public async Task CalculateDelayAsync(ApplicationUser applicationUser, CancellationToken token)

        {
            _lastUpdatedUser = applicationUser;

            List<WorkPattern> listWorkPatterns =applicationUser.WorkPatterns;
            List<WorkPattern> newListWorkPattern = listWorkPatterns;
            try
            {
                DateTime now = DateTime.UtcNow;
                TimeSpan lowestDelay = await FindLowestDelayAsync(listWorkPatterns, now, token);
                DateTime nextUserShiftDateTime = now + lowestDelay;
                int id = applicationUser.Id;
                if (_usersWithNextUpdateTime.ContainsKey(id))
                {
                    _usersWithNextUpdateTime[id] = nextUserShiftDateTime;
                }
                else
                {
                    _usersWithNextUpdateTime.Add(id, nextUserShiftDateTime);
                }
                var eventPublishWorkPattern = new PublishWorkPatternEvent(applicationUser);
                await _mediator.Publish(eventPublishWorkPattern, token);
                _taskTimers = WaitForNextShiftAsync(token);
            }
            catch
            {
            }

        }
        public async Task WaitForNextShiftAsync(CancellationToken token)
        {
            if (_usersWithNextUpdateTime.Count() == 0)
            {
                await Task.Delay(9999999, token);
            }
            else
            {
                while (!token.IsCancellationRequested)
                {
                    DateTime now = DateTime.UtcNow;
                    DateTime nextShiftDateTime = _usersWithNextUpdateTime.Min(x => x.Value);
                    var applicationUserInDic = _usersWithNextUpdateTime.FirstOrDefault(x => x.Value == nextShiftDateTime);

                    int idApplicationUserNextShift = applicationUserInDic.Key;
                    TimeSpan delayTask = nextShiftDateTime - now;

                    using var scope = _serviceProvider.CreateScope();
                    var applicationUsersRepository = scope.ServiceProvider.GetRequiredService<IApplicationUsersRepository>();
                    var user = await applicationUsersRepository.GetApplicationUserAsync(idApplicationUserNextShift, token);
                    var eventPublishWorkPattern = new PublishWorkPatternEvent(user);
                    await _mediator.Publish(eventPublishWorkPattern, token);

                    if (delayTask < TimeSpan.Zero)
                    {
                        delayTask = TimeSpan.FromSeconds(1);
                    };

                    await Task.Delay(delayTask, token);
                    await CalculateDelayWithoutSettingTaskAsync(idApplicationUserNextShift, token);
                }
            }
        }
        public async Task ReinitializeTimersAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
        {
            _cancellationTokenSourceTimers.Cancel();
            try
            {
                await _taskTimers;
            }
            catch (TaskCanceledException) { }
            _cancellationTokenSourceTimers.Dispose();
            _cancellationTokenSourceTimers = new CancellationTokenSource();


            var token = _cancellationTokenSourceTimers.Token;
            _taskTimers = CalculateDelayAsync(applicationUser, token);
        }
        public async Task RemoveTimersAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
        {
            _cancellationTokenSourceTimers.Cancel();
            _cancellationTokenSourceTimers.Dispose();
            _cancellationTokenSourceTimers = new CancellationTokenSource();

            var token = _cancellationTokenSourceTimers.Token;
            await RemoveDelayAsync(applicationUser, token);
        }
        public async Task RemoveDelayAsync(ApplicationUser applicationUser, CancellationToken token)

        {
            int id = applicationUser.Id;
            _usersWithNextUpdateTime.Remove(id);
            _taskTimers = WaitForNextShiftAsync(token);

        }
        public async Task<TimeSpan> FindLowestDelayAsync(List<WorkPattern> listWorkPatterns, DateTime now, CancellationToken cancellationToken)

        {
            List<TimeSpan> delays = new();
            if (listWorkPatterns != null)
            {
                foreach (var workPattern in listWorkPatterns)
                {
                    int userId = listWorkPatterns[0].UserId;
                    bool insideWorkPattern = await CheckIfInsideWorkPatternAsync(workPattern, cancellationToken);
                    if (insideWorkPattern)
                    {
                        List<WorkPatternPart> listWorkPatternWorkPatterns = workPattern.Parts;

                        DayOfWeek dayNextShift = new DayOfWeek();

                        for (int dayIndex = 0; dayIndex < 8; dayIndex++)
                        {
                            dayNextShift = now.AddDays(dayIndex).DayOfWeek;
                            var nextListWorkPatterns = listWorkPatternWorkPatterns.FindAll(y => y.Day == dayNextShift);
                            if (nextListWorkPatterns.Any())
                            {

                                List<Absent> listAbsentPerDay = _listAllAbsents
                                .Where(x => x.StartDate.Date == now.Date.AddDays(dayIndex))
                                .Where(y => y.UserId == userId)
                                .ToList();

                                nextListWorkPatterns = await CheckAbsentAndOverLapAsync(listWorkPatternWorkPatterns, listAbsentPerDay, nextListWorkPatterns, userId, cancellationToken);
                                int countWorkPatterns = nextListWorkPatterns.Count;

                                foreach (var part in nextListWorkPatterns)
                                {
                                    TimeSpan nowTimeOfDay = now.TimeOfDay;
                                    DayOfWeek nowDayOfWeek = now.DayOfWeek;
                                    TimeSpan start = part.StartTime;
                                    TimeSpan end = part.EndTime;

                                    if (dayIndex == 0)
                                    {
                                        if ((nowTimeOfDay <= start))
                                        {
                                            delays.Add(TimeSpan.FromDays(dayIndex) + start - nowTimeOfDay);
                                        }
                                        else if ((nowTimeOfDay < end) & (nowTimeOfDay >= start))
                                        {
                                            delays.Add(TimeSpan.FromDays(dayIndex) + end - nowTimeOfDay);
                                        }
                                        else if ((nowTimeOfDay >= end))
                                        {
                                            TimeSpan beginNextShift = await FindNextPartJumpOneDayAsync(dayIndex, listWorkPatternWorkPatterns, now, cancellationToken);
                                            delays.Add(TimeSpan.FromHours(24) - nowTimeOfDay + beginNextShift);
                                        }
                                    }
                                    else
                                    {
                                        TimeSpan beginNextShift = await FindNextPartAsync(dayIndex, listWorkPatternWorkPatterns, now, cancellationToken);
                                        delays.Add(TimeSpan.FromHours(24) - nowTimeOfDay + beginNextShift);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            TimeSpan lowestDelay = delays.Any() ? delays.Min() : TimeSpan.FromSeconds(999999999);

            return lowestDelay;
        }
        private async Task<TimeSpan> FindNextPartAsync(int dayIndex, List<WorkPatternPart> listWorkPatternWorkPatterns, DateTime now, CancellationToken cancellationToken)
        {
            TimeSpan lowestStart = TimeSpan.FromHours(24);
            DayOfWeek dayNextNextShift = new DayOfWeek();
            int index = 0;
            for (index = dayIndex; index < 8; index++)
            {
                dayNextNextShift = now.AddDays(index).DayOfWeek;
                var nextNextListWorkPatterns = listWorkPatternWorkPatterns.FindAll(x => x.Day == dayNextNextShift);
                if (nextNextListWorkPatterns.Any())
                {
                    foreach (var part in nextNextListWorkPatterns)
                    {
                        TimeSpan nowTimeOfDay = now.TimeOfDay;
                        TimeSpan start = part.StartTime;
                        TimeSpan end = part.EndTime;

                        if (start < lowestStart)
                        {
                            lowestStart = start;
                        }
                    }
                    break;
                }
            }
            return (lowestStart + TimeSpan.FromDays(index - 1));
        }
        private async Task<TimeSpan> FindNextPartJumpOneDayAsync(int dayIndex, List<WorkPatternPart> listWorkPatternWorkPatterns, DateTime now, CancellationToken cancellationToken)
        {
            TimeSpan lowestStart = TimeSpan.FromHours(24);
            DayOfWeek dayNextNextShift = new DayOfWeek();
            int index = 0;
            for (index = dayIndex + 1; index < 8; index++)
            {
                dayNextNextShift = now.AddDays(index).DayOfWeek;
                var nextNextListWorkPatterns = listWorkPatternWorkPatterns.FindAll(x => x.Day == dayNextNextShift);
                if (nextNextListWorkPatterns.Any())
                {
                    foreach (var part in nextNextListWorkPatterns)
                    {
                        TimeSpan nowTimeOfDay = now.TimeOfDay;
                        TimeSpan start = part.StartTime;
                        TimeSpan end = part.EndTime;

                        if (start < lowestStart)
                        {
                            lowestStart = start;
                        }
                    }
                    break;
                }
            }
            return (lowestStart + TimeSpan.FromDays(index - 1));
        }
        public async Task<bool> CheckIfInsideWorkPatternAsync(WorkPattern workPattern, CancellationToken cancellationToken)
        {
            DateTime today = DateTime.Today.Date;
            DateTime beginWorkPattern = workPattern.StartDate.Date;
            DateTime? endWorkPattern = workPattern.EndDate;
            bool insideWorkPattern = false;

            if (today >= beginWorkPattern & today <= endWorkPattern)
            {
                insideWorkPattern = true;
            }
            else if (today >= beginWorkPattern & endWorkPattern == null)
            {
                insideWorkPattern = true;
            }

            return insideWorkPattern;
        }
        public async Task<List<WorkPatternPart>> CheckAbsentAndOverLapAsync(List<WorkPatternPart> listWorkPatternWorkPatterns, List<Absent> listAbsentPerDay, List<WorkPatternPart> nextListWorkPatterns, int userId, CancellationToken cancellationToken)
        {
            listWorkPatternWorkPatterns = listWorkPatternWorkPatterns.OrderBy(x => x.EndTime).ToList();
            listWorkPatternWorkPatterns = listWorkPatternWorkPatterns.OrderBy(x => x.StartTime).ToList();
            List<WorkPatternPart> newListTodayToAdd = new List<WorkPatternPart>();
            WorkPatternPart previousPart = null;

            if (listAbsentPerDay.Any())
            {
                nextListWorkPatterns = await CheckIfPartSplitAsync(listWorkPatternWorkPatterns, listAbsentPerDay, userId, cancellationToken);
            }

            nextListWorkPatterns = nextListWorkPatterns.OrderBy(x => x.EndTime).ToList();
            nextListWorkPatterns = nextListWorkPatterns.OrderBy(x => x.StartTime).ToList();

            foreach (var part in nextListWorkPatterns)
            {
                (newListTodayToAdd, previousPart) = await OverLapTodayScheduleAsync(part, previousPart, newListTodayToAdd, cancellationToken);
            }

            return newListTodayToAdd;
        }
        public async Task<(List<WorkPatternPart>, WorkPatternPart)> OverLapTodayScheduleAsync(WorkPatternPart part, WorkPatternPart previousPart, List<WorkPatternPart> newListTodayToAdd, CancellationToken cancellationToken)
        {
            var startTime = part.StartTime;
            var endTime = part.EndTime;

            if (!newListTodayToAdd.Any())
            {
                newListTodayToAdd.Add(part);
                previousPart = part;
                return (newListTodayToAdd, previousPart);
            }

            var startPrevious = previousPart.StartTime;
            var endPrevious = previousPart.EndTime;

            newListTodayToAdd = newListTodayToAdd.OrderBy(x => x.StartTime).ToList();
            newListTodayToAdd = newListTodayToAdd.OrderBy(x => x.EndTime).ToList();


            if (startTime <= endPrevious & endTime >= startPrevious & endTime >= endPrevious)
            {
                newListTodayToAdd.Remove(previousPart);
                previousPart.StartTime = startPrevious;
                previousPart.EndTime = endTime;
                newListTodayToAdd.Add(previousPart);
            }
            else if (endTime <= endPrevious)
            {
            }
            else
            {
                newListTodayToAdd.Add(part);
                previousPart = part;
            }

            return (newListTodayToAdd, previousPart);

        }
        public async Task<List<WorkPatternPart>> CheckIfPartSplitAsync(List<WorkPatternPart> listWorkPatternWorkPatterns, List<Absent> listAbsentPerDay, int userId, CancellationToken cancellationToken)
        {
            DateTime today = DateTime.Today.Date;
            var newListWorkPatternWorkPatternsToAdd = new List<WorkPatternPart>();
            var newListWorkPatternWorkPatternsFOR = new List<WorkPatternPart>();
            var finalListWorkPatternWorkPatterns = new List<WorkPatternPart>();

            foreach (var workPatternPart in listWorkPatternWorkPatterns)
            {
                var workPatternPartToAdd = new WorkPatternPart
                {
                    Id = workPatternPart.Id,
                    Day = workPatternPart.Day,
                    StartTime = workPatternPart.StartTime,
                    EndTime = workPatternPart.EndTime,
                };
                newListWorkPatternWorkPatternsToAdd.Add(workPatternPartToAdd);
                newListWorkPatternWorkPatternsFOR.Add(workPatternPartToAdd);

                int indexAbsent = 0;
                finalListWorkPatternWorkPatterns = await BreakWorkPatternAsync(listAbsentPerDay, newListWorkPatternWorkPatternsToAdd, newListWorkPatternWorkPatternsFOR, indexAbsent, cancellationToken);
            }
            return finalListWorkPatternWorkPatterns;
        }
        public async Task<List<WorkPatternPart>> BreakWorkPatternAsync(List<Absent> listAbsentToday, List<WorkPatternPart> listWorkPatternWorkPatternsToAdd, List<WorkPatternPart> listWorkPatternWorkPatternsFOR, int indexAbsent, CancellationToken cancellationToken)
        {
            List<WorkPatternPart> newListWorkPatternPartToAdd = listWorkPatternWorkPatternsToAdd;
            List<WorkPatternPart> newListWorkPatternPartFOR = listWorkPatternWorkPatternsFOR;
            List<WorkPatternPart> finalListWorkPatternWorkPatterns = new List<WorkPatternPart>();

            int numberAbsent = listAbsentToday.Count();
            Absent absent = listAbsentToday[indexAbsent];
            TimeSpan startAbsent = absent.StartDate.TimeOfDay;
            TimeSpan endAbsent = absent.EndDate.TimeOfDay;


            newListWorkPatternPartFOR.Clear();
            foreach (var part in newListWorkPatternPartToAdd)
            {
                newListWorkPatternPartFOR.Add(part);
            }

            foreach (var part in newListWorkPatternPartFOR)
            {
                TimeSpan startPart = part.StartTime;
                TimeSpan endPart = part.EndTime;

                if ((startPart >= startAbsent) & (endPart > endAbsent) & (startPart <= endAbsent) & (endPart >= startAbsent))
                {
                    newListWorkPatternPartToAdd.Remove(part);
                    part.StartTime = endAbsent;
                    newListWorkPatternPartToAdd.Add(part);
                }
                else if ((startPart < startAbsent) & (endPart <= endAbsent) & (startPart <= endAbsent) & (endPart >= startAbsent))
                {
                    newListWorkPatternPartToAdd.Remove(part);
                    part.EndTime = startAbsent;
                    newListWorkPatternPartToAdd.Add(part);
                }
                else if ((startPart < startAbsent) & (endPart > endAbsent))
                {
                    newListWorkPatternPartToAdd.Remove(part);

                    var workPatternsplit1 = new WorkPatternPart
                    {
                        Id = new Guid(),
                        Day = part.Day,
                        StartTime = startPart,
                        EndTime = startAbsent,
                    };

                    var workPatternsplit2 = new WorkPatternPart
                    {
                        Id = new Guid(),
                        Day = part.Day,
                        StartTime = endAbsent,
                        EndTime = endPart,
                    };
                    newListWorkPatternPartToAdd.Add(workPatternsplit1);
                    newListWorkPatternPartToAdd.Add(workPatternsplit2);
                }
                else if ((startPart >= startAbsent) & (startPart <= endAbsent))
                {
                    newListWorkPatternPartToAdd.Remove(part);
                }
            }

            indexAbsent += 1;
            if (indexAbsent < numberAbsent)
            {
                newListWorkPatternPartFOR.Clear();
                foreach (var part in newListWorkPatternPartToAdd)
                {
                    newListWorkPatternPartFOR.Add(part);
                }
                newListWorkPatternPartToAdd = await BreakWorkPatternAsync(listAbsentToday, newListWorkPatternPartToAdd, newListWorkPatternPartFOR, indexAbsent, cancellationToken);
                return newListWorkPatternPartToAdd;
            }
            else
            {
                return newListWorkPatternPartToAdd;
            }
        }
        public void Dispose()
        {
            _cancellationTokenSourceTimers.Cancel();
            _cancellationTokenSourceTimers.Dispose();
        }        
    }
}
