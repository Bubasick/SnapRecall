using MediatR;
using SnapRecall.Domain;


namespace SnapRecall.Application.Features.Topics.CreateTopicCommand;

public class CreateTopicCommandHandler(ISnapRecallDbContext dbContext) : IRequestHandler<CreateTopicCommand>
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