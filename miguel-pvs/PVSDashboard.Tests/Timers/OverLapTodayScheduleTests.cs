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
    public class OverLapTodayTests
    {
        private readonly TimerService _timerService;

        public OverLapTodayTests()
        {
            _timerService = new TimerService(null, null, null);
        }


        [Fact(DisplayName = "OverLapTodayTestsAsync should return one part")]
        public async Task OverLapTodayTestsAsyncShouldReturnOnePart_TestingEdge1()
        {
            // Arrange

            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(11),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var previousPart = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(11),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var newListTodayToAdd = new List<WorkPatternPart>();
            newListTodayToAdd.Add(previousPart);

            // Act
            (List<WorkPatternPart>,WorkPatternPart) response = await _timerService
                .OverLapTodayScheduleAsync(part, previousPart, newListTodayToAdd, CancellationToken.None);

            // Assert
            response.Item1.Should().NotBeNull();
            response.Item1[0].StartTime.Should().Be(TimeSpan.FromHours(9));
            response.Item1[0].EndTime.Should().Be(TimeSpan.FromHours(11));


            response.Item2.Should().NotBeNull();
            response.Item2.Should().BeEquivalentTo(previousPart);
        }     
    

    [Fact(DisplayName = "OverLapTodayTestsAsync should return one part")]
    public async Task OverLapTodayTestsAsyncShouldReturnOnePart_TestingEdge2()
    {
        // Arrange

        var part = new WorkPatternPart
        {
            StartTime = TimeSpan.FromHours(9),
            EndTime = TimeSpan.FromHours(10),
            Day = DateTime.Today.Date.DayOfWeek
        };

        var previousPart = new WorkPatternPart
        {
            StartTime = TimeSpan.FromHours(9),
            EndTime = TimeSpan.FromHours(11),
            Day = DateTime.Today.Date.DayOfWeek
        };

        var newListTodayToAdd = new List<WorkPatternPart>();
        newListTodayToAdd.Add(previousPart);

        // Act
        (List<WorkPatternPart>, WorkPatternPart) response = await _timerService
            .OverLapTodayScheduleAsync(part, previousPart, newListTodayToAdd, CancellationToken.None);

        // Assert
        response.Item1.Should().NotBeNull();
        response.Item1[0].StartTime.Should().Be(TimeSpan.FromHours(9));
        response.Item1[0].EndTime.Should().Be(TimeSpan.FromHours(11));


        response.Item2.Should().NotBeNull();
        response.Item2.Should().BeEquivalentTo(previousPart);


    }

    [Fact(DisplayName = "OverLapTodayTestsAsync should return one part")]
    public async Task OverLapTodayTestsAsyncShouldReturnOnePart_TestingEdge3()
    {
        // Arrange

        var part = new WorkPatternPart
        {
            StartTime = TimeSpan.FromHours(9),
            EndTime = TimeSpan.FromHours(12),
            Day = DateTime.Today.Date.DayOfWeek
        };

        var previousPart = new WorkPatternPart
        {
            StartTime = TimeSpan.FromHours(9),
            EndTime = TimeSpan.FromHours(11),
            Day = DateTime.Today.Date.DayOfWeek
        };

        var newListTodayToAdd = new List<WorkPatternPart>();
        newListTodayToAdd.Add(previousPart);

        // Act
        (List<WorkPatternPart>, WorkPatternPart) response = await _timerService
            .OverLapTodayScheduleAsync(part, previousPart, newListTodayToAdd, CancellationToken.None);

        // Assert
        response.Item1.Should().NotBeNull();
        response.Item1[0].StartTime.Should().Be(TimeSpan.FromHours(9));
        response.Item1[0].EndTime.Should().Be(TimeSpan.FromHours(12));


        response.Item2.Should().NotBeNull();
        response.Item2.Should().BeEquivalentTo(previousPart);


    }

        [Fact(DisplayName = "OverLapTodayTestsAsync should return one part")]
        public async Task OverLapTodayTestsAsyncShouldReturnOnePart_TestingEdge4()
        {
            // Arrange

            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(11),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var previousPart = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(11),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var newListTodayToAdd = new List<WorkPatternPart>();
            newListTodayToAdd.Add(previousPart);

            // Act
            (List<WorkPatternPart>, WorkPatternPart) response = await _timerService
                .OverLapTodayScheduleAsync(part, previousPart, newListTodayToAdd, CancellationToken.None);

            // Assert
            response.Item1.Should().NotBeNull();
            response.Item1[0].StartTime.Should().Be(TimeSpan.FromHours(9));
            response.Item1[0].EndTime.Should().Be(TimeSpan.FromHours(11));


            response.Item2.Should().NotBeNull();
            response.Item2.Should().BeEquivalentTo(previousPart);


        }

        [Fact(DisplayName = "OverLapTodayTestsAsync should return one part")]
        public async Task OverLapTodayTestsAsyncShouldReturnOnePart_TestingEdge5()
        {
            // Arrange

            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(12),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var previousPart = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(11),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var newListTodayToAdd = new List<WorkPatternPart>();
            newListTodayToAdd.Add(previousPart);

            // Act
            (List<WorkPatternPart>, WorkPatternPart) response = await _timerService
                .OverLapTodayScheduleAsync(part, previousPart, newListTodayToAdd, CancellationToken.None);

            // Assert
            response.Item1.Should().NotBeNull();
            response.Item1[0].StartTime.Should().Be(TimeSpan.FromHours(9));
            response.Item1[0].EndTime.Should().Be(TimeSpan.FromHours(12));


            response.Item2.Should().NotBeNull();
            response.Item2.Should().BeEquivalentTo(previousPart);


        }
        [Fact(DisplayName = "OverLapTodayTestsAsync should return one part")]
        public async Task OverLapTodayTestsAsyncShouldReturnOnePart_TestingEdge6()
        {
            // Arrange

            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(11),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var previousPart = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(11),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var newListTodayToAdd = new List<WorkPatternPart>();
            newListTodayToAdd.Add(previousPart);

            // Act
            (List<WorkPatternPart>, WorkPatternPart) response = await _timerService
                .OverLapTodayScheduleAsync(part, previousPart, newListTodayToAdd, CancellationToken.None);

            // Assert
            response.Item1.Should().NotBeNull();
            response.Item1[0].StartTime.Should().Be(TimeSpan.FromHours(9));
            response.Item1[0].EndTime.Should().Be(TimeSpan.FromHours(11));


            response.Item2.Should().NotBeNull();
            response.Item2.Should().BeEquivalentTo(previousPart);


        }

        [Fact(DisplayName = "OverLapTodayTestsAsync should return one part")]
        public async Task OverLapTodayTestsAsyncShouldReturnOnePart_TestingEdge7()
        {
            // Arrange

            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(11),
                EndTime = TimeSpan.FromHours(12),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var previousPart = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(11),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var newListTodayToAdd = new List<WorkPatternPart>();
            newListTodayToAdd.Add(previousPart);

            // Act
            (List<WorkPatternPart>, WorkPatternPart) response = await _timerService
                .OverLapTodayScheduleAsync(part, previousPart, newListTodayToAdd, CancellationToken.None);

            // Assert
            response.Item1.Should().NotBeNull();
            response.Item1[0].StartTime.Should().Be(TimeSpan.FromHours(9));
            response.Item1[0].EndTime.Should().Be(TimeSpan.FromHours(12));


            response.Item2.Should().NotBeNull();
            response.Item2.Should().BeEquivalentTo(previousPart);


        }


        [Fact(DisplayName = "OverLapTodayTestsAsync should return one part")]
        public async Task OverLapTodayTestsAsyncShouldReturnOnePart_TestingEdge8()
        {
            // Arrange

            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(11),
                EndTime = TimeSpan.FromHours(13),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var previousPart = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(12),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var newListTodayToAdd = new List<WorkPatternPart>();
            newListTodayToAdd.Add(previousPart);

            // Act
            (List<WorkPatternPart>, WorkPatternPart) response = await _timerService
                .OverLapTodayScheduleAsync(part, previousPart, newListTodayToAdd, CancellationToken.None);

            // Assert
            response.Item1.Should().NotBeNull();
            response.Item1[0].StartTime.Should().Be(TimeSpan.FromHours(9));
            response.Item1[0].EndTime.Should().Be(TimeSpan.FromHours(13));


            response.Item2.Should().NotBeNull();
            response.Item2.Should().BeEquivalentTo(previousPart);


        }

        [Fact(DisplayName = "OverLapTodayTestsAsync should return one part")]
        public async Task OverLapTodayTestsAsyncShouldReturnOnePart_TestingEdge9()
        {
            // Arrange

            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(12),
                EndTime = TimeSpan.FromHours(13),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var previousPart = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(11),
                Day = DateTime.Today.Date.DayOfWeek
            };

            var newListTodayToAdd = new List<WorkPatternPart>();
            newListTodayToAdd.Add(previousPart);

            // Act
            (List<WorkPatternPart>, WorkPatternPart) response = await _timerService
                .OverLapTodayScheduleAsync(part, previousPart, newListTodayToAdd, CancellationToken.None);

            // Assert
            response.Item1.Should().NotBeNull();
            response.Item1[0].StartTime.Should().Be(TimeSpan.FromHours(9));
            response.Item1[0].EndTime.Should().Be(TimeSpan.FromHours(11));

            response.Item1[1].StartTime.Should().Be(TimeSpan.FromHours(12));
            response.Item1[1].EndTime.Should().Be(TimeSpan.FromHours(13));


            response.Item2.Should().NotBeNull();
            response.Item2.Should().BeEquivalentTo(part);


        }
    }
}

