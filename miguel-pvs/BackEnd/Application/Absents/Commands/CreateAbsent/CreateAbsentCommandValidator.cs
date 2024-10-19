using FluentValidation;

namespace Project1.Application.Absent.Commands.CreateAbsent
{
    public class CreateAbsentCommandValidator : AbstractValidator<CreateAbsentCommand>
    {
        public CreateAbsentCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Body).SetValidator(new CreateAbsentCommandBodyValidator());
        }
    }
}