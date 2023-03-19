using System.Collections.Generic;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using Project1.Models;

namespace Project1.Infrastructure
{
    public class UnsService : IUnsService
    {

        public List<TodaySchedule> _todayList;
        public bool _scheduleToWorkNow;
        public AbsentEachDay _listAbsentEachDay;
        public List<Absent> _listAllAbsents;

        public List<TodaySchedule> testNewListTodayToAdd;

        private readonly IMqttService _mqttService;

        public UnsService(IMqttService mqttService)
        {
            _mqttService = mqttService;
        }

        public async Task CallEachCheckInAsync(List<ApplicationUser> allUsers,DateTime now, CancellationToken cancellationToken)
        {
            foreach (var user in allUsers)
            {
                await PublishCheckInAsync(user,now,cancellationToken);
            }
        }
        public async Task PublishCheckInAsync(ApplicationUser checkInUser,DateTime now, CancellationToken cancellationToken)

        {
            CheckInPayLoad messageJson = new CheckInPayLoad()
            {
                Value = checkInUser.CheckedIn,
                Timestamp = now
            };
            string payLoad1 = JsonConvert.SerializeObject(messageJson);
            string topic1 = $"users/{checkInUser.Id}/pvs/checked_in";
            await PublishOnTopicAsync(payLoad1, topic1, cancellationToken);

            string payLoad2 = checkInUser.FirstName;
            string topic2 = $"users/{checkInUser.Id}/name";
            await PublishOnTopicAsync(payLoad2, topic2, cancellationToken);
        }
        public async Task DeleteTopicApplicationUserAsync(ApplicationUser deletedUser, CancellationToken cancellationToken)

        {
            string payLoad = null;
            string userId = Convert.ToString(deletedUser.Id);
            string userOfficeLocation = Convert.ToString(deletedUser.OfficeLocation);
            List<string> listTopics = new List<string>();

            listTopics.Add($"users/{userId}/pvs/checked_in");
            listTopics.Add($"locations/{userOfficeLocation}/pvs/mechanics");
            listTopics.Add($"users/{userId}/pvs/schedule");
            listTopics.Add($"users/{userId}/name");
            listTopics.Add($"users/{userId}/pvs/schedule/shift_today");
            listTopics.Add($"users/{userId}/pvs/schedule/absent_today");

            foreach (var topic in listTopics)
            {
                await PublishOnTopicAsync(payLoad, topic, cancellationToken);
            }
        }
        public async Task CallEachApplicationUserAsync(List<ApplicationUser> listApplicationUsers, CancellationToken cancellationToken)

        {
            try
            {
                foreach (var applicationUser in listApplicationUsers)
                {
                    await CallListWorkPatternAsync(applicationUser, cancellationToken);
                }
            }
            catch (NullReferenceException except)
            {
            }
        }
        public async Task CallListWorkPatternAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
        {
            List<WorkPattern> listWorkPatterns = applicationUser.WorkPatterns;
            var now = DateTime.UtcNow;
            try
            {
                _todayList = new List<TodaySchedule>();
                _scheduleToWorkNow = false;

                await CallEachWorkPatternAsync(listWorkPatterns, cancellationToken);

                int id = applicationUser.Id;
                bool varScheduleToWorkNow = _scheduleToWorkNow;
                bool userAbsentToday = false;

                TodaySchedule previousToday = null;
                List<TodaySchedule> newListTodayToAdd = new List<TodaySchedule>();
                List<Absent> listTodayAbsent = new List<Absent>();
                _todayList = _todayList.OrderBy(x => x.From).ToList();


                foreach (var today in _todayList)
                {
                    (newListTodayToAdd, previousToday) = await OverLapTodayScheduleAsync(today,previousToday,newListTodayToAdd,cancellationToken);
                }


                if ( (_listAbsentEachDay is not null ) & (_listAbsentEachDay.ListIds.Contains(id) ))
                {
                    listTodayAbsent = _listAllAbsents.FindAll(x => ((x.UserId == id) & (x.StartDate.Date == now.Date) & (x.EndDate.Date == now.Date)));
                }

                await PublishAbsentsAsync(listTodayAbsent,id, cancellationToken);
                await PublishTodayScheduleAsync(id, newListTodayToAdd, cancellationToken);
                await PublishScheduleToWorkNowAsync(id, varScheduleToWorkNow, cancellationToken);
            }
            catch
            {

            }
        }
        public async Task CallEachWorkPatternAsync(List<WorkPattern> listWorkPatterns, CancellationToken cancellationToken)
        {
            if (listWorkPatterns is not null)
            {
                foreach (var workPattern in listWorkPatterns)
                {
                    bool insideWorkPattern = await CheckIfInsideWorkPatternAsync(workPattern, cancellationToken);

                    if (insideWorkPattern)
                        await CallListWorkPatternPartAsync(workPattern, cancellationToken);
                }

            };
        }
        public async Task CallListWorkPatternPartAsync(WorkPattern workPattern, CancellationToken cancellationToken)
        {
            if (workPattern is not null)
            {
                List<WorkPatternPart> listWorkPatternParts = workPattern.Parts;
                List<WorkPatternPart> newListWorkPatternParts = listWorkPatternParts;
                int userId = workPattern.UserId;
                bool userAbsentToday = false;
                if (_listAbsentEachDay != null)
                {
                    userAbsentToday = _listAbsentEachDay.ListIds.Contains(userId);
                }

                if (userAbsentToday)
                {
                    newListWorkPatternParts = await CheckIfPartSplitAsync(listWorkPatternParts, _listAllAbsents, userId, cancellationToken);
                }
                await CallEachWorkPatternPartAsync(newListWorkPatternParts, cancellationToken);
            };
        }
        public async Task CallEachWorkPatternPartAsync(List<WorkPatternPart> listWorkPatternParts, CancellationToken cancellationToken)

