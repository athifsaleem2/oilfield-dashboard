// Application/Features/Wells/Commands/DeleteWell/DeleteWellCommand.cs
using MediatR;

namespace OilfieldDashboard.Application.Features.Wells.Commands.DeleteWell
{
    public class DeleteWellCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteWellCommand(int id)
        {
            Id = id;
        }
    }
}