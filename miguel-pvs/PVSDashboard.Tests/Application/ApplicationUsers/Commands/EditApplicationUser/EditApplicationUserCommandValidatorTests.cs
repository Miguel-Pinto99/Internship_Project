using FluentValidation.TestHelper;
using Project1.Application.ApplicationUsers.Commands.EditApplicationUser;
using Project1.Application.ApplicationUsers.Queries.EditApplicationUser;
using Xunit;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Commands.EditApplicationUser
{
    public class EditApplicationUserCommandValidatorTests
    {
        private EditApplicationUserCommandValidator _validator;

        public EditApplicationUserCommandValidatorTests()
        {
            _validator = new EditApplicationUserCommandValidator();
        }

        [Fact(DisplayName = "UserId when zero should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new EditApplicationUserCommand(0, new EditApplicationUserCommandBody());
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "UserId when two should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new EditApplicationUserCommand(2, new EditApplicationUserCommandBody());
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "Body should have a child validator")]
        public void Body_ShouldHaveChildValidator()
        {
            var command = new EditApplicationUserCommand(2, new EditApplicationUserCommandBody());
            _validator.ShouldHaveChildValidator(command => command.Body, typeof(EditApplicationUserCommandBodyValidator));
        }

        [Fact(DisplayName = "Body when name is null should have error")]
        public void NameIsNull_ShouldHaveError()
        {
            var editApplicationUserCommandBody = new EditApplicationUserCommandBody();
            editApplicationUserCommandBody.FirstName = null;
            editApplicationUserCommandBody.OfficeLocation = 1;
            var command = new EditApplicationUserCommand(2, editApplicationUserCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.FirstName);
        }



        [Fact(DisplayName = "Body when name is empty should have error")]
        public void NameIsEmpty_ShouldHaveError()
        {
            var editApplicationUserCommandBody = new EditApplicationUserCommandBody();
            editApplicationUserCommandBody.FirstName = "";
            editApplicationUserCommandBody.OfficeLocation = 1;
            var command = new EditApplicationUserCommand(2, editApplicationUserCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.FirstName);
        }

        [Fact(DisplayName = "Body when office location is lower than zero should have error")]
        public void OfficeLocationIsZero_ShouldHaveError()
        {
            var editApplicationUserCommandBody = new EditApplicationUserCommandBody();
            editApplicationUserCommandBody.FirstName = "Miguel";
            editApplicationUserCommandBody.OfficeLocation = 0;
            var command = new EditApplicationUserCommand(2, editApplicationUserCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.OfficeLocation);
        }

    }
}

