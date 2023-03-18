using FluentValidation.TestHelper;
using Project1.Application.ApplicationUsers.Queries.CheckOutApplicationUser;
using Xunit;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Commands.CheckOutApplicationUser
{
    public class CheckOutApplicationUserCommandValidatorTests
    {
        private CheckOutApplicationUserCommandValidator _validator;

        public CheckOutApplicationUserCommandValidatorTests()
        {
            _validator = new CheckOutApplicationUserCommandValidator();
        }

        [Fact(DisplayName = "UserId when zero should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new CheckOutApplicationUserCommand(0);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }


        [Fact(DisplayName = "UserId when two should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new CheckOutApplicationUserCommand(2);
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }

    }
}

