using FluentValidation;

namespace Project1.Application.WorkPatterns.Commands.DeleteWorkPattern
{
    public class DeleteWorkPatternCommandValidator : AbstractValidator<DeleteWorkPatternCommand>
    {
        public DeleteWorkPatternCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}