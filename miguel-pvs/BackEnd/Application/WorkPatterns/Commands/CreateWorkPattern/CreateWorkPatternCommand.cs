using MediatR;
using Project1.Models;
using System.Text.Json.Serialization;

namespace Project1.Application.WorkPatterns.Commands.CreateWorkPattern
{

    public class CreateWorkPatternCommand : IRequest<CreateWorkPatternResponse>
        {

            [JsonIgnore]
            public Models.ApplicationUser? User { get; set; }
            public int UserId { get; set; }
            public CreateWorkPatternCommandBody Body { get; set; }
            public CreateWorkPatternCommand(int id, CreateWorkPatternCommandBody body)
            {
                UserId = id;
                Body = body;
            }

        }
}