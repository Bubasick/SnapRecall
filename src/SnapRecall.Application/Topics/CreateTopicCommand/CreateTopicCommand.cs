using MediatR;

namespace SnapRecall.Application.Topics.CreateTopicCommand
{
    public class CreateTopicCommand :IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
