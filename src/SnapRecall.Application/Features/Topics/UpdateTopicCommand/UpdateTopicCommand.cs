using MediatR;

namespace SnapRecall.Application.Features.Topics.UpdateTopicCommand;

public class UpdateTopicCommand : IRequest
{
    public long TopicId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsCreationFinished { get; set; }
}