using FluentValidation;
using Project1.Application.ApplicationUsers.Queries.EditWorkPatterm;

namespace Project1.Application.Absents.Commands.EditAbsent
{
    public class EditAbsentCommandBodyValidator : AbstractValidator<EditAbsentCommandBody>
    {
        public EditAbsentCommandBodyValidator()
        {

            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        }
    }
}
