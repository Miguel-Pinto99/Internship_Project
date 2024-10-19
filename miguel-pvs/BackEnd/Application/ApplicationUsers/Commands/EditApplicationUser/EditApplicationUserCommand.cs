using MediatR;

namespace Project1.Application.ApplicationUsers.Queries.EditApplicationUser
{
    public class EditApplicationUserCommand : IRequest<EditApplicationUserResponse>
    {
        public int Id { get; set; }
        public EditApplicationUserCommandBody Body { get; set; }
        public EditApplicationUserCommand(int id, EditApplicationUserCommandBody body)
        {
            Id = id;
            Body = body;
        }
    }
}