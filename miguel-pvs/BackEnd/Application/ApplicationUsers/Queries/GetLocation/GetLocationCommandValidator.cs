using FluentValidation;

namespace Project1.Application.ApplicationUsers.Queries.GetLocation
{
    public class GetLocationCommandValidator : AbstractValidator<GetLocationCommand>
    {
        public GetLocationCommandValidator()
        {
            RuleFor(x => x.OfficeLocation).GreaterThan(0);
        }
    }
}