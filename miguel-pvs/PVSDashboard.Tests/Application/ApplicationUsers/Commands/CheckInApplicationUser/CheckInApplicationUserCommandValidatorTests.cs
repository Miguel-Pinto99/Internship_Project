using FluentValidation.TestHelper;
using Project1.Application.ApplicationUsers.Queries.CheckInApplicationUser;
using Xunit;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Commands.CheckInApplicationUser
{
    public class CheckInApplicationUserCommandValidatorTests
    {
        private CheckInApplicationUserCommandValidator _validator;

        public CheckInApplicationUserCommandValidatorTests()
        {
            _validator = new CheckInApplicationUserCommandValidator();
        }

        [Fact(DisplayName = "UserId when zero should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new CheckInApplicationUserCommand(0);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "UserId when two should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new CheckInApplicationUserCommand(2);
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }

    }
}

