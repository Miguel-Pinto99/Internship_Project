using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Project1.Infrastructure;
using Project1.Models;
using Xunit;

namespace PVSDashboard.Tests.Infrastructure
{
    public class BreakWorkPatternTests
    {
        private readonly UnsService _unsService;

        public BreakWorkPatternTests()
        {
            _unsService = new UnsService(null);
        }

        [Fact(DisplayName = "BreakWorkPatternAsync should return empty list when inserting empty list")]
        public async Task BreakWorkPatternAsyncShouldReturnEmptyList_WhenInsertingEmptyList()
        {
            // Arrange
            var userId = 1;
            var workPatternParts = new List<WorkPatternPart>();
            var absents = new List<Absent>();

            // Act
            List<WorkPatternPart> response = await _unsService
                .CheckIfPartSplitAsync(workPatternParts, absents, userId, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeEmpty();
        }

        [Fact(DisplayName = "BreakWorkPatternAsync should return work pattern when user has no absent")]
        public async Task BreakWorkPatternAsyncShouldReturnWorkPattern_WhenUserHasNoAbsent()
        {
            // Arrange
            var userId = 1;
            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                }
            };
            var absents = new List<Absent>();

            // Act
            List<WorkPatternPart> response = await _unsService
                .CheckIfPartSplitAsync(workPatternParts, absents, userId, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);

            WorkPatternPart reponseWorkPatternPart = response[0];
            reponseWorkPatternPart.StartTime.Should().Be(TimeSpan.FromHours(8));
            reponseWorkPatternPart.EndTime.Should().Be(TimeSpan.FromHours(16));
        }

