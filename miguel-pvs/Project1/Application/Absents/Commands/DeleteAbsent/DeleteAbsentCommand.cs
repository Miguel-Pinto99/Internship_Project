using MediatR;
using Project1.Application.Absent.Commands.CreateAbsent;

namespace Project1.Application.Absent.Commands.DeleteAbsent
{
    public class DeleteAbsentCommand : IRequest<DeleteAbsentResponse>
    {
        public Guid Id { get; set; }

        public DeleteAbsentCommand(Guid id)
        {
            Id = id;
        }

    }
}