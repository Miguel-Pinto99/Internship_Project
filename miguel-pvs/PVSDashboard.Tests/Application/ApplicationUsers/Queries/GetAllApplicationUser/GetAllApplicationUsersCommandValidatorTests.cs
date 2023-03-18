using FluentValidation.TestHelper;
using Project1.Application.ApplicationUsers.Queries.GetAllApplicationUser;
using Xunit;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Queries.GetAllApplicationUser
{
    public class GetAllApplicationUsersCommandValidatorTests
    {
        private GetAllApplicationUserCommandValidator _validator;

        public GetAllApplicationUsersCommandValidatorTests()
        {
            _validator = new GetAllApplicationUserCommandValidator();
        }

        [Fact(DisplayName = "when create a command should not have error")]
        public void Command_WhenCreated_ShouldNotHaveError()
        {
            var command = new GetAllApplicationUserCommand();
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command);
        }
    }
}

