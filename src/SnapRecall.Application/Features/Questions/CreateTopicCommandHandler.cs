using MediatR;
using SnapRecall.Application.Features.Questions;
using SnapRecall.Domain;
using SnapRecall.Infrastructure.Data;

namespace SnapRecall.Application.Features.Topics.CreateTopicCommand;

public class AddQuestionCommandHandler(SnapRecallDbContext dbContext) : IRequestHandler<AddQuestionCommand>
{
    public async Task Handle(AddQuestionCommand request, CancellationToken token)
    {
        var question = new Quiz()
        {
            Text = request.Text,
            TopicId = request.TopicId,
            Options = request.Options,
        };
        dbContext.Questions.Add(question);
        await dbContext.SaveChangesAsync(token);
    }
}