using FluentValidation.TestHelper;
using Xunit;
using Project1.Application.WorkPatterns.Queries.GetAllWorkPattern;

namespace PVSDashboard.Tests.Application.WorkPatterns.Queries.GetAllWorkPattern
{
    public class GetAllWorkPatternCommandValidatorTests
    {
        private GetAllWorkPatternCommandValidator _validator;

        public GetAllWorkPatternCommandValidatorTests()
        {
            _validator = new GetAllWorkPatternCommandValidator();
        }

        [Fact(DisplayName = "when create a command should not have error")]
        public void Command_WhenCreated_ShouldNotHaveError()
        {
            var command = new GetAllWorkPatternCommand();
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command);
        }
    }
}

