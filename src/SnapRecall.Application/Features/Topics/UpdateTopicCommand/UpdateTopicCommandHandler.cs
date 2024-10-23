using MediatR;
using SnapRecall.Domain;
using SnapRecall.Infrastructure.Data;

namespace SnapRecall.Application.Features.Topics.UpdateTopicCommand;

public class UpdateTopicCommandHandler(SnapRecallDbContext dbContext) : IRequestHandler<UpdateTopicCommand>
{
    public async Task Handle(UpdateTopicCommand request, CancellationToken token)
    {
        var topic = dbContext.Topics.FirstOrDefault(x => x.Id == request.TopicId);

        if (topic == null)
        {
            return;
        }

        topic.Name = request.Name;
        topic.Description = request.Description;
        topic.IsCreationFinished = request.IsCreationFinished;
        dbContext.Topics.Update(topic);
        await dbContext.SaveChangesAsync(token);
    }
}