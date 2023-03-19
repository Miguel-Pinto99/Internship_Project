using FluentValidation;
using Project1.Application.Absent.Commands.CreateAbsent;
using Project1.Application.ApplicationUser.Commands.CreateApplicationUser;

namespace Project1.Application.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreateApplicationUserCommandValidator : AbstractValidator<CreateApplicationUserCommand>
    {
        public CreateApplicationUserCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Body).SetValidator(new CreateApplicationUserCommandBodyValidator());
        }
    }
}