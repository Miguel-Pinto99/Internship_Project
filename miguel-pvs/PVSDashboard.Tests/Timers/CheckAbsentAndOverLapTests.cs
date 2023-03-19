using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Timers;
using Xunit;

namespace PVSDashboard.Tests.Timers
{
    public class CheckAbsentAndOverLapTests
    {
        private readonly TimerService _timerService;

        public CheckAbsentAndOverLapTests()
        {
            _timerService = new TimerService(null, null, null);
        }

        [Fact(DisplayName = "CheckAbsentAndOverLapAsync should return two parts")]
        public async Task CheckAbsentAndOverLapAsyncTestingEdges_PatternHasFivePartAndNoAbsents()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;

            var userId = 1;

            var listWorkPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(5),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(10),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(2),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                 new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(5),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(9),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
            };

            var listAbsents = new List<Absent>();


            // Act
            var response = await _timerService.CheckAbsentAndOverLapAsync(listWorkPatternParts, listAbsents, listWorkPatternParts, userId, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(2);

            response = response.OrderBy(x => x.StartTime).ToList();

            WorkPatternPart reponseWorkPatternPart1 = response[0];
            reponseWorkPatternPart1.StartTime.Should().Be(TimeSpan.FromHours(1));
            reponseWorkPatternPart1.EndTime.Should().Be(TimeSpan.FromHours(9));

            WorkPatternPart reponseWorkPatternPart2 = response[1];
            reponseWorkPatternPart2.StartTime.Should().Be(TimeSpan.FromHours(10));
            reponseWorkPatternPart2.EndTime.Should().Be(TimeSpan.FromHours(20));

        }

        [Fact(DisplayName = "CheckAbsentAndOverLapAsync should return two parts")]
        public async Task CheckAbsentAndOverLapAsyncTestingEdgesAbsents_PatternHasTwiPartAndThreeAbsents()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;

            var userId = 1;

            var listWorkPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
            };

            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,3,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,4,0,0),
                },
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,6,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,8,0,0),
                },
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,1,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,2,0,0),
                },
            };


            // Act
            var response = await _timerService.CheckAbsentAndOverLapAsync(listWorkPatternParts, listAbsents, listWorkPatternParts, userId, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(2);

            response = response.OrderBy(x => x.StartTime).ToList();

            WorkPatternPart reponseWorkPatternPart1 = response[0];
            reponseWorkPatternPart1.StartTime.Should().Be(TimeSpan.FromHours(2));
            reponseWorkPatternPart1.EndTime.Should().Be(TimeSpan.FromHours(3));

            WorkPatternPart reponseWorkPatternPart2 = response[1];
            reponseWorkPatternPart2.StartTime.Should().Be(TimeSpan.FromHours(4));
            reponseWorkPatternPart2.EndTime.Should().Be(TimeSpan.FromHours(6));

        }

        [Fact(DisplayName = "CheckAbsentAndOverLapAsync should return zero parts")]
        public async Task CheckAbsentAndOverLapAsyncTestingInside_PatternHasFourPartAndFourAbsents()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;

            var userId = 1;

            var listWorkPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(5),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(7),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(5),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
            };

            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,4,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,8,0,0),
                },
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,4,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,5,0,0),
                },
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,7,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,8,0,0),
                },
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,5,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,6,0,0),
                },
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,20,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,21,0,0),
                },
            };


            // Act
            var response = await _timerService.CheckAbsentAndOverLapAsync(listWorkPatternParts, listAbsents, listWorkPatternParts, userId, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(0);

            response = response.OrderBy(x => x.StartTime).ToList();

        }

        [Fact(DisplayName = "CheckAbsentAndOverLapAsync should return zero")]
        public async Task CheckAbsentAndOverLapAsyncTestingInside_PatternHasFourPartAndOneAbsents()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;

            var userId = 1;

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

            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,4,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,12,0,0),
                }
            };


            // Act
            var response = await _timerService.CheckAbsentAndOverLapAsync(listWorkPatternParts, listAbsents, listWorkPatternParts, userId, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(0);

            response = response.OrderBy(x => x.StartTime).ToList();

        }


        [Fact(DisplayName = "CheckAbsentAndOverLapAsync should return zero")]
        public async Task CheckAbsentAndOverLapAsyncTestingAbsentFullDay_PatternHasFourPartAndOneAbsents()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;

            var userId = 1;

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
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(14),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
            };

            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,1,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,23,0,0),
                }
            };


            // Act
            var response = await _timerService.CheckAbsentAndOverLapAsync(listWorkPatternParts, listAbsents, listWorkPatternParts, userId, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(0);

            response = response.OrderBy(x => x.StartTime).ToList();

        }


        [Fact(DisplayName = "CheckAbsentAndOverLapAsync should return two parts")]
        public async Task CheckAbsentAndOverLapAsyncTestingInsidePatternsAndOverlaps_PatternHasFourPartAndFourAbsents()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;

            var userId = 1;

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
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(11),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(12),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(14),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
            };

            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,7,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,10,0,0),
                },
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,14,0,0),
                    EndDate = new DateTime(now.Year,now.Month,now.Day,15,0,0),
                }
            };


            // Act
            var response = await _timerService.CheckAbsentAndOverLapAsync(listWorkPatternParts, listAbsents, listWorkPatternParts, userId, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(4);

            response = response.OrderBy(x => x.StartTime).ToList();

            WorkPatternPart reponseWorkPatternPart1 = response[0];
            reponseWorkPatternPart1.StartTime.Should().Be(TimeSpan.FromHours(6));
            reponseWorkPatternPart1.EndTime.Should().Be(TimeSpan.FromHours(7));

            WorkPatternPart reponseWorkPatternPart2 = response[1];
            reponseWorkPatternPart2.StartTime.Should().Be(TimeSpan.FromHours(10));
            reponseWorkPatternPart2.EndTime.Should().Be(TimeSpan.FromHours(11));


            WorkPatternPart reponseWorkPatternPart3 = response[2];
            reponseWorkPatternPart3.StartTime.Should().Be(TimeSpan.FromHours(12));
            reponseWorkPatternPart3.EndTime.Should().Be(TimeSpan.FromHours(14));


            WorkPatternPart reponseWorkPatternPart4 = response[3];
            reponseWorkPatternPart4.StartTime.Should().Be(TimeSpan.FromHours(15));
            reponseWorkPatternPart4.EndTime.Should().Be(TimeSpan.FromHours(16));

        }
    }
}

