using Project1.Application.Absent.Commands.DeleteAbsent;
using FluentValidation.TestHelper;
using Xunit;
using Project1.Application.Absent.Commands.EditAbsent;
using Project1.Application.ApplicationUsers.Queries.EditWorkPatterm;
using Project1.Application.Absents.Commands.EditAbsent;

namespace PVSDashboard.Tests.Application.Absents.Commands.EditAbsent
{
    public class EditAbsentCommandValidatorTests
    {
        private EditAbsentCommandValidator _validator;

        public EditAbsentCommandValidatorTests()
        {
            _validator = new EditAbsentCommandValidator();
        }

        [Fact(DisplayName = "Id when zero should have error")]
        public void Id_WhenZero_ShouldHaveError()
        {
            var command = new EditAbsentCommand(new Guid(), new EditAbsentCommandBody());
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "Id when two should not have error")]
        public void Id_WhenTwo_ShouldNotHaveError()
        {
            var command = new EditAbsentCommand(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new EditAbsentCommandBody());
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "Body should have a child validator")]
        public void Body_ShouldHaveChildValidator()
        {
            var command = new EditAbsentCommand(new Guid(), new EditAbsentCommandBody());
            _validator.ShouldHaveChildValidator(command => command.Body, typeof(EditAbsentCommandBodyValidator));
        }


        [Fact(DisplayName = "Body when start time is empty should have error")]
        public void StartTimeEmpty_ShouldHaveError()
        {
            var editAbsentCommandBody = new EditAbsentCommandBody();
            editAbsentCommandBody.StartDate = new DateTime();
            editAbsentCommandBody.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);
            var command = new EditAbsentCommand(new Guid(), editAbsentCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.StartDate);
        }

        [Fact(DisplayName = "Body when start time lower than end time should not have error")]
        public void StartTimeLowerEndTime_ShouldNotHaveError()
        {
            var editAbsentCommandBody = new EditAbsentCommandBody();
            editAbsentCommandBody.StartDate = new DateTime(2000, 1, 1, 0, 0, 0);
            editAbsentCommandBody.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);
            var command = new EditAbsentCommand(new Guid(), editAbsentCommandBody);
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Body.EndDate);
        }

        [Fact(DisplayName = "Body when start time equal end time should have error")]
        public void StartTimeEqualEndTime_ShouldNotHaveError()
        {
            var editAbsentCommandBody = new EditAbsentCommandBody();
            editAbsentCommandBody.StartDate = new DateTime(2022, 1, 1, 0, 0, 0);
            editAbsentCommandBody.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);
            var command = new EditAbsentCommand(new Guid(), editAbsentCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.EndDate);
        }

        [Fact(DisplayName = "Body when start time higher than end time should have error")]
        public void StartTimeHigherEndTime_ShouldNotHaveError()
        {
            var editAbsentCommandBody = new EditAbsentCommandBody();
            editAbsentCommandBody.StartDate = new DateTime(2022, 1, 1, 0, 0, 0);
            editAbsentCommandBody.EndDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var command = new EditAbsentCommand(new Guid(), editAbsentCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.EndDate);
        }
    }
}



