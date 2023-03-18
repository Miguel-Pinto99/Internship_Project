using FluentValidation.TestHelper;
using Project1.Application.WorkPatterns.Commands.CreateWorkPattern;
using Project1.Models;
using Xunit;

namespace PVSDashboard.Tests.Application.WorkPatterns.Commands.CreateWorkPattern
{
    public class CreateWorkPatternCommandValidatorTests
    {
        private CreateWorkPatternCommandValidator _validator;

        public CreateWorkPatternCommandValidatorTests()
        {
            _validator = new CreateWorkPatternCommandValidator();
        }

        [Fact(DisplayName = "UserId when zero should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new CreateWorkPatternCommand(0, new CreateWorkPatternCommandBody());
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.UserId);
        }

        [Fact(DisplayName = "UserId when two should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new CreateWorkPatternCommand(2, new CreateWorkPatternCommandBody());
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.UserId);
        }

        [Fact(DisplayName = "Body should have a child validator")]
        public void Body_ShouldHaveChildValidator()
        {
            var command = new CreateWorkPatternCommand(2, new CreateWorkPatternCommandBody());
            _validator.ShouldHaveChildValidator(command => command.Body, typeof(CreateWorkPatternCommandBodyValidator));
        }

        [Fact(DisplayName = "Body when StartDate is empty should have error")]
        public void StartDateIsEmpty_ShouldHaveError()
        {
            var createWorkPatternCommandBody = new CreateWorkPatternCommandBody();
            createWorkPatternCommandBody.StartDate = new DateTime();
            createWorkPatternCommandBody.EndDate = new DateTime(2023, 1, 1, 0, 0, 0);
            createWorkPatternCommandBody.Parts = new List<Project1.Models.WorkPatternPart>();
            var command = new CreateWorkPatternCommand(2, createWorkPatternCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.StartDate);
        }

        [Fact(DisplayName = "Body when StartDate is lower than Endate should not have error")]
        public void StartDateLowerThanEndDate_ShouldNotHaveError()
        {
            var createWorkPatternCommandBody = new CreateWorkPatternCommandBody();
            createWorkPatternCommandBody.StartDate = new DateTime(2022,1,1,0,0,0);
            createWorkPatternCommandBody.EndDate = new DateTime(2023, 1, 1, 0, 0, 0);
            createWorkPatternCommandBody.Parts = new List<Project1.Models.WorkPatternPart>();
            var command = new CreateWorkPatternCommand(2, createWorkPatternCommandBody);
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Body.EndDate);
        }

        [Fact(DisplayName = "Body when StartDate is higher than Endate should not have error")]
        public void StartDateHigherThanEndDate_ShouldHaveError()
        {
            var createWorkPatternCommandBody = new CreateWorkPatternCommandBody();
            createWorkPatternCommandBody.StartDate = new DateTime(2023, 1, 1, 0, 0, 0);
            createWorkPatternCommandBody.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);
            createWorkPatternCommandBody.Parts = new List<Project1.Models.WorkPatternPart>();
            var command = new CreateWorkPatternCommand(2, createWorkPatternCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.EndDate);
        }

        [Fact(DisplayName = "Body when list of parts is empty should have error")]
        public void ListPartsIsNull_ShouldHaveError()
        {
            var createWorkPatternCommandBody = new CreateWorkPatternCommandBody();
            createWorkPatternCommandBody.StartDate = new DateTime(2023, 1, 1, 0, 0, 0);
            createWorkPatternCommandBody.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);
            createWorkPatternCommandBody.Parts = new List<Project1.Models.WorkPatternPart>();
            var command = new CreateWorkPatternCommand(2, createWorkPatternCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.EndDate);
        }

    }
}

