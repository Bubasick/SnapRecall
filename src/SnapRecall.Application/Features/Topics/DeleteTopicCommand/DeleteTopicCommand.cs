using MediatR;

namespace SnapRecall.Application.Features.Topics.DeleteTopicCommand;

public class DeleteTopicCommand: IRequest
{
    public long TopicId { get; set; }
    public long UserId { get; set; }
}