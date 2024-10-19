using FluentValidation;

namespace Project1.Application.Absent.Queries.GetAbsentByIdById
{
    public class GetAbsentByIdCommandValidator : AbstractValidator<GetAbsentByIdCommand>
    {
        public GetAbsentByIdCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}