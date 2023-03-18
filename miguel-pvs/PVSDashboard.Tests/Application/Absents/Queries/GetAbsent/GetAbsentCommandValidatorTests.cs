using FluentValidation.TestHelper;
using Project1.Application.Absent.Queries.GetAbsent;
using Xunit;

namespace PVSDashboard.Tests.Application.Absents.Queries.GetAbsent
{
    public class GetAbsentCommandValidatorTests
    {
        private GetAbsentCommandValidator _validator;

        public GetAbsentCommandValidatorTests()
        {
            _validator = new GetAbsentCommandValidator();
        }

        [Fact(DisplayName = "Id when empty should have error")]
        public void UserId_WhenZero_ShouldHaveError()
        {
            var command = new GetAbsentCommand(new Guid());
            _validator.TestValidate(command).ShouldHaveValidationErrorFor(command => command.Id);
        }

        [Fact(DisplayName = "Id when guid should not have error")]
        public void UserId_WhenGuid_ShouldNotHaveError()
        {
            var command = new GetAbsentCommand(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"));
            _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(command => command.Id);
        }
    }
}

