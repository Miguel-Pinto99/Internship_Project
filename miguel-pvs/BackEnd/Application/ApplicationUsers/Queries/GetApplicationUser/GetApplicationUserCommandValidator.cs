using FluentValidation;

namespace Project1.Application.ApplicationUsers.Queries.GetApplicationUser
{
    public class GetApplicationUserCommandValidator : AbstractValidator<GetApplicationUserCommand>
    {
        public GetApplicationUserCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}