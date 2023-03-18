using FluentValidation.TestHelper;
using Project1.Application.WorkPatterns.Commands.DeleteWorkPattern;
using Xunit;

namespace PVSDashboard.Tests.Application.WorkPatterns.Commands.DeleteWorkPattern
{
    public class DeleteWorkPatternCommandValidatorTests
    {
        private DeleteWorkPatternCommandValidator _validator;

        public DeleteWorkPatternCommandValidatorTests()
        {
            _validator = new DeleteWorkPatternCommandValidator();
        }

        [Fact(DisplayName = "UserId when zero should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new DeleteWorkPatternCommand(new Guid());
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }


    }
}