        {
            if (listWorkPatternParts is not null)
            {
                foreach (var workPatternPart in listWorkPatternParts)
                {
                    await ScheduledTodayAsync(workPatternPart, cancellationToken);
                    await ScheduledToWorkNowAsync(workPatternPart, cancellationToken);
                }
            }

        }
        public async Task ScheduledToWorkNowAsync(WorkPatternPart workPatternPart, CancellationToken cancellationToken)

        {
            TimeSpan startTime = workPatternPart.StartTime;
            TimeSpan? endTime = workPatternPart.EndTime;
            TimeSpan currentTime = DateTime.UtcNow.TimeOfDay;
            DayOfWeek today = DateTime.UtcNow.DayOfWeek;


            if (startTime <= currentTime & endTime >= currentTime & workPatternPart.Day == today)
            {
                _scheduleToWorkNow = true;
            }
        }
        public async Task ScheduledTodayAsync(WorkPatternPart workPatternPart, CancellationToken cancellationToken)

        {
            DayOfWeek dayOfWeekToday = DateTime.Today.DayOfWeek;
            TodaySchedule messageJson = new TodaySchedule()
            {
                From = TimeSpan.Zero,
                Till = TimeSpan.Zero
            };

            if (workPatternPart.Day == dayOfWeekToday)
            {
                messageJson.From = workPatternPart.StartTime;
                messageJson.Till = workPatternPart.EndTime;
            }
            else
            {
                messageJson = null;
            }

            if (messageJson != null)
            {
                _todayList.Add(messageJson);
            }
        }
        private async Task<bool> CheckIfInsideWorkPatternAsync(WorkPattern workPattern, CancellationToken cancellationToken)
        {
            DateTime today = DateTime.Today.Date;
            DateTime beginWorkPattern = workPattern.StartDate.Date;
            DateTime? endWorkPattern = workPattern.EndDate;
            bool insideWorkPattern;

            if (today >= beginWorkPattern & today <= endWorkPattern)
            {
                insideWorkPattern = true;
            }
            else
            {
                insideWorkPattern = false;
            }

            return insideWorkPattern;

        }
        public async Task CheckTodayAbsentsAsync(List<Absent> listAllAbsents, CancellationToken cancellationToken)
        {

            int todayDayOfYear = DateTime.UtcNow.DayOfYear;
            DayOfWeek todayDayOfWeek = DateTime.UtcNow.DayOfWeek;
            var utcNow = DateTime.UtcNow;

            List<Absent> absentsChecked = await CheckIfAbsentNeedsSplitAsync(listAllAbsents,utcNow, cancellationToken);

            List<AbsentEachDay> absentEachDay = listAllAbsents
               .GroupBy(p => p.StartDate.DayOfYear)
               .Select(g => new AbsentEachDay(g.Key, g.Select(x => x.UserId).ToList()))
               .ToList();


            _listAbsentEachDay = absentEachDay.FirstOrDefault(x => x.DayOfYear == todayDayOfYear);

            if (_listAbsentEachDay is null)
            {
                var absentEachDayEmpty = new AbsentEachDay(todayDayOfYear,new List<int>());
                _listAbsentEachDay = absentEachDayEmpty;
            }

            _listAllAbsents = listAllAbsents;

        }
        public async Task<List<Absent>> CheckIfAbsentNeedsSplitAsync(List<Absent> listAllAbsents,DateTime utcNow, CancellationToken cancellationToken)
        {

            int todayDayOfYear = utcNow.DayOfYear;
            DayOfWeek todayDayOfWeek = utcNow.DayOfWeek;
            List<Absent> newListAllAbsents = new List<Absent>();

            foreach (var absent in listAllAbsents)
            {
                var todayDate = utcNow.Date;
                var endDate = absent.EndDate.Date;
                var startDate = absent.StartDate.Date;

                if (endDate > startDate & startDate <= todayDate & todayDate <= endDate)
                {
                    if (startDate == todayDate)
                    {
                        var absentToAdd = absent;
                        absentToAdd.EndDate = startDate.AddDays(1);
                        newListAllAbsents.Add(absentToAdd);
                    }
                    else if (endDate == todayDate)
                    {
                        var absentToAdd = absent;
                        absentToAdd.StartDate = endDate;
                        newListAllAbsents.Add(absentToAdd);
                    }
                    else
                    {
                        var absentToAdd = absent;
                        absentToAdd.StartDate = todayDate;
                        absentToAdd.EndDate = todayDate.AddDays(1).AddMilliseconds(-1);
                        newListAllAbsents.Add(absentToAdd);
                    }

                }
                else
                {
                    newListAllAbsents.Add(absent);
                }

            }
            return newListAllAbsents;

        }
        public async Task<List<WorkPatternPart>> CheckIfPartSplitAsync(List<WorkPatternPart> listWorkPatternParts, List<Absent> absents, int userId, CancellationToken cancellationToken)
        {
            DateTime today = DateTime.Today.Date;
            var newListWorkPatternPartsToAdd = new List<WorkPatternPart>();
            var newListWorkPatternPartsFOR = new List<WorkPatternPart>();
            var finalListWorkPatternParts = new List<WorkPatternPart>();

            List<Absent> listAbsentToday = absents
                .Where(x => x.StartDate.Date == today)
                .Where(x => x.UserId == userId)
                .ToList();

            List<WorkPatternPart> workPatternPartsForToday = listWorkPatternParts
                .Where(x => x.Day == today.DayOfWeek)
                .ToList();

            foreach (var workPatternPart in workPatternPartsForToday)
            {
                var workPatternPartToAdd = new WorkPatternPart
                {
                    Id = workPatternPart.Id,
                    Day = workPatternPart.Day,
                    StartTime = workPatternPart.StartTime,
                    EndTime = workPatternPart.EndTime,
                };

                newListWorkPatternPartsToAdd.Add(workPatternPartToAdd);
                newListWorkPatternPartsFOR.Add(workPatternPartToAdd);

                if (listAbsentToday.Any())
                {
                    int indexTodaySchedule = 0;
                    finalListWorkPatternParts = await BreakWorkPatternAsync(listAbsentToday, newListWorkPatternPartsToAdd, newListWorkPatternPartsFOR, indexTodaySchedule, cancellationToken);

                }
                else
                {
                    return newListWorkPatternPartsToAdd;
                }

            }
            return finalListWorkPatternParts;
        }
        public async Task<List<WorkPatternPart>> BreakWorkPatternAsync(List<Absent> listAbsentToday, List<WorkPatternPart> listWorkPatternPartsToAdd, List<WorkPatternPart> listWorkPatternPartsFOR, int indexAbsent, CancellationToken cancellationToken)
        {
            List<WorkPatternPart> newListWorkPatternPartToAdd = listWorkPatternPartsToAdd;
            List<WorkPatternPart> newListWorkPatternPartFOR = listWorkPatternPartsFOR;
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

                if ((startPart >= startAbsent) & (endPart >= endAbsent) & (startPart <= endAbsent) & (endPart >= startAbsent))
                {
                    newListWorkPatternPartToAdd.Remove(part);
                    part.StartTime = endAbsent;
                    newListWorkPatternPartToAdd.Add(part);
                }
                else if ((startPart <= startAbsent) & (endPart <= endAbsent) & (startPart <= endAbsent) & (endPart >= startAbsent))
                {
                    newListWorkPatternPartToAdd.Remove(part);
                    part.EndTime = startAbsent;
                    newListWorkPatternPartToAdd.Add(part);
                }
                else if ((startPart <= startAbsent) & (endPart >= endAbsent))
                {
                    newListWorkPatternPartToAdd.Remove(part);

                    var partSplit1 = new WorkPatternPart
                    {
                        Id = new Guid(),
                        Day = part.Day,
                        StartTime = startPart,
                        EndTime = startAbsent,
                    };

                    var partSplit2 = new WorkPatternPart
                    {
                        Id = new Guid(),
                        Day = part.Day,
                        StartTime = endAbsent,
                        EndTime = endPart,
                     };
                    newListWorkPatternPartToAdd.Add(partSplit1);
                    newListWorkPatternPartToAdd.Add(partSplit2);
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
        public async Task<(List<TodaySchedule>, TodaySchedule)> OverLapTodayScheduleAsync(TodaySchedule today, TodaySchedule previousToday, List<TodaySchedule> newListTodayToAdd, CancellationToken cancellationToken)
        {
            var from = today.From;
            var till = today.Till;

            if (!newListTodayToAdd.Any())
            {
                newListTodayToAdd.Add(today);
                previousToday = today;
                return (newListTodayToAdd, previousToday);
            }

            var fromPrevious = previousToday.From;
            var tillPrevious = previousToday.Till;

            newListTodayToAdd = newListTodayToAdd.OrderBy(x => x.Till).ToList();
            newListTodayToAdd = newListTodayToAdd.OrderBy(x => x.From).ToList();

            if (from <= tillPrevious & till >= fromPrevious & till >= tillPrevious)
            {
                newListTodayToAdd.Remove(previousToday);
                previousToday.From = fromPrevious;
                previousToday.Till = till;
                newListTodayToAdd.Add(previousToday);
            }
            else if (till <= tillPrevious)
            {
            }
            else
            {
                newListTodayToAdd.Add(today);
                previousToday = today;
            }
            return (newListTodayToAdd,previousToday);
        }
        public async Task PostAllLocationAsync(List<UsersEachLocation> location, CancellationToken cancellationToken)
        {
            foreach (UsersEachLocation loc in location)
                await PublishLocationAsync(loc, loc.OfficeLocation, cancellationToken);
        }
        public async Task PublishScheduleToWorkNowAsync(int id, bool varScheduleToWorkNow, CancellationToken cancellationToken)
        {
            SchedulePayLoad workNow = new SchedulePayLoad();
            if (varScheduleToWorkNow is true)
            {
                workNow.Value = true;
                workNow.Timestamp = DateTime.UtcNow;

            }
            else
            {
                workNow.Value = false;
                workNow.Timestamp = DateTime.UtcNow;
            }

            string messageJson = JsonConvert.SerializeObject(workNow);
            string topic = $"users/{Convert.ToString(id)}/pvs/schedule";
            await PublishOnTopicAsync(messageJson, topic, cancellationToken);
        }
        public async Task PublishTodayScheduleAsync(int id, List<TodaySchedule> todayList, CancellationToken cancellationToken)

        {
            string todaySchedule = JsonConvert.SerializeObject(todayList);
            string topic = $"users/{Convert.ToString(id)}/pvs/schedule/shift_today";
            await PublishOnTopicAsync(todaySchedule, topic, cancellationToken);
        }
        public async Task PublishLocationAsync(UsersEachLocation location, int officeLocationId, CancellationToken cancellationToken)

        {
            List<int> payLoad = new List<int>();
            string combinedPayLoad = "";

            if (location is not null)
            {
                payLoad = location.UserIds;
                combinedPayLoad = JsonConvert.SerializeObject(payLoad);
            }

            string topic = $"locations/{Convert.ToString(officeLocationId)}/pvs/mechanics";
            await PublishOnTopicAsync(combinedPayLoad, topic, cancellationToken);
        }
        public async Task PublishAbsentsAsync(List<Absent> listTodayAbsent,int id, CancellationToken cancellationToken)

        {
            List<AbsentPayLoad> listPayload= new List<AbsentPayLoad>();

            foreach (var todayAbsent in listTodayAbsent)
            {
                var reason = "";
                if (todayAbsent is not null)
                {
                    reason = todayAbsent.Description;
                }

                var payload = new AbsentPayLoad
                {
                    From = todayAbsent.StartDate.TimeOfDay,
                    Till = todayAbsent.EndDate.TimeOfDay,
                    Reason = reason
                };
                listPayload.Add(payload);
            }

            string combinedPayLoad = JsonConvert.SerializeObject(listPayload);
            string topic = $"users/{Convert.ToString(id)}/pvs/schedule/absent_today";
            await PublishOnTopicAsync(combinedPayLoad, topic, cancellationToken);
        }

        private Task PublishOnTopicAsync(string payLoad, string topic, CancellationToken cancellationToken)        
            => _mqttService.PublishOnTopicAsync(payLoad, topic, cancellationToken);
    }
}

