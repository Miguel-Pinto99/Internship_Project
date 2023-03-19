using MediatR;
using Project1.Application.ApplicationUsers.Queries.EditWorkPatterm;

namespace Project1.Application.WorkPatterns.Commands.EditWorkPattern
{
    public class EditWorkPatternCommand : IRequest<EditWorkPatternResponse>
    {
        public Guid Id { get; set; }
        public EditWorkPatternCommandBody Body { get; set; }
        public EditWorkPatternCommand(Guid id, EditWorkPatternCommandBody body)
        {
            Id = id;
            Body = body;
        }
    }
}
