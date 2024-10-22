using MediatR;

namespace SnapRecall.Application.Topics.CreateTopicCommand;

public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand>
{
    public CreateTopicCommandHandler()
    {
    }

    public Task Handle(CreateTopicCommand request, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}