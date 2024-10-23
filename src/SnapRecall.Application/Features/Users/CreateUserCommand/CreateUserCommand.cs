using MediatR;

namespace SnapRecall.Application.Features.Users.CreateUserCommand
{
    public class CreateUserCommand : IRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string LastExecutedCommand { get; set; }
    }
}
