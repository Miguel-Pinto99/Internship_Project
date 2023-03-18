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
    public class CheckIfAbsentSplitTests
    {
        private readonly UnsService _unsService;

        public CheckIfAbsentSplitTests()
        {
            _unsService = new UnsService(null);
        }

        [Fact(DisplayName = "CheckIfAbsentNeedsSplitAsync should return one part")]
        public async Task CheckIfAbsentNeedsSplit_BeginingIsTodayEndsTodayAsync()
        {
            // Arrange
            DateTime now = new DateTime(2023, 4, 4, 12, 0, 0);
            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,7,0,0),
                    EndDate = new DateTime(now.Year, now.Month, now.Day, 10,0,0),
                }
            };

            // Act
            var response = await _unsService.CheckIfAbsentNeedsSplitAsync(listAbsents, now, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);

            response = response.OrderBy(x => x.StartDate).ToList();

            Absent reponseWorkPatternPart = response[0];
            reponseWorkPatternPart.StartDate.Should().Be(new DateTime(2023, 4, 4, 7, 0, 0));
            reponseWorkPatternPart.EndDate.Should().Be(new DateTime(2023, 4, 4, 10, 0, 0));
        }


        [Fact(DisplayName = "CheckIfAbsentNeedsSplitAsync should return one part")]
        public async Task CheckIfAbsentNeedsSplit_BeginingIsTodayEndsTomorrowAsync()
        {
            // Arrange
            DateTime now = new DateTime(2023, 4, 4, 12, 0, 0);
            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day,7,0,0),
                    EndDate = new DateTime(now.Year, now.Month, now.Day+1, 10,0,0),
                }
            };

            // Act
            var response = await _unsService.CheckIfAbsentNeedsSplitAsync(listAbsents, now, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);

            response = response.OrderBy(x => x.StartDate).ToList();

            Absent reponseWorkPatternPart = response[0];
            reponseWorkPatternPart.StartDate.Should().Be(new DateTime(2023, 4, 4, 7, 0, 0));
            reponseWorkPatternPart.EndDate.Should().Be(new DateTime(2023, 4, 5, 0, 0, 0));
        }



        [Fact(DisplayName = "CheckIfAbsentNeedsSplitAsync should return one part")]
        public async Task CheckIfAbsentNeedsSplit_BeginingIsYesterdayEndsTomorrowAsync()
        {
            // Arrange
            DateTime now = new DateTime(2023, 4, 4, 12, 0, 0);
            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day-1,7,0,0),
                    EndDate = new DateTime(now.Year, now.Month, now.Day+1, 10,0,0),
                }
            };

            // Act
            var response = await _unsService.CheckIfAbsentNeedsSplitAsync(listAbsents, now, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);

            response = response.OrderBy(x => x.StartDate).ToList();

            Absent reponseWorkPatternPart = response[0];
            reponseWorkPatternPart.StartDate.Should().Be(new DateTime(2023, 4, 4, 0, 0, 0));
            reponseWorkPatternPart.EndDate.Should().Be(new DateTime(2023, 4, 5, 0, 0, 0));
        }



        [Fact(DisplayName = "CheckIfAbsentNeedsSplitAsync should return one part")]
        public async Task CheckIfAbsentNeedsSplit_BeginingIsYesterdayEndsTodayAsync()
        {
            // Arrange
            DateTime now = new DateTime(2023, 4, 4, 12, 0, 0);
            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day-1,7,0,0),
                    EndDate = new DateTime(now.Year, now.Month, now.Day, 10,0,0),
                }
            };

            // Act
            var response = await _unsService.CheckIfAbsentNeedsSplitAsync(listAbsents, now, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);

            response = response.OrderBy(x => x.StartDate).ToList();

            Absent reponseWorkPatternPart = response[0];
            reponseWorkPatternPart.StartDate.Should().Be(new DateTime(2023, 4, 4, 0, 0, 0));
            reponseWorkPatternPart.EndDate.Should().Be(new DateTime(2023, 4, 4, 10, 0, 0));
        }

        [Fact(DisplayName = "CheckIfAbsentNeedsSplitAsync should return one part")]
        public async Task CheckIfAbsentNeedsSplit_BeginingTwoDaysEndsTodayAsync()
        {
            // Arrange
            DateTime now = new DateTime(2023, 4, 4, 12, 0, 0);
            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day-2,7,0,0),
                    EndDate = new DateTime(now.Year, now.Month, now.Day, 10,0,0),
                }
            };

            // Act
            var response = await _unsService.CheckIfAbsentNeedsSplitAsync(listAbsents, now, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);

            response = response.OrderBy(x => x.StartDate).ToList();

            Absent reponseWorkPatternPart = response[0];
            reponseWorkPatternPart.StartDate.Should().Be(new DateTime(2023, 4, 4, 0, 0, 0));
            reponseWorkPatternPart.EndDate.Should().Be(new DateTime(2023, 4, 4, 10, 0, 0));
        }

        [Fact(DisplayName = "CheckIfAbsentNeedsSplitAsync should return one part")]
        public async Task CheckIfAbsentNeedsSplit_BeginingIsYesterdayEndsInTwoDaysAsync()
        {
            // Arrange
            DateTime now = new DateTime(2023, 4, 4, 12, 0, 0);
            var listAbsents = new List<Absent>
            {
                new Absent
                {
                    StartDate = new DateTime(now.Year,now.Month,now.Day-1,7,0,0),
                    EndDate = new DateTime(now.Year, now.Month, now.Day+4, 10,0,0),
                }
            };

            // Act
            var response = await _unsService.CheckIfAbsentNeedsSplitAsync(listAbsents, now, CancellationToken.None);

            //Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);

            response = response.OrderBy(x => x.StartDate).ToList();

            Absent reponseWorkPatternPart = response[0];
            reponseWorkPatternPart.StartDate.Should().Be(new DateTime(2023, 4, 4, 0, 0, 0));
            reponseWorkPatternPart.EndDate.Should().Be(new DateTime(2023, 4, 5, 0, 0, 0));
        }



    }
}