        [Fact(DisplayName = "BreakWorkPatternAsync should split the work pattern when user has a absent")]
        public async Task BreakWorkPatternAsyncShouldSplitThePartIntoTwo_WhenUserHasOneAbsent()
        {
            // Arrange
            DateTime today = DateTime.Today;

            var userId = 1;
            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                }
            };
            var absents = new List<Absent>
            {
                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 11, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 12, 0, 0, DateTimeKind.Local),
                }
            };

            // Act
            List<WorkPatternPart> response = await _unsService
                .CheckIfPartSplitAsync(workPatternParts, absents, userId, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(2);

            WorkPatternPart reponseWorkPatternPart1 = response[0];
            reponseWorkPatternPart1.StartTime.Should().Be(TimeSpan.FromHours(8));
            reponseWorkPatternPart1.EndTime.Should().Be(TimeSpan.FromHours(11));

            WorkPatternPart reponseWorkPatternPart2 = response[1];
            reponseWorkPatternPart2.StartTime.Should().Be(TimeSpan.FromHours(12));
            reponseWorkPatternPart2.EndTime.Should().Be(TimeSpan.FromHours(16));
        }

        [Fact(DisplayName = "BreakWorkPatternAsync should return a empty workPattern when user has day absent")]
        public async Task BreakWorkPatternAsyncShouldReturnEmptyPart_WhenUserHasDayAbsent()
        {
            // Arrange
            DateTime today = DateTime.Today;

            var userId = 1;
            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                }
            };
            var absents = new List<Absent>
            {
                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59, DateTimeKind.Local),
                }
            };

            // Act
            List<WorkPatternPart> response = await _unsService
                .CheckIfPartSplitAsync(workPatternParts, absents, userId, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeEmpty();
        }

        [Fact(DisplayName = "BreakWorkPatternAsync should split ending of the work pattern when user has a absent")]
        public async Task BreakWorkPatternAsyncShouldAlterEndOfPart_WhenUserHasAnAbsent()
        {
            // Arrange
            DateTime today = DateTime.Today;

            var userId = 1;
            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                }
            };
            var absents = new List<Absent>
            {
                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 15, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 17, 0, 0, DateTimeKind.Local),
                }
            };

            // Act
            List<WorkPatternPart> response = await _unsService
                .CheckIfPartSplitAsync(workPatternParts, absents, userId, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);

            WorkPatternPart reponseWorkPatternPart = response[0];
            reponseWorkPatternPart.Should().BeEquivalentTo(
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime = TimeSpan.FromHours(15),
                    Day = DateTime.Today.Date.DayOfWeek
                }
                );
        }

        [Fact(DisplayName = "BreakWorkPatternAsync should split beginning of the work pattern when user has a absent")]
        public async Task BreakWorkPatternAsyncShouldAlterBeginOfPart_WhenUserHasAnAbsent()
        {
            // Arrange
            DateTime today = DateTime.Today;

            var userId = 1;
            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                }
            };
            var absents = new List<Absent>
            {
                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 7, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 9, 0, 0, DateTimeKind.Local),
                }
            };

            // Act
            List<WorkPatternPart> response = await _unsService
                .CheckIfPartSplitAsync(workPatternParts, absents, userId, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);

            WorkPatternPart reponseWorkPatternPart = response[0];
            reponseWorkPatternPart.Should().BeEquivalentTo(
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                }
                );
        }

        [Fact(DisplayName = "BreakWorkPatternAsync should split the work pattern in two and split beginning of the work pattern")]
        public async Task BreakWorkPatternAsyncShoudSplitThePartIntoTwoAndAlterBegin_WhenUserHasTwoAbsentsInTheSameDay()
        {
            // Arrange
            DateTime today = DateTime.Today;

            var userId = 1;
            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                }
            };
            var absents = new List<Absent>
            {
                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 7, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 9, 0 , 0, DateTimeKind.Local),
                },

                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 11, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 12, 0, 0, DateTimeKind.Local),
                }
            };

            // Act
            List<WorkPatternPart> response = await _unsService
                .CheckIfPartSplitAsync(workPatternParts, absents, userId, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(2);

            WorkPatternPart reponseWorkPatternPart1 = response[0];
            reponseWorkPatternPart1.StartTime.Should().Be(TimeSpan.FromHours(9));
            reponseWorkPatternPart1.EndTime.Should().Be(TimeSpan.FromHours(11));

            WorkPatternPart reponseWorkPatternPart2 = response[1];
            reponseWorkPatternPart2.StartTime.Should().Be(TimeSpan.FromHours(12));
            reponseWorkPatternPart2.EndTime.Should().Be(TimeSpan.FromHours(16));



        }

        [Fact(DisplayName = "BreakWorkPatternAsync should split the work pattern two and split ending of the work pattern")]
        public async Task BreakWorkPatternAsyncShoudSplitThePartIntoTwoAndAlterEnd_WhenUserHasTwoAbsentsInTheSameDay()
        {
            // Arrange
            DateTime today = DateTime.Today;

            var userId = 1;
            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.AddDays(4).Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.AddDays(3).Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.AddDays(1).Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                }
            };
            var absents = new List<Absent>
            {
                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 9, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 10, 0 , 0, DateTimeKind.Local),
                },

                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 15, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 17, 0, 0, DateTimeKind.Local),
                }
            };

            // Act
            List<WorkPatternPart> response = await _unsService
                .CheckIfPartSplitAsync(workPatternParts, absents, userId, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(2);

            WorkPatternPart reponseWorkPatternPart1 = response[0];
            reponseWorkPatternPart1.StartTime.Should().Be(TimeSpan.FromHours(8));
            reponseWorkPatternPart1.EndTime.Should().Be(TimeSpan.FromHours(9));

            WorkPatternPart reponseWorkPatternPart2 = response[1];
            reponseWorkPatternPart2.StartTime.Should().Be(TimeSpan.FromHours(10));
            reponseWorkPatternPart2.EndTime.Should().Be(TimeSpan.FromHours(15));


        }

        [Fact(DisplayName = "BreakWorkPatternAsync should split the work pattern in three when user has two absents")]
        public async Task BreakWorkPatternAsyncShoudSplitThePartIntoThree_WhenUserHasTwoAbsentsInTheSameDay()
        {
            // Arrange
            DateTime today = DateTime.Today;

            var userId = 1;
            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.AddDays(4).Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.AddDays(3).Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.AddDays(1).Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                }
            };
            var absents = new List<Absent>
            {
                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 9, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 10, 0 , 0, DateTimeKind.Local),
                },

                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 11, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 12, 0, 0, DateTimeKind.Local),
                }
            };

            // Act
            List<WorkPatternPart> response = await _unsService
                .CheckIfPartSplitAsync(workPatternParts, absents, userId, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(3);

            WorkPatternPart reponseWorkPatternPart1 = response[0];
            reponseWorkPatternPart1.StartTime.Should().Be(TimeSpan.FromHours(8));
            reponseWorkPatternPart1.EndTime.Should().Be(TimeSpan.FromHours(9));

            WorkPatternPart reponseWorkPatternPart2 = response[1];
            reponseWorkPatternPart2.StartTime.Should().Be(TimeSpan.FromHours(10));
            reponseWorkPatternPart2.EndTime.Should().Be(TimeSpan.FromHours(11));

            WorkPatternPart reponseWorkPatternPart3 = response[2];
            reponseWorkPatternPart3.StartTime.Should().Be(TimeSpan.FromHours(12));
            reponseWorkPatternPart3.EndTime.Should().Be(TimeSpan.FromHours(16));

        }

        [Fact(DisplayName = "BreakWorkPatternAsync should split the work pattern in two when user has two overlapping absents")]
        public async Task BreakWorkPatternAsyncShoudSplitThePartIntoTwo_WhenUserHasTwoOverlappingAbsentsInTheSameDay()
        {
            // Arrange
            DateTime today = DateTime.Today;

            var userId = 1;
            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.AddDays(4).Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.AddDays(3).Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.AddDays(1).Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                }
            };
            var absents = new List<Absent>
            {
                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 9, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 11, 0 , 0, DateTimeKind.Local),
                },

                new Absent
                {
                    UserId = userId,
                    StartDate = new DateTime(today.Year, today.Month, today.Day, 10, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(today.Year, today.Month, today.Day, 12, 0, 0, DateTimeKind.Local),
                }
            };

            // Act
            List<WorkPatternPart> response = await _unsService
                .CheckIfPartSplitAsync(workPatternParts, absents, userId, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(2);

            WorkPatternPart reponseWorkPatternPart1 = response[0];
            reponseWorkPatternPart1.StartTime.Should().Be(TimeSpan.FromHours(8));
            reponseWorkPatternPart1.EndTime.Should().Be(TimeSpan.FromHours(9));

            WorkPatternPart reponseWorkPatternPart2 = response[1];
            reponseWorkPatternPart2.StartTime.Should().Be(TimeSpan.FromHours(12));
            reponseWorkPatternPart2.EndTime.Should().Be(TimeSpan.FromHours(16));

        }


    }
}

