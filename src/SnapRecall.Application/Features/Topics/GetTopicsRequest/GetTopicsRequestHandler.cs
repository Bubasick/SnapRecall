using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Application;


namespace SnapRecall.Domain.Features.Topics.GetTopicsRequest;

public class GetTopicsRequestHandler(ISnapRecallDbContext dbContext) : IRequestHandler<GetTopicsRequest, List<Topic>>
{
    public async Task<List<Topic>> Handle(GetTopicsRequest request, CancellationToken token)
    {
        var topics = dbContext.Topics.Where(x => x.AuthorId == request.UserId && x.IsCreationFinished);
        return await topics.ToListAsync(token);
    }
}