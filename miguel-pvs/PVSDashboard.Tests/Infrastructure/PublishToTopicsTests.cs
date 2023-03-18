using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Project1.Infrastructure;
using Project1.Models;
using Xunit;

namespace PVSDashboard.Tests.Infrastructure
{
    public class CheckInApplicationUserTests
    {
        private readonly UnsService _unsService;
        private readonly Mock<IMqttService> _mqttServiceMock;

        public CheckInApplicationUserTests()
        {
            _mqttServiceMock = new Mock<IMqttService>();


            _unsService = new UnsService(_mqttServiceMock.Object);
        }
        [Fact(DisplayName = "TestShiftLogic - The application publish 2 parts and workToday has to be false - No absents (Random Variables-3WP,10Parts each,0Absents) ")]
        public async Task CallListWorkPatternsShouldReturnFalseAndTwoSchedulesForToday_UserWith3WorkPattern2PartsADay()
        {
            // Arrange
            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(2),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(5),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(7),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(1),
                    EndDate = today.AddYears(2),
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

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            // Act
            await _unsService.CallListWorkPatternAsync(applicationUser, CancellationToken.None);

            // Assert
            _unsService._scheduleToWorkNow.Should().BeFalse();
            _unsService._todayList.Should().HaveCount(2);
            var todayList1 = _unsService._todayList[0];
            var todayList2 = _unsService._todayList[1];

            todayList1.From.Should().Be(TimeSpan.FromHours(1));
            todayList1.Till.Should().Be(TimeSpan.FromHours(2));

            todayList2.From.Should().Be(TimeSpan.FromHours(3));
            todayList2.Till.Should().Be(TimeSpan.FromHours(4));

        }

        [Fact(DisplayName = "TestShiftLogic - The application publish 0 parts and workToday has to be false - No absents and not inside any WP (Random Variables-3WP,10Parts each,0Absents) ")]
        public async Task CallListWorkPatternsShouldReturnFalseAndZeroSchedulesForToday_UserWith3WorkPattern2PartsADay()
        {
            // Arrange
            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(2),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(5),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(7),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(1),
                    EndDate = today.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(1),
                    EndDate = today.AddYears(2),
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

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            // Act
            await _unsService.CallListWorkPatternAsync(applicationUser, CancellationToken.None);

            // Assert
            _unsService._scheduleToWorkNow.Should().BeFalse();
            _unsService._todayList.Should().HaveCount(0);

        }

        [Fact(DisplayName = "TestShiftLogic - The application publish 0 parts and workToday has to be false - No absents and no Parts in WP (Random Variables-3WP,0Parts each,0Absents) ")]
        public async Task CallListWorkPatternsShouldReturnFalseAndZeroSchedulesForToday_UserWith3WorkPattern0PartsADay()
        {
            // Arrange
            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(1),
                    EndDate = today.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(-1),
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                }
            };

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            // Act
            await _unsService.CallListWorkPatternAsync(applicationUser, CancellationToken.None);

