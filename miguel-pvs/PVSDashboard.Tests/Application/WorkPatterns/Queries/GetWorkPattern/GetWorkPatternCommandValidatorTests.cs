using FluentValidation.TestHelper;
using Project1.Application.WorkPatterns.Queries.GetWorkPattern;
using Xunit;

namespace PVSDashboard.Tests.Application.WorkPatterns.Queries.GetWorkPattern
{
    public class GetWorkPatternCommandValidatorTests
    {
        private GetWorkPatternCommandValidator _validator;

        public GetWorkPatternCommandValidatorTests()
        {
            _validator = new GetWorkPatternCommandValidator();
        }

        [Fact(DisplayName = "Id when empty should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new GetWorkPatternCommand(new Guid());
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "Id when guid should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new GetWorkPatternCommand(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }
    }
}

