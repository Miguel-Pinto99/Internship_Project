using FluentValidation.TestHelper;
using Project1.Application.ApplicationUser.Commands.CreateApplicationUser;
using Project1.Application.ApplicationUsers.Commands.CreateApplicationUser;
using Xunit;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreateApplicationUserCommandValidatorTests
    {
        private CreateApplicationUserCommandValidator _validator;

        public CreateApplicationUserCommandValidatorTests()
        {
            _validator = new CreateApplicationUserCommandValidator();
        }

        [Fact(DisplayName = "UserId when zero should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new CreateApplicationUserCommand(0, new CreateApplicationUserCommandBody());
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "UserId when two should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new CreateApplicationUserCommand(2, new CreateApplicationUserCommandBody());
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "Body should have a child validator")]
        public void Body_ShouldHaveChildValidator()
        {
            var command = new CreateApplicationUserCommand(2, new CreateApplicationUserCommandBody());
            _validator.ShouldHaveChildValidator(command => command.Body, typeof(CreateApplicationUserCommandBodyValidator));
        }

        [Fact(DisplayName = "Body when name is null should have error")]
        public void NameIsNull_ShouldHaveError()
        {
            var createApplicationUserCommandBody = new CreateApplicationUserCommandBody();
            createApplicationUserCommandBody.FirstName = null;
            createApplicationUserCommandBody.OfficeLocation = 1;
            var command = new CreateApplicationUserCommand(2, createApplicationUserCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.FirstName);
        }

        [Fact(DisplayName = "Body when name is empty should have error")]
        public void NameIsEmpty_ShouldHaveError()
        {
            var createApplicationUserCommandBody = new CreateApplicationUserCommandBody();
            createApplicationUserCommandBody.FirstName = "";
            createApplicationUserCommandBody.OfficeLocation = 1;
            var command = new CreateApplicationUserCommand(2, createApplicationUserCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.FirstName);
        }

        [Fact(DisplayName = "Body when office location is lower than zero should have error")]
        public void OfficeLocationIsZero_ShouldHaveError()
        {
            var createApplicationUserCommandBody = new CreateApplicationUserCommandBody();
            createApplicationUserCommandBody.FirstName = "Miguel";
            createApplicationUserCommandBody.OfficeLocation = 0;
            var command = new CreateApplicationUserCommand(2, createApplicationUserCommandBody);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Body.OfficeLocation);
        }
    }
}

