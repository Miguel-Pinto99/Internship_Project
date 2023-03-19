using FluentValidation;

namespace Project1.Application.Absent.Queries.GetAbsent
{
    public class GetAbsentCommandValidator : AbstractValidator<GetAbsentCommand>
    {
        public GetAbsentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}