using Project1.Application.Absent.Commands.DeleteAbsent;
using FluentValidation.TestHelper;
using Xunit;

namespace PVSDashboard.Tests.Application.Absents.Commands.DeleteAbsent
{
    public class DeleteAbsentCommandValidatorTests
    {
        private DeleteAbsentCommandValidator _validator;

        public DeleteAbsentCommandValidatorTests()
        {
            _validator = new DeleteAbsentCommandValidator();
        }

        [Fact(DisplayName = "Id when empty should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new DeleteAbsentCommand(new Guid());
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "Id when guid should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new DeleteAbsentCommand(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }
    }
}

