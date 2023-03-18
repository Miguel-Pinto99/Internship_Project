using FluentValidation.TestHelper;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Xunit;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Queries.GetApplicationUser
{
    public class GetApplicationUserCommandValidatorTests
    {
        private GetApplicationUserCommandValidator _validator;

        public GetApplicationUserCommandValidatorTests()
        {
            _validator = new GetApplicationUserCommandValidator();
        }

        [Fact(DisplayName = "Id when empty should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new GetApplicationUserCommand(new int());
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "Id when guid should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new GetApplicationUserCommand(2);
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }
    }
}

