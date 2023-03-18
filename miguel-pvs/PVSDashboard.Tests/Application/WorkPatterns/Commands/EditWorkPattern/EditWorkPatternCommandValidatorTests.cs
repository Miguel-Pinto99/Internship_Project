using FluentValidation.TestHelper;
using Project1.Application.ApplicationUsers.Queries.EditWorkPatterm;
using Project1.Application.WorkPatterns.Commands.EditWorkPattern;
using Project1.Models;
using Xunit;

namespace PVSDashboard.Tests.Application.WorkPatterns.Commands.EditWorkPattern
{
    public class EditWorkPatternCommandValidatorTests
    {
        private EditWorkPatternCommandValidator _validator;

        public EditWorkPatternCommandValidatorTests()
        {
            _validator = new EditWorkPatternCommandValidator();
        }

        [Fact(DisplayName = "UserId when zero should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new EditWorkPatternCommand(new Guid(), new EditWorkPatternCommandBody());
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "UserId when two should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new EditWorkPatternCommand(new Guid("00033300-1111-1111-1111-000000333000"), new EditWorkPatternCommandBody());
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "Body should have a child validator")]
        public void Body_ShouldHaveChildValidator()
        {
            var command = new EditWorkPatternCommand(new Guid(), new EditWorkPatternCommandBody());
            _validator.ShouldHaveChildValidator(command => command.Body, typeof(EditWorkPatternCommandBodyValidator));
        }

        [Fact(DisplayName = "Body when StartDate is empty should have error")]
        public void StartDateIsEmpty_ShouldHaveError()
        {
            var editWorkPatternCommandBody = new EditWorkPatternCommandBody();
            editWorkPatternCommandBody.StartDate = new DateTime();
            editWorkPatternCommandBody.EndDate = new DateTime(2023, 1, 1, 0, 0, 0);
            editWorkPatternCommandBody.Parts = new List<Project1.Models.WorkPatternPart>();
            var command = new EditWorkPatternCommand(new Guid("00033300-1111-1111-1111-000000333000"), editWorkPatternCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.StartDate);
        }

        [Fact(DisplayName = "Body when StartDate is lower than Endate should not have error")]
        public void StartDateLowerThanEndDate_ShouldNotHaveError()
        {
            var editWorkPatternCommandBody = new EditWorkPatternCommandBody();
            editWorkPatternCommandBody.StartDate = new DateTime(2022, 1, 1, 0, 0, 0);
            editWorkPatternCommandBody.EndDate = new DateTime(2023, 1, 1, 0, 0, 0);
            editWorkPatternCommandBody.Parts = new List<Project1.Models.WorkPatternPart>();
            var command = new EditWorkPatternCommand(new Guid("00033300-1111-1111-1111-000000333000"), editWorkPatternCommandBody);
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Body.EndDate);
        }

        [Fact(DisplayName = "Body when StartDate is higher than Endate should not have error")]
        public void StartDateHigherThanEndDate_ShouldHaveError()
        {
            var editWorkPatternCommandBody = new EditWorkPatternCommandBody();
            editWorkPatternCommandBody.StartDate = new DateTime(2023, 1, 1, 0, 0, 0);
            editWorkPatternCommandBody.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);
            editWorkPatternCommandBody.Parts = new List<Project1.Models.WorkPatternPart>();
            var command = new EditWorkPatternCommand(new Guid("00033300-1111-1111-1111-000000333000"), editWorkPatternCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.EndDate);
        }

        [Fact(DisplayName = "Body when list of parts is empty should have error")]
        public void ListPartsIsNull_ShouldHaveError()
        {
            var editWorkPatternCommandBody = new EditWorkPatternCommandBody();
            editWorkPatternCommandBody.StartDate = new DateTime(2023, 1, 1, 0, 0, 0);
            editWorkPatternCommandBody.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);
            editWorkPatternCommandBody.Parts = new List<Project1.Models.WorkPatternPart>();
            var command = new EditWorkPatternCommand(new Guid("00033300-1111-1111-1111-000000333000"), editWorkPatternCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.EndDate);
        }

    }
}

