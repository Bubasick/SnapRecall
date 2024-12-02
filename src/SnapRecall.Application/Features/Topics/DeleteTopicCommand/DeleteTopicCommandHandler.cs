using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Infrastructure.Data;

namespace SnapRecall.Application.Features.Topics.DeleteTopicCommand;

public class DeleteTopicCommandHandler(SnapRecallDbContext dbContext) : IRequestHandler<DeleteTopicCommand>
{
    public async Task Handle(DeleteTopicCommand request, CancellationToken token)
    {
        var topic = await dbContext.Topics.FirstOrDefaultAsync(x => x.Id == request.TopicId && x.AuthorId == request.UserId, token);
        dbContext.Topics.Remove(topic);
        await dbContext.SaveChangesAsync(token);
    }
}