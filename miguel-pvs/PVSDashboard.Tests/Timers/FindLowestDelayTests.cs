using FluentAssertions;
using Project1.Models;
using Project1.Timers;
using Xunit;

namespace PVSDashboard.Tests.Timers
{
    public class FindLowestDelayTests
    {
        private readonly TimerService _timerService;

        public FindLowestDelayTests()
        {
            _timerService = new TimerService(null,null,null);
        }

        [Fact(DisplayName = "FindLowestDelayAsync should set the right delay when the application starts before the next part")]
        public async Task FindLowestDelayAsyncShouldReturnDelay_AppllicationStartsBeforeNextPart()
        {
            // Arrange
            DateTime now = new DateTime(2022, 11, 21, 00, 00, 00); //FRIDAY MIDNIGHT

            var userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(2),
                    Day = now.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = now.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = now.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = now.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = now.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = now.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = now.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now,
                    EndDate = now.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now.AddYears(1),
                    EndDate = now.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = new DateTime(2020, 01, 01, 00, 00, 00),
                    EndDate = new DateTime(2021, 01, 01, 00, 00, 00),
                    Parts = workPatternParts
                }
            };

            // Act
            TimeSpan response = await _timerService.FindLowestDelayAsync(workPattern,now,CancellationToken.None);

