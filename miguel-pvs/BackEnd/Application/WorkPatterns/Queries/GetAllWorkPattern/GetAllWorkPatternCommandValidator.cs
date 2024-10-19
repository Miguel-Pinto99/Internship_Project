using FluentValidation;

namespace Project1.Application.WorkPatterns.Queries.GetAllWorkPattern
{
    public class GetAllWorkPatternCommandValidator : AbstractValidator<GetAllWorkPatternCommand>
    {
        public GetAllWorkPatternCommandValidator()
        {
        }
    }
}