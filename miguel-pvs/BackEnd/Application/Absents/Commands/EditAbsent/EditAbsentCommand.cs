using MediatR;
using Project1.Application.ApplicationUsers.Queries.EditWorkPatterm;

namespace Project1.Application.Absent.Commands.EditAbsent
{
    public class EditAbsentCommand : IRequest<EditAbsentResponse>
    {
        public Guid Id { get; set; }
        public EditAbsentCommandBody Body { get; set; }
        public EditAbsentCommand(Guid id, EditAbsentCommandBody body)
        {
            Id = id;
            Body = body;
        }
    }
}