            //Assert
            response.Should().Be(TimeSpan.FromHours(1));

        }

        [Fact(DisplayName = "FindLowestDelayAsync should set the right delay when the application starts during part")]
        public async Task FindLowestDelayAsyncShouldReturnDelay_AppllicationStartsDuringPart()
        {
            // Arrange
            DateTime now = new DateTime(2022, 11, 21, 1, 30, 00); //FRIDAY MIDNIGHT

            var userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = now.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = now.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = now.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = now.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = now.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = now.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now,
                    EndDate = now.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now.AddYears(1),
                    EndDate = now.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = new DateTime(2020, 01, 01, 00, 00, 00),
                    EndDate = new DateTime(2021, 01, 01, 00, 00, 00),
                    Parts = workPatternParts
                }
            };

            // Act
            TimeSpan response = await _timerService.FindLowestDelayAsync(workPattern,now, CancellationToken.None);

            //Assert
            response.Should().Be(new TimeSpan(02,30,00));

        }

        [Fact(DisplayName = "FindLowestDelayAsync should set the right delay when the application starts after today´s part")]
        public async Task FindLowestDelayAsyncShouldReturnDelay_AppllicationStartsAfterTodaysPart()
        {
            // Arrange
            DateTime now = new DateTime(2022, 11, 21, 05, 00, 00); //MONDAY MIDNIGHT

            var userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(2),
                    Day = now.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = now.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = now.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = now.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = now.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = now.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = now.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now,
                    EndDate = now.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now.AddYears(1),
                    EndDate = now.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = new DateTime(2020, 01, 01, 00, 00, 00),
                    EndDate = new DateTime(2021, 01, 01, 00, 00, 00),
                    Parts = workPatternParts
                }
            };

            // Act
            TimeSpan response = await _timerService.FindLowestDelayAsync(workPattern,now, CancellationToken.None);

            //Assert
            response.Should().Be(TimeSpan.FromHours(44));

        }

        [Fact(DisplayName = "FindLowestDelayAsync should set the right delay when the application starts almost at midnight")]
        public async Task FindLowestDelayAsyncShouldReturnDelay_AppllicationStartsAlmostAtMidnightPart()
        {
            // Arrange
            DateTime now = new DateTime(2022, 11, 21, 23, 00, 00); //FRIDAY MIDNIGHT

            var userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(2),
                    Day = now.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = now.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = now.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = now.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = now.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = now.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = now.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now,
                    EndDate = now.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now.AddYears(1),
                    EndDate = now.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = new DateTime(2020, 01, 01, 00, 00, 00),
                    EndDate = new DateTime(2021, 01, 01, 00, 00, 00),
                    Parts = workPatternParts
                }
            };

            // Act
            TimeSpan response = await _timerService.FindLowestDelayAsync(workPattern, now, CancellationToken.None);

            //Assert
            response.Should().Be(TimeSpan.FromHours(62));

        }

        [Fact(DisplayName = "FindLowestDelayAsync should set a one week delay")]
        public async Task FindLowestDelayAsyncShouldReturnDelay_ReturnsAWeekDelay()
        {
            // Arrange
            DateTime now = new DateTime(2022, 11, 21, 15, 00, 00); //FRIDAY MIDNIGHT

            var userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(2),
                    Day = now.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(2),
                    Day = now.Date.AddDays(0).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.AddDays(0).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(7).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.AddDays(7).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now,
                    EndDate = now.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now.AddYears(1),
                    EndDate = now.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = new DateTime(2020, 01, 01, 00, 00, 00),
                    EndDate = new DateTime(2021, 01, 01, 00, 00, 00),
                    Parts = workPatternParts
                }
            };

            // Act
            TimeSpan response = await _timerService.FindLowestDelayAsync(workPattern, now, CancellationToken.None);

            //Assert
            response.Should().Be(TimeSpan.FromHours(154));

        }

        [Fact(DisplayName = "FindLowestDelayAsync should set the right delay  when the application starts before the next part plus 24h")]
        public async Task FindLowestDelayAsyncShouldReturnDelay_AppllicationStartsBeforeNextPartPlus24h()
        {
            // Arrange
            DateTime now = new DateTime(2022, 11, 21, 00, 00, 00); //FRIDAY MIDNIGHT

            var userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = now.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = now.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = now.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = now.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = now.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = now.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = now.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now,
                    EndDate = now.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now.AddYears(1),
                    EndDate = now.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = new DateTime(2020, 01, 01, 00, 00, 00),
                    EndDate = new DateTime(2021, 01, 01, 00, 00, 00),
                    Parts = workPatternParts
                }
            };

            // Act
            TimeSpan response = await _timerService.FindLowestDelayAsync(workPattern, now, CancellationToken.None);

            //Assert
            response.Should().Be(TimeSpan.FromHours(25));

        }

        [Fact(DisplayName = "FindLowestDelayAsync should set the right delay  when the application starts during the next part plus 24h")]
        public async Task FindLowestDelayAsyncShouldReturnDelay_AppllicationStartsDuringPartPlus24h()
        {
            // Arrange
            DateTime now = new DateTime(2022, 11, 21, 4, 00, 00); //FRIDAY MIDNIGHT

            var userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = now.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = now.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = now.Date.AddDays(4).DayOfWeek
                }

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now,
                    EndDate = now.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now.AddYears(1),
                    EndDate = now.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = new DateTime(2020, 01, 01, 00, 00, 00),
                    EndDate = new DateTime(2021, 01, 01, 00, 00, 00),
                    Parts = workPatternParts
                }
            };

            // Act
            TimeSpan response = await _timerService.FindLowestDelayAsync(workPattern, now, CancellationToken.None);

            //Assert
            response.Should().Be(TimeSpan.FromHours(24));

        }

        [Fact(DisplayName = "FindLowestDelayAsync should set the right delay  when pattern has one part")]
        public async Task FindLowestDelayAsyncShouldReturnDelay_PatternHasOnlyOnePart()
        {
            // Arrange
            DateTime now = new DateTime(2022, 11, 21, 5, 00, 00); //FRIDAY MIDNIGHT

            var userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(1).DayOfWeek // TOMORROW
                }
            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now,
                    EndDate = now.AddYears(1),
                    Parts = workPatternParts
                }
            };

            // Act
            TimeSpan response = await _timerService.FindLowestDelayAsync(workPattern, now, CancellationToken.None);

            //Assert
            response.Should().Be(TimeSpan.FromHours(20));

        }

        [Fact(DisplayName = "FindLowestDelayAsync should set the right delay  when pattern has five ten and tested on edges")]
        public async Task FindLowestDelayAsyncByTestingAllEdges_PatternHasFivePart()
        {
            // Arrange
            DateTime now = new DateTime(2022, 11, 21, 1, 00, 00); //FRIDAY MIDNIGHT

            var userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(0),
                    EndTime  = TimeSpan.FromHours(1),
                    Day = now.Date.AddDays(0).DayOfWeek // TOMORROW
                },
                 new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(2).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(5),
                    Day = now.Date.AddDays(2).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(5),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = now.Date.AddDays(2).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = now.Date.AddDays(2).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = now.Date.AddDays(2).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(6),
                    EndTime  = TimeSpan.FromHours(7),
                    Day = now.Date.AddDays(2).DayOfWeek // TOMORROW
                },

                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = now.Date.AddDays(1).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(5),
                    Day = now.Date.AddDays(1).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(5),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = now.Date.AddDays(1).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(11),
                    Day = now.Date.AddDays(1).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = now.Date.AddDays(1).DayOfWeek // TOMORROW
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(6),
                    EndTime  = TimeSpan.FromHours(7),
                    Day = now.Date.AddDays(1).DayOfWeek // TOMORROW
                }

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now,
                    EndDate = now.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now.AddYears(-1),
                    EndDate = now,
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = now.AddYears(-1),
                    EndDate = now.AddYears(2),
                    Parts = workPatternParts
                }
            };

            // Act
            TimeSpan response = await _timerService.FindLowestDelayAsync(workPattern, now, CancellationToken.None);

            //Assert
            response.Should().Be(TimeSpan.FromHours(24));

        }
    }
}
