using MediatR;

namespace SnapRecall.Application.Features.Topics.CreateTopicCommand
{
    public class CreateTopicCommand : IRequest
    {
        public long AuthorId { get; set; }
    }
}
