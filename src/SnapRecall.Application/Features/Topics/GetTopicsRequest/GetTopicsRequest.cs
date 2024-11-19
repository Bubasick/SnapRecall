using MediatR;

namespace SnapRecall.Domain.Features.Topics.GetTopicsRequest;

public class GetTopicsRequest :IRequest<List<Topic>>
{
    public long UserId { get; set; }
}