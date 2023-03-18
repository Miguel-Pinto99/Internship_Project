using FluentValidation.TestHelper;
using Project1.Application.ApplicationUsers.Commands.DeleteApplicationUser;
using Xunit;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Commands.DeleteApplicationUser
{
    public class DeleteApplicationUserCommandValidatorTests
    {
        private DeleteApplicationUserCommandValidator _validator;

        public DeleteApplicationUserCommandValidatorTests()
        {
            _validator = new DeleteApplicationUserCommandValidator();
        }

        [Fact(DisplayName = "UserId when zero should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new DeleteApplicationUserCommand(0);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "UserId when two should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new DeleteApplicationUserCommand(2);
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }

    }
}

