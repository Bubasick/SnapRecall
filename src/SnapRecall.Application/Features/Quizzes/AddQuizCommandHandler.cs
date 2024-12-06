using MediatR;
using SnapRecall.Application.Features.Questions;
using SnapRecall.Domain;


namespace SnapRecall.Application.Features.Topics.CreateTopicCommand;

public class AddQuizCommandHandler(ISnapRecallDbContext dbContext) : IRequestHandler<AddQuizCommand>
{
    public async Task Handle(AddQuizCommand request, CancellationToken token)
    {
        var question = new Quiz()
        {
            Text = request.Text,
            TopicId = request.TopicId,
            Options = request.Options,
        };
        dbContext.Quizzes.Add(question);
        await dbContext.SaveChangesAsync(token);
    }
}