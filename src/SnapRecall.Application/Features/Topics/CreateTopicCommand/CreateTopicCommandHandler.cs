using MediatR;
using SnapRecall.Domain;
using SnapRecall.Infrastructure.Data;

namespace SnapRecall.Application.Features.Topics.CreateTopicCommand;

public class CreateTopicCommandHandler(SnapRecallDbContext dbContext) : IRequestHandler<CreateTopicCommand>
{
    public async Task Handle(CreateTopicCommand request, CancellationToken token)
    {
        dbContext.Topics.Add(new Topic()
        {
            AuthorId = request.AuthorId,
        });
        await dbContext.SaveChangesAsync(token);
    }
}