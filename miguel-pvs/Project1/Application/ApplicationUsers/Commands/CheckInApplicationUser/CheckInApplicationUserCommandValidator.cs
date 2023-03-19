using FluentValidation;

namespace Project1.Application.ApplicationUsers.Queries.CheckInApplicationUser
{
    public class CheckInApplicationUserCommandValidator : AbstractValidator<CheckInApplicationUserCommand>
    {
        public CheckInApplicationUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}