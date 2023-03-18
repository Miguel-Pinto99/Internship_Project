using FluentValidation.TestHelper;
using Project1.Application.Absent.Queries.GetAbsentByIdById;
using Xunit;

namespace PVSDashboard.Tests.Application.Absents.Queries.GetById
{
    public class GetByIdCommandValidatorTests
    {
        private GetAbsentByIdCommandValidator _validator;

        public GetByIdCommandValidatorTests()
        {
            _validator = new GetAbsentByIdCommandValidator();
        }

        [Fact(DisplayName = "Id when empty should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new GetAbsentByIdCommand(0);
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.UserId);
        }

        [Fact(DisplayName = "Id when Id is 2 should not have error")]
        public void UserId_WhenTwo_ShouldNotHaveError()
        {
            var command = new GetAbsentByIdCommand(2);
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.UserId);
        }
    }
}