            // Assert
            _unsService._scheduleToWorkNow.Should().BeFalse();
            _unsService._todayList.Should().HaveCount(0);

        }

        [Fact(DisplayName = "TestShiftLogic - The application publish 0 parts and workToday has to be false - No absents and no Parts Today (Random Variables-3WP,8Parts each,0Absents) ")]
        public async Task CallListWorkPatternsShouldReturnFalseAndFourSchedulesForToday_UserWith3WorkPattern8PartsADay()
        {
            // Arrange
            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(5),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(7),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(1),
                    EndDate = today.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(-1),
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                }
            };

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            // Act
            await _unsService.CallListWorkPatternAsync(applicationUser, CancellationToken.None);

            // Assert
            _unsService._scheduleToWorkNow.Should().BeFalse();
            _unsService._todayList.Should().HaveCount(0);

        }

        [Fact(DisplayName = "TestShiftLogic - The application publish 4 parts and workToday has to be false - No absents and 2 WP overlap (Random Variables-3WP,10Parts each,0Absents) ")]
        public async Task CallListWorkPatternsShouldReturnFalseAndFourSchedulesForToday_UserWith3WorkPattern2PartsADay()
        {
            // Arrange
            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(2),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(5),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(7),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(1),
                    EndDate = today.AddYears(2),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(-1),
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                }
            };

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            // Act
            await _unsService.CallListWorkPatternAsync(applicationUser, CancellationToken.None);

            // Assert
            _unsService._scheduleToWorkNow.Should().BeFalse();
            _unsService._todayList.Should().HaveCount(4);
            var todayList1 = _unsService._todayList[0];
            var todayList2 = _unsService._todayList[1];
            var todayList3 = _unsService._todayList[2];
            var todayList4 = _unsService._todayList[3];

            todayList1.From.Should().Be(TimeSpan.FromHours(1));
            todayList1.Till.Should().Be(TimeSpan.FromHours(2));

            todayList2.From.Should().Be(TimeSpan.FromHours(1));
            todayList2.Till.Should().Be(TimeSpan.FromHours(2));

            todayList3.From.Should().Be(TimeSpan.FromHours(3));
            todayList3.Till.Should().Be(TimeSpan.FromHours(4));

            todayList4.From.Should().Be(TimeSpan.FromHours(3));
            todayList4.Till.Should().Be(TimeSpan.FromHours(4));

        }

        [Fact(DisplayName = "TestShiftLogic - The application publish 2 work patterns and workToday has to be true - No absents (Random Variables-3WP,10Parts,0Absents)")]
        public async Task CallListWorkPatternsShouldReturnTrueAndTwoSchedulesForToday_UserWith2WorkPattern2PartsADay()
        {
            // Arrange
            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(23),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(3),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(5),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(7),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(1),
                    EndDate = today.AddYears(2),
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

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            // Act
            await _unsService.CallListWorkPatternAsync(applicationUser, CancellationToken.None);

            // Assert
            _unsService._scheduleToWorkNow.Should().BeTrue();
            _unsService._todayList.Should().HaveCount(2);
            var todayList1 = _unsService._todayList[0];
            var todayList2 = _unsService._todayList[1];

            todayList1.From.Should().Be(TimeSpan.FromHours(1));
            todayList1.Till.Should().Be(TimeSpan.FromHours(23));

            todayList2.From.Should().Be(TimeSpan.FromHours(3));
            todayList2.Till.Should().Be(TimeSpan.FromHours(4));

        }

        [Fact(DisplayName = "TestAbsentLogic - The application should publish 4 parts and workToday has to be true - It has two absents (Random Variables-3WP,10Parts each,2Absents)")]
        public async Task CallListWorkPatternsShouldReturnTrueAndFourSchedulesForToday_UserWith2WorkPattern2PartsADay2Absent()
        {
            // Arrange
            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(23),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(5),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(7),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(1),
                    EndDate = today.AddYears(2),
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

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            var listAbsentEachDay = new AbsentEachDay(today.DayOfYear, new List<int> { 1 });

            DateTime absentDateTime1 = new DateTime(today.Year, today.Month, today.Day, 3, 0, 0);
            DateTime absentDateTime2 = new DateTime(today.Year, today.Month, today.Day, 4, 0, 0);
            DateTime absentDateTime3 = new DateTime(today.Year, today.Month, today.Day, 5, 0, 0);
            DateTime absentDateTime4 = new DateTime(today.Year, today.Month, today.Day, 6, 0, 0);

            var listAllAbsents = new List<Absent>
            {
                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime1,
                    EndDate = absentDateTime2
            },
                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime3,
                    EndDate = absentDateTime4
                }
            };

            // Act
            _unsService._listAbsentEachDay = listAbsentEachDay;
            _unsService._listAllAbsents = listAllAbsents;
            await _unsService.CallListWorkPatternAsync(applicationUser, CancellationToken.None);

            // Assert
            _unsService._scheduleToWorkNow.Should().BeTrue();
            _unsService._todayList.Should().HaveCount(4);

            var todayList1 = _unsService._todayList[0];
            var todayList2 = _unsService._todayList[1];
            var todayList3 = _unsService._todayList[2];
            var todayList4 = _unsService._todayList[3];




            todayList1.From.Should().Be(TimeSpan.FromHours(1));
            todayList1.Till.Should().Be(TimeSpan.FromHours(3));

            todayList2.From.Should().Be(TimeSpan.FromHours(2));
            todayList2.Till.Should().Be(TimeSpan.FromHours(3));

            todayList3.From.Should().Be(TimeSpan.FromHours(4));
            todayList3.Till.Should().Be(TimeSpan.FromHours(5));

            todayList4.From.Should().Be(TimeSpan.FromHours(6));
            todayList4.Till.Should().Be(TimeSpan.FromHours(23));




        }

        [Fact(DisplayName = "TestAbsentLogic - The application should publish 2 parts and workToday has to be true - It has one absent off the part(Random Variables-3WP,10Parts each,1Absents)")]
        public async Task CallListWorkPatternsShouldReturnTrueAndTwoSchedulesForToday_UserWith2WorkPattern2PartsADay1Absent()
        {
            // Arrange
            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(2),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(6),
                    EndTime  = TimeSpan.FromHours(21),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(5),
                    EndTime  = TimeSpan.FromHours(6),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(7),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = DateTime.Today.Date.AddDays(1).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(1),
                    EndDate = today.AddYears(2),
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

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            var listAbsentEachDay = new AbsentEachDay(today.DayOfYear, new List<int> { 1 });

            DateTime absentDateTime1 = new DateTime(today.Year, today.Month, today.Day, 3, 0, 0);
            DateTime absentDateTime2 = new DateTime(today.Year, today.Month, today.Day, 5, 0, 0);

            var listAllAbsents = new List<Absent>
            {
                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime1,
                    EndDate = absentDateTime2
                },

            };

            // Act
            _unsService._listAbsentEachDay = listAbsentEachDay;
            _unsService._listAllAbsents = listAllAbsents;
            await _unsService.CallListWorkPatternAsync(applicationUser, CancellationToken.None);

            // Assert
            _unsService._scheduleToWorkNow.Should().BeTrue();
            _unsService._todayList.Should().HaveCount(2);

            var todayList1 = _unsService._todayList[0];
            var todayList2 = _unsService._todayList[1];




            todayList1.From.Should().Be(TimeSpan.FromHours(1));
            todayList1.Till.Should().Be(TimeSpan.FromHours(2));

            todayList2.From.Should().Be(TimeSpan.FromHours(6));
            todayList2.Till.Should().Be(TimeSpan.FromHours(21));
        }

        [Fact(DisplayName = "TestJoinPartsToday - The application should publish 2 parts workToday has to be true- It has two absents (Random Variables-3WP,10Parts each,2Absents)")]
        public async Task OverLapTodayScheduleAsyncShouldReturnTrueAndTwoSchedulesForToday_UserWith2WorkPattern2PartsADay()
        {
            // Arrange

            Dictionary<string, string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((string payload, string topic, CancellationToken ct)
                => responsePayloads.Add(topic, payload));


            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(6),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(23),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(13),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(1),
                    EndDate = today.AddYears(2),
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

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            var listAbsentEachDay = new AbsentEachDay(today.DayOfYear, new List<int> { 1 });

            DateTime absentDateTime1 = new DateTime(today.Year, today.Month, today.Day, 3, 0, 0);
            DateTime absentDateTime2 = new DateTime(today.Year, today.Month, today.Day, 7, 0, 0);

            var listAllAbsents = new List<Absent>
            {
                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime1,
                    EndDate = absentDateTime2
                }
            };

            // Act
            _unsService._listAbsentEachDay = listAbsentEachDay;
            _unsService._listAllAbsents = listAllAbsents;
            List<ApplicationUser> listApplicationUsers = new List<ApplicationUser>();
            listApplicationUsers.Add(applicationUser);
            await _unsService.CallEachApplicationUserAsync(listApplicationUsers, CancellationToken.None);

            // Assert

            var listAllTodaySchedules = new List<TodaySchedule>
            {
                new TodaySchedule
                {
                    From = TimeSpan.FromHours(1),
                    Till = TimeSpan.FromHours(3),
                },

                new TodaySchedule
                {
                    From = TimeSpan.FromHours(7),
                    Till = TimeSpan.FromHours(23),
                }
            };

            string todaySchedule = JsonConvert.SerializeObject(listAllTodaySchedules);
            string topic = $"users/{Convert.ToString(userId)}/pvs/Schedule/today";

            _mqttServiceMock.Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/Schedule/today", It.IsAny<CancellationToken>()));
            responsePayloads[topic].Should().Be(todaySchedule);
        }

        [Fact(DisplayName = "TestJoinPartsToday - The application should publish 1 parts workToday has to be true testing ending edge")]
        public async Task OverLapTodayScheduleAsyncShoulTestEgdes_Ending()
        {
            // Arrange

            Dictionary<string, string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((string payload, string topic, CancellationToken ct)
                => responsePayloads.Add(topic, payload));


            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = DateTime.Today.Date.DayOfWeek
                },
            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
            };

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };
            List<ApplicationUser> listApplicationUsers = new List<ApplicationUser>();
            listApplicationUsers.Add(applicationUser);
            await _unsService.CallEachApplicationUserAsync(listApplicationUsers, CancellationToken.None);

            // Assert

            var listAllTodaySchedules = new List<TodaySchedule>
            {
                new TodaySchedule
                {
                    From = TimeSpan.FromHours(1),
                    Till = TimeSpan.FromHours(8),
                }
            };

            string todaySchedule = JsonConvert.SerializeObject(listAllTodaySchedules);
            string topic = $"users/{Convert.ToString(userId)}/pvs/Schedule/today";

            _mqttServiceMock.Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/Schedule/today", It.IsAny<CancellationToken>()));
            responsePayloads[topic].Should().Be(todaySchedule);
        }

        [Fact(DisplayName = "TestJoinPartsToday - The application should publish 1 parts workToday has to be true testing start edge")]
        public async Task OverLapTodayScheduleAsyncShoulTestEgdes_Start()
        {
            // Arrange

            Dictionary<string, string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((string payload, string topic, CancellationToken ct)
                => responsePayloads.Add(topic, payload));


            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(0),
                    EndTime  = TimeSpan.FromHours(1),
                    Day = DateTime.Today.Date.DayOfWeek
                },
            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
            };

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };
            List<ApplicationUser> listApplicationUsers = new List<ApplicationUser>();
            listApplicationUsers.Add(applicationUser);
            await _unsService.CallEachApplicationUserAsync(listApplicationUsers, CancellationToken.None);

            // Assert

            var listAllTodaySchedules = new List<TodaySchedule>
            {
                new TodaySchedule
                {
                    From = TimeSpan.FromHours(0),
                    Till = TimeSpan.FromHours(4),
                }
            };

            string todaySchedule = JsonConvert.SerializeObject(listAllTodaySchedules);
            string topic = $"users/{Convert.ToString(userId)}/pvs/Schedule/today";

            _mqttServiceMock.Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/Schedule/today", It.IsAny<CancellationToken>()));
            responsePayloads[topic].Should().Be(todaySchedule);
        }

        [Fact(DisplayName = "TestJoinPartsToday - The application should publish 1 parts workToday has to be true testing ending edge")]
        public async Task OverLapTodayScheduleAsyncShoulTestEgdes_Same()
        {
            // Arrange

            Dictionary<string, string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((string payload, string topic, CancellationToken ct)
                => responsePayloads.Add(topic, payload));


            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
            };

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };
            List<ApplicationUser> listApplicationUsers = new List<ApplicationUser>();
            listApplicationUsers.Add(applicationUser);
            await _unsService.CallEachApplicationUserAsync(listApplicationUsers, CancellationToken.None);

            // Assert

            var listAllTodaySchedules = new List<TodaySchedule>
            {
                new TodaySchedule
                {
                    From = TimeSpan.FromHours(1),
                    Till = TimeSpan.FromHours(4),
                }
            };

            string todaySchedule = JsonConvert.SerializeObject(listAllTodaySchedules);
            string topic = $"users/{Convert.ToString(userId)}/pvs/Schedule/today";

            _mqttServiceMock.Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/Schedule/today", It.IsAny<CancellationToken>()));
            responsePayloads[topic].Should().Be(todaySchedule);
        }

        [Fact(DisplayName = "TestJoinPartsToday - The application should publish 1 parts workToday has to be true testing ending edge")]
        public async Task OverLapTodayScheduleAsyncShouldReturnTrueAndOneSchedulesForToday_UserWith2WorkPattern2PartsADay()
        {
            // Arrange

            Dictionary<string, string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((string payload, string topic, CancellationToken ct)
                => responsePayloads.Add(topic, payload));


            DateTime today = DateTime.Today;

            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(4),
                    EndTime  = TimeSpan.FromHours(8),
                    Day = DateTime.Today.Date.DayOfWeek
                },
            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
            };

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };
            List<ApplicationUser> listApplicationUsers = new List<ApplicationUser>();
            listApplicationUsers.Add(applicationUser);
            await _unsService.CallEachApplicationUserAsync(listApplicationUsers, CancellationToken.None);

            // Assert

            var listAllTodaySchedules = new List<TodaySchedule>
            {
                new TodaySchedule
                {
                    From = TimeSpan.FromHours(1),
                    Till = TimeSpan.FromHours(8),
                }
            };

            string todaySchedule = JsonConvert.SerializeObject(listAllTodaySchedules);
            string topic = $"users/{Convert.ToString(userId)}/pvs/Schedule/today";

            _mqttServiceMock.Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/Schedule/today", It.IsAny<CancellationToken>()));
            responsePayloads[topic].Should().Be(todaySchedule);
        }

        [Fact(DisplayName = "Publish Location Test - Should Publish one UserEachLocation")]
        public async Task CallPublishLocationAsyncAndPublishLocation()
        {
            // Arrange
            DateTime today = DateTime.Today;

            Dictionary<string, string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((string payload, string topic, CancellationToken ct)
                => responsePayloads.Add(topic, payload));

            UsersEachLocation usersEachLocation = new UsersEachLocation(1, new List<int> { 1, 2 });
            UsersEachLocation nullUsersEachLocation = null;

            // Act
            await _unsService.PublishLocationAsync(usersEachLocation,1, CancellationToken.None);
            await _unsService.PublishLocationAsync(nullUsersEachLocation, 2, CancellationToken.None);

            List<int> payLoad = usersEachLocation.UserIds;
            string combinedPayLoad = JsonConvert.SerializeObject(payLoad);

            // Assert

            string topic1 = $"locations/{Convert.ToString(1)}/pvs/mechanics";
            string topic2 = $"locations/{Convert.ToString(2)}/pvs/mechanics";

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"locations/1/pvs/mechanics", It.IsAny<CancellationToken>()));
            responsePayloads[topic1].Should().Be(combinedPayLoad);

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"locations/2/pvs/mechanics", It.IsAny<CancellationToken>()));
            responsePayloads[topic2].Should().Be("");
        }

        [Fact(DisplayName = "DeleteApplicationUser should publish 4 empty payloads")]
        public async Task DeleteApplicationUser_ShouldPublish4EmptyPayLoads()
        {
            // Arrange
            DateTime today = DateTime.Today;

            Dictionary<string, string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((string payload, string topic, CancellationToken ct)
                => responsePayloads.Add(topic, payload));


            int userId = 1;
            var now = new DateTime(2022, 1, 1, 0, 0, 0);

            var workPatternParts = new List<WorkPatternPart>
            {

                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(19),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.DayOfWeek
                }

            };

            var workPattern1 = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(-1),
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                }
            };

            var listApplicationUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = 1,
                    OfficeLocation = 2,
                    FirstName = "Miguel",
                    WorkPatterns = workPattern1,
                    CheckedIn = false
                },
            };

            // Act
            await _unsService.DeleteTopicApplicationUserAsync(listApplicationUsers[0], CancellationToken.None);

            // Assert

            string payLoad = null;

            string topicCheckIn = $"users/{Convert.ToString(userId)}/pvs/checked_in";
            string topicOfficeLocation = ($"locations/{listApplicationUsers[0].OfficeLocation}/pvs/mechanics");
            string topicSchedule = $"users/{userId}/pvs/Schedule";
            string topicScheduleToday = $"users/{userId}/pvs/Schedule/today";

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/checked_in", It.IsAny<CancellationToken>()));
            responsePayloads[topicCheckIn].Should().Be(payLoad);

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/checked_in", It.IsAny<CancellationToken>()));
            responsePayloads[topicOfficeLocation].Should().Be(payLoad);

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/checked_in", It.IsAny<CancellationToken>()));
            responsePayloads[topicSchedule].Should().Be(payLoad);

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/Schedule/today", It.IsAny<CancellationToken>()));
            responsePayloads[topicScheduleToday].Should().Be(payLoad);

        }

        [Fact(DisplayName = "Publish Check-In Test - ShouldPublish Two CheckIns,one false and one true")]
        public async Task TestCallEachUserAndPublishEveryCheckIn()
        {
            // Arrange
            DateTime today = DateTime.Today;

            Dictionary<string,string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<CancellationToken>()))
                .Callback((string payload,string topic,CancellationToken ct)
                => responsePayloads.Add(topic,payload));


            int userId1 = 1;
            int userId2 = 2;
            var now = new DateTime(2022,1,1,0,0,0);

            var workPatternParts = new List<WorkPatternPart>
            {

                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(19),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.DayOfWeek
                }

            };

            var workPattern1 = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId1,
                    StartDate = today.AddYears(-1),
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                }
            };
            var workPattern2 = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId2,
                    StartDate = today.AddYears(-1),
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                }
            };

            var listApplicationUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = 1,
                    OfficeLocation = 2,
                    FirstName = "Miguel",
                    WorkPatterns = workPattern1,
                    CheckedIn = false
                },
                new ApplicationUser
                {
                    Id = 2,
                    OfficeLocation = 2,
                    FirstName = "Miguel",
                    WorkPatterns = workPattern2,
                    CheckedIn = true
                },
            };

            // Act
            await _unsService.CallEachCheckInAsync(listApplicationUsers,now, CancellationToken.None);

            // Assert

            var falseCheckIn = new CheckInPayLoad()
            {
                Value = false,
                Timestamp = now
            };

            var trueCheckIn = new CheckInPayLoad()
            {
                Value = true,
                Timestamp = now
            };

            string stringFalseCheckIn = JsonConvert.SerializeObject(falseCheckIn);
            string stringTrueCheckIn = JsonConvert.SerializeObject(trueCheckIn);
            string topic1 = $"users/{Convert.ToString(userId1)}/pvs/checked_in";
            string topic2 = $"users/{Convert.ToString(userId2)}/pvs/checked_in";

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/checked_in", It.IsAny<CancellationToken>()));
            responsePayloads[topic1].Should().Be(stringFalseCheckIn);

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/2/pvs/checked_in", It.IsAny<CancellationToken>()));
            responsePayloads[topic2].Should().Be(stringTrueCheckIn);
        }

        [Fact(DisplayName = "Publish Check-In Test")]
        public async Task TestOnTheDeleteApplicationUserAsyncMethod()
        {
            // Arrange
            DateTime today = DateTime.Today;

            Dictionary<string, string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((string payload, string topic, CancellationToken ct)
                => responsePayloads.Add(topic, payload));


            int userId = 1;
            var now = new DateTime(2022, 1, 1, 0, 0, 0);

            var workPatternParts = new List<WorkPatternPart>
            {

                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(19),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.DayOfWeek
                }

            };

            var workPattern1 = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(-1),
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                }
            };

            var listApplicationUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = 1,
                    OfficeLocation = 2,
                    FirstName = "Miguel",
                    WorkPatterns = workPattern1,
                    CheckedIn = false
                },
            };

            // Act
            await _unsService.DeleteTopicApplicationUserAsync(listApplicationUsers[0], CancellationToken.None);

            // Assert

            string payLoad = null;

            string topicCheckIn = $"users/{Convert.ToString(userId)}/pvs/checked_in";
            string topicOfficeLocation = ($"locations/{listApplicationUsers[0].OfficeLocation}/pvs/mechanics");
            string topicSchedule = $"users/{userId}/pvs/Schedule";
            string topicScheduleToday = $"users/{userId}/pvs/Schedule/today";

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/checked_in", It.IsAny<CancellationToken>()));
            responsePayloads[topicCheckIn].Should().Be(payLoad);

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/checked_in", It.IsAny<CancellationToken>()));
            responsePayloads[topicOfficeLocation].Should().Be(payLoad);

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/checked_in", It.IsAny<CancellationToken>()));
            responsePayloads[topicSchedule].Should().Be(payLoad);

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/Schedule/today", It.IsAny<CancellationToken>()));
            responsePayloads[topicScheduleToday].Should().Be(payLoad);

        }

        [Fact(DisplayName = "Test All Edges on AbsentLOGIC")]
        public async Task TestEdgesOnAbsentLogic()
        {
            // Arrange
            DateTime today = DateTime.Today;

            Dictionary<string, string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((string payload, string topic, CancellationToken ct)
                => responsePayloads.Add(topic, payload));


            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {

                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(10),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = DateTime.Today.Date.DayOfWeek
                }

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = DateTime.Now,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
            };

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            var listAbsentEachDay = new AbsentEachDay(today.DayOfYear, new List<int> {1});

            DateTime absentDateTime1 = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            DateTime absentDateTime2 = new DateTime(today.Year, today.Month, today.Day, 10, 0, 0);

            DateTime absentDateTime3 = new DateTime(today.Year, today.Month, today.Day, 20, 0, 0);
            DateTime absentDateTime4 = new DateTime(today.Year, today.Month, today.Day, 23, 0, 0);

            DateTime absentDateTime5 = new DateTime(today.Year, today.Month, today.Day, 10, 0, 0);
            DateTime absentDateTime6 = new DateTime(today.Year, today.Month, today.Day, 11, 0, 0);

            DateTime absentDateTime7 = new DateTime(today.Year, today.Month, today.Day, 19, 0, 0);
            DateTime absentDateTime8 = new DateTime(today.Year, today.Month, today.Day, 20, 00, 00);

            var listAllAbsents = new List<Absent>
            {
                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime1,
                    EndDate = absentDateTime2
                },

                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime7,
                    EndDate = absentDateTime8
                },

                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime3,
                    EndDate = absentDateTime4
                },
                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime7,
                    EndDate = absentDateTime8
                },

                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime5,
                    EndDate = absentDateTime6
                },

            };

            // Act
            _unsService._listAbsentEachDay = listAbsentEachDay;
            _unsService._listAllAbsents = listAllAbsents;
            await _unsService.CallListWorkPatternAsync(applicationUser, CancellationToken.None);

            // Assert

            var listAllTodaySchedules = new List<TodaySchedule>
            {
                new TodaySchedule
                {
                    From = TimeSpan.FromHours(11),
                    Till = TimeSpan.FromHours(19),
                }

            };

            string todaySchedule = JsonConvert.SerializeObject(listAllTodaySchedules);
            string topic = $"users/{Convert.ToString(userId)}/pvs/Schedule/today";

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/Schedule/today", It.IsAny<CancellationToken>()));
            responsePayloads[topic].Should().Be(todaySchedule);
        }

        [Fact(DisplayName = "UltimateHardcoreTEST: The ApplicationUser has 3WP(2 overLap),6 Parts for today (All not sorted, 2 equals, 3 overlaps some kind of value). 6 Absent(All not sorted, 2 equals, 3 overlaps some kind of value)")]
        public async Task UltimateHardcoreTEST()
        {
            // Arrange
            DateTime today = DateTime.Today;

            Dictionary<string, string> responsePayloads = new();

            _mqttServiceMock.Setup
                (x => x.
                PublishOnTopicAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((string payload, string topic, CancellationToken ct)
                => responsePayloads.Add(topic, payload));


            int userId = 1;

            var workPatternParts = new List<WorkPatternPart>
            {

                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(19),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(8),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.DayOfWeek
                },

                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(19),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(20),
                    EndTime  = TimeSpan.FromHours(23),
                    Day = DateTime.Today.Date.DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(9),
                    EndTime  = TimeSpan.FromHours(10),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(11),
                    EndTime  = TimeSpan.FromHours(12),
                    Day = DateTime.Today.Date.AddDays(2).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(13),
                    EndTime  = TimeSpan.FromHours(14),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(15),
                    EndTime  = TimeSpan.FromHours(16),
                    Day = DateTime.Today.Date.AddDays(3).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(17),
                    EndTime  = TimeSpan.FromHours(18),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(19),
                    EndTime  = TimeSpan.FromHours(20),
                    Day = DateTime.Today.Date.AddDays(4).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(21),
                    EndTime  = TimeSpan.FromHours(22),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(23),
                    EndTime  = TimeSpan.FromHours(24),
                    Day = DateTime.Today.Date.AddDays(5).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(1),
                    EndTime  = TimeSpan.FromHours(3),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },
                new WorkPatternPart
                {
                    StartTime = TimeSpan.FromHours(2),
                    EndTime  = TimeSpan.FromHours(4),
                    Day = DateTime.Today.Date.AddDays(6).DayOfWeek
                },

            };

            var workPattern = new List<WorkPattern>
            {
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = DateTime.Now,
                    EndDate = today.AddYears(1),
                    Parts = workPatternParts
                },
                new WorkPattern
                {
                    UserId = userId,
                    StartDate = today.AddYears(-1),
                    EndDate = DateTime.Now,
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

            var applicationUser = new ApplicationUser
            {
                Id = 1,
                OfficeLocation = 1,
                FirstName = "Miguel",
                WorkPatterns = workPattern
            };

            var listAbsentEachDay = new AbsentEachDay(today.DayOfYear, new List<int> { 1 });

            DateTime absentDateTime1 = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            DateTime absentDateTime2 = new DateTime(today.Year, today.Month, today.Day, 1, 0, 0);

            DateTime absentDateTime3 = new DateTime(today.Year, today.Month, today.Day, 2, 0, 0);
            DateTime absentDateTime4 = new DateTime(today.Year, today.Month, today.Day, 6, 0, 0);

            DateTime absentDateTime5 = new DateTime(today.Year, today.Month, today.Day, 4, 0, 0);
            DateTime absentDateTime6 = new DateTime(today.Year, today.Month, today.Day, 10, 0, 0);

            DateTime absentDateTime7 = new DateTime(today.Year, today.Month, today.Day, 21, 0, 0);
            DateTime absentDateTime8 = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59);

            var listAllAbsents = new List<Absent>
            {
                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime1,
                    EndDate = absentDateTime2
                },

                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime7,
                    EndDate = absentDateTime8
                },

                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime3,
                    EndDate = absentDateTime4
                },
                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime7,
                    EndDate = absentDateTime8
                },

                new Absent
                {
                    Id = new Guid(),
                    UserId = userId,
                    StartDate = absentDateTime5,
                    EndDate = absentDateTime6
                },

            };

            // Act
            _unsService._listAbsentEachDay = listAbsentEachDay;
            _unsService._listAllAbsents = listAllAbsents;
            await _unsService.CallListWorkPatternAsync(applicationUser, CancellationToken.None);

            // Assert

            var listAllTodaySchedules = new List<TodaySchedule>
            {
                new TodaySchedule
                {
                    From = TimeSpan.FromHours(1),
                    Till = TimeSpan.FromHours(2),
                },

                new TodaySchedule
                {
                    From = TimeSpan.FromHours(10),
                    Till = TimeSpan.FromHours(19),
                },

                new TodaySchedule
                {
                    From = TimeSpan.FromHours(20),
                    Till = TimeSpan.FromHours(21),
                }

            };

            string todaySchedule = JsonConvert.SerializeObject(listAllTodaySchedules);
            string topic = $"users/{Convert.ToString(userId)}/pvs/Schedule/today";

            _mqttServiceMock
                .Verify(x => x.PublishOnTopicAsync(It.IsAny<string>(), $"users/1/pvs/Schedule/today", It.IsAny<CancellationToken>()));
            responsePayloads[topic].Should().Be(todaySchedule);
        }

    }
}
