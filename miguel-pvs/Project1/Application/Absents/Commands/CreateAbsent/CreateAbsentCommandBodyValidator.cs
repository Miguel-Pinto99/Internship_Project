using FluentValidation;

namespace Project1.Application.Absent.Commands.CreateAbsent
{
    public class CreateAbsentCommandBodyValidator : AbstractValidator<CreateAbsentCommandBody>
    {
        public CreateAbsentCommandBodyValidator()
        {

            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        }
    }
}
