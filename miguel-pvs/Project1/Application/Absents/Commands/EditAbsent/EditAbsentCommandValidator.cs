using FluentValidation;
using Project1.Application.Absents.Commands.EditAbsent;

namespace Project1.Application.Absent.Commands.EditAbsent
{
    public class EditAbsentCommandValidator : AbstractValidator<EditAbsentCommand>
    {
        public EditAbsentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Body).SetValidator(new EditAbsentCommandBodyValidator());
        }
    }
}