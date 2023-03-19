using FluentAssertions;
using Project1.Models;
using Project1.Timers;
using Xunit;

namespace PVSDashboard.Tests.Timers
{
    public class CheckIfInsideWorkPatternTests
    {
        private readonly TimerService _timerService;

        public CheckIfInsideWorkPatternTests()
        {
            _timerService = new TimerService(null, null, null);
        }

        [Fact(DisplayName = " CheckIfInsideWorkPatternAsync should return True")]
        public async Task CheckIfInsideWorkPatternAsyncTestingPatternStartsTodayEndsToday()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;
            var listWorkPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(6),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(10),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
            };

            var workPattern = new WorkPattern
            {
                Parts = listWorkPatternParts,
                StartDate = now,
                EndDate = now,
            };

            // Act
            var response = await _timerService.CheckIfInsideWorkPatternAsync(workPattern, CancellationToken.None);

            //Assert

            response.Should().BeTrue();

        }


        [Fact(DisplayName = " CheckIfInsideWorkPatternAsync should return True")]
        public async Task CheckIfInsideWorkPatternAsyncTestingPatternStartsTodayEndsTomorrow()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;
            var listWorkPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(6),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(10),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
            };

            var workPattern = new WorkPattern
            {
                Parts = listWorkPatternParts,
                StartDate = now,
                EndDate = now.AddDays(1),
            };

            // Act
            var response = await _timerService.CheckIfInsideWorkPatternAsync(workPattern, CancellationToken.None);

            //Assert

            response.Should().BeTrue();

        }

        [Fact(DisplayName = " CheckIfInsideWorkPatternAsync should return True")]
        public async Task CheckIfInsideWorkPatternAsyncTestingPatternStartsYesterdayEndsToday()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;
            var listWorkPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(6),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(10),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
            };

            var workPattern = new WorkPattern
            {
                Parts = listWorkPatternParts,
                StartDate = now.AddDays(-1),
                EndDate = now,
            };

            // Act
            var response = await _timerService.CheckIfInsideWorkPatternAsync(workPattern, CancellationToken.None);

            //Assert

            response.Should().BeTrue();

        }

        [Fact(DisplayName = " CheckIfInsideWorkPatternAsync should return False")]
        public async Task CheckIfInsideWorkPatternAsyncTestingPatternStartsYesterdayEndsYesterday()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;
            var listWorkPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(6),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(10),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
            };

            var workPattern = new WorkPattern
            {
                Parts = listWorkPatternParts,
                StartDate = now.AddDays(-1),
                EndDate = now.AddDays(-1),
            };

            // Act
            var response = await _timerService.CheckIfInsideWorkPatternAsync(workPattern, CancellationToken.None);

            //Assert

            response.Should().BeFalse();

        }

        [Fact(DisplayName = " CheckIfInsideWorkPatternAsync should return False")]
        public async Task CheckIfInsideWorkPatternAsyncTestingPatternStartsTomorrowEndsTomorrow()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;
            var listWorkPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(6),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(10),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
            };

            var workPattern = new WorkPattern
            {
                Parts = listWorkPatternParts,
                StartDate = now.AddDays(1),
                EndDate = now.AddDays(1),
            };

            // Act
            var response = await _timerService.CheckIfInsideWorkPatternAsync(workPattern, CancellationToken.None);

            //Assert

            response.Should().BeFalse();

        }


    }
}

