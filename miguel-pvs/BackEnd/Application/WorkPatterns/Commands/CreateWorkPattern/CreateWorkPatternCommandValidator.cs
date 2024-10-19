using FluentValidation;

namespace Project1.Application.WorkPatterns.Commands.CreateWorkPattern
{
    public class CreateWorkPatternCommandValidator : AbstractValidator<CreateWorkPatternCommand>
    {
        public CreateWorkPatternCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Body).SetValidator(new CreateWorkPatternCommandBodyValidator());
        }
    }
}