using Project1.Application.Absent.Commands.CreateAbsent;
using FluentValidation.TestHelper;
using Xunit;

namespace PVSDashboard.Tests.Application.Absent.Commands.CreateAbsent
{
    public class CreateAbsentCommandValidatorTests
    {
        private CreateAbsentCommandValidator _validator;

        public CreateAbsentCommandValidatorTests()
        {
            _validator = new CreateAbsentCommandValidator();
        }

        [Fact(DisplayName = "UserId when zero should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new CreateAbsentCommand(0, new CreateAbsentCommandBody());
           _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.UserId);
        }

        [Fact(DisplayName = "UserId when two should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new CreateAbsentCommand(2, new CreateAbsentCommandBody());
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.UserId);
        }

        [Fact(DisplayName = "Body should have a child validator")]
        public void Body_ShouldHaveChildValidator()
        {
            var command = new CreateAbsentCommand(2, new CreateAbsentCommandBody());
            _validator.ShouldHaveChildValidator(command => command.Body, typeof(CreateAbsentCommandBodyValidator));
        }

        [Fact(DisplayName = "Body when start time is empty should have error")]
        public void StartTimeEmpty_ShouldHaveError()
        {
            var createAbsentCommandBody = new CreateAbsentCommandBody();
            createAbsentCommandBody.StartDate = new DateTime();
            createAbsentCommandBody.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);
            var command = new CreateAbsentCommand(2, createAbsentCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.StartDate);
        }

        [Fact(DisplayName = "Body when start time lower than end time should not have error")]
        public void StartTimeLowerEndTime_ShouldNotHaveError()
        {
            var createAbsentCommandBody = new CreateAbsentCommandBody();
            createAbsentCommandBody.StartDate = new DateTime(2000, 1, 1, 0, 0, 0);
            createAbsentCommandBody.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);
            var command = new CreateAbsentCommand(2, createAbsentCommandBody);
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Body.EndDate);
        }

        [Fact(DisplayName = "Body when start time equal end time should have error")]
        public void StartTimeEqualEndTime_ShouldNotHaveError()
        {
            var createAbsentCommandBody = new CreateAbsentCommandBody();
            createAbsentCommandBody.StartDate = new DateTime(2022, 1, 1, 0, 0, 0);
            createAbsentCommandBody.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);
            var command = new CreateAbsentCommand(2, createAbsentCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.EndDate);
        }

        [Fact(DisplayName = "Body when start time higher than end time should have error")]
        public void StartTimeHigherEndTime_ShouldNotHaveError()
        {
            var createAbsentCommandBody = new CreateAbsentCommandBody();
            createAbsentCommandBody.StartDate = new DateTime(2022, 1, 1, 0, 0, 0);
            createAbsentCommandBody.EndDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var command = new CreateAbsentCommand(2, createAbsentCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.EndDate);
        }
    }
}

