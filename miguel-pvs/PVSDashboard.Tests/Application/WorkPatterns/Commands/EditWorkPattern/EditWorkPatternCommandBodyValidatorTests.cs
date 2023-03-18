using FluentValidation.TestHelper;
using Project1.Application.WorkPatterns.Commands.EditWorkPattern;
using Project1.Models;
using Xunit;

namespace PVSDashboard.Tests.Application.WorkPatterns.Commands.EditWorkPattern
{
    public class EditWorkPatternCommandBodyValidatorTests
    {
        private readonly WorkPatternPartValidator _validator;

        public EditWorkPatternCommandBodyValidatorTests()
        {
            _validator = new WorkPatternPartValidator();
        }

        [Fact(DisplayName = "Body when StartTimePart is lower than EndTimePart should have error")]
        public void StartTimeLowerThanEndTime_ShouldHaveError()
        {
            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(-1),
                EndTime = TimeSpan.FromHours(2),
                Day = DateTime.Today.Date.DayOfWeek
            };
            _validator.TestValidate(part).ShouldHaveValidationErrorFor(command => command.StartTime);
        }

        [Fact(DisplayName = "Body when StartTimePart is higher than EndTimePart should have error")]
        public void StartTimeHigherThanEndTime_ShouldHaveError()
        {
            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(3),
                EndTime = TimeSpan.FromHours(2),
                Day = DateTime.Today.Date.DayOfWeek
            };
            _validator.TestValidate(part).ShouldHaveValidationErrorFor(command => command.EndTime);
        }

        [Fact(DisplayName = "Body when EndTimePart is higher than 24h should have error")]
        public void EndTimeHigherThan24h_ShouldHaveError()
        {
            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(1),
                EndTime = TimeSpan.FromHours(25),
                Day = DateTime.Today.Date.DayOfWeek
            };
            _validator.TestValidate(part).ShouldHaveValidationErrorFor(command => command.EndTime);
        }

        [Fact(DisplayName = "Body when EndTimePart is higher than 24h should have error")]
        public void DayIsEmpty_ShouldHaveError()
        {
            var part = new WorkPatternPart
            {
                StartTime = TimeSpan.FromHours(1),
                EndTime = TimeSpan.FromHours(23),
                Day = new DayOfWeek(),
            };
            _validator.TestValidate(part).ShouldHaveValidationErrorFor(command => command.Day);
        }

    }
}

