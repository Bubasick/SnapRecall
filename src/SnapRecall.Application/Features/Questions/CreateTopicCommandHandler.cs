using MediatR;
using SnapRecall.Application.Features.Questions;
using SnapRecall.Domain;
using SnapRecall.Infrastructure.Data;

namespace SnapRecall.Application.Features.Topics.CreateTopicCommand;

public class AddQuestionCommandHandler(SnapRecallDbContext dbContext) : IRequestHandler<AddQuestionCommand>
{
    public async Task Handle(AddQuestionCommand request, CancellationToken token)
    {
        var question = new Question()
        {
            Text = request.Text,
        };
        dbContext.Questions.Add(question);
        question.Options = request.Options;
        await dbContext.SaveChangesAsync(token);
    }
}