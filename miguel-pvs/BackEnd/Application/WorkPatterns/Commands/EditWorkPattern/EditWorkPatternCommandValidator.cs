using FluentValidation;
using Project1.Application.WorkPatterns.Commands.CreateWorkPattern;

namespace Project1.Application.WorkPatterns.Commands.EditWorkPattern
{
    public class EditWorkPatternCommandValidator : AbstractValidator<EditWorkPatternCommand>
    {
        public EditWorkPatternCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Body).SetValidator(new EditWorkPatternCommandBodyValidator());
        }
    }
}