using FluentValidation;
using Project1.Application.ApplicationUsers.Commands.EditApplicationUser;

namespace Project1.Application.ApplicationUsers.Queries.EditApplicationUser
{
    public class EditApplicationUserCommandValidator : AbstractValidator<EditApplicationUserCommand>
    {
        public EditApplicationUserCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Body).SetValidator(new EditApplicationUserCommandBodyValidator());
        }
    }
}