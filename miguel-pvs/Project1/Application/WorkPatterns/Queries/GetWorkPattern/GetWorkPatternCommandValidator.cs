using FluentValidation;

namespace Project1.Application.WorkPatterns.Queries.GetWorkPattern
{
    public class GetWorkPatternCommandValidator : AbstractValidator<GetWorkPatternCommand>
    {
        public GetWorkPatternCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}