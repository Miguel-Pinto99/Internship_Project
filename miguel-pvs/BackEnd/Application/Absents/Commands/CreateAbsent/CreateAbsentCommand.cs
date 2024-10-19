using MediatR;
using Project1.Models;
using System.Text.Json.Serialization;

namespace Project1.Application.Absent.Commands.CreateAbsent
{

    public class CreateAbsentCommand : IRequest<CreateAbsentResponse>
        {

            [JsonIgnore]
            public Models.ApplicationUser? User { get; set; }
            public int UserId { get; set; }
            public CreateAbsentCommandBody Body { get; set; }
            public CreateAbsentCommand(int id, CreateAbsentCommandBody body)
            {
                UserId = id;
                Body = body;
            }

        }
}