using FluentValidation.TestHelper;
using Project1.Application.ApplicationUsers.Queries.GetLocation;
using Xunit;

namespace PVSDashboard.Tests.Application.ApplicationUsers.Queries.GetLocation
{
    public class GetLocationCommandValidatorTests
    {
        private GetLocationCommandValidator _validator;

        public GetLocationCommandValidatorTests()
        {
            _validator = new GetLocationCommandValidator();
        }

        [Fact(DisplayName = "Id when is zero should not have error")]
        public void UserId_WhenZero_ShouldNotHaveError()
        {
            var command = new GetLocationCommand(new int());
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command);
        }

        [Fact(DisplayName = "Id when is two should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new GetLocationCommand(2);
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command);
        }
    }
}

