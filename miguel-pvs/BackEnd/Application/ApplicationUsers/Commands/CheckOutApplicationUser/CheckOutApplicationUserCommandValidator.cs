using FluentValidation;

namespace Project1.Application.ApplicationUsers.Queries.CheckOutApplicationUser
{
    public class CheckOutApplicationUserCommandValidator : AbstractValidator<CheckOutApplicationUserCommand>
    {
        public CheckOutApplicationUserCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}