using MediatR;
using SnapRecall.Domain;

namespace SnapRecall.Application.Features.Questions
{
    public class AddQuestionCommand : IRequest
    {
        public long TopicId { get; set; }
        public required string Text { get; set; }
        public List<Option> Options { get; set; }
    }
}
