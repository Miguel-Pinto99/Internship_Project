using MediatR;

namespace Project1.Application.ApplicationUsers.Commands.DeleteApplicationUser
{
    public class DeleteApplicationUserCommand : IRequest<DeleteApplicationUserResponse>
    {
        public int Id { get; set; }

        public DeleteApplicationUserCommand(int id)
        {
            Id = id;
        }
    }
}