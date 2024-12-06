using MediatR;
using SnapRecall.Domain;


namespace SnapRecall.Application.Features.Users.CreateUserCommand;

public class CreateUserCommandHandler(ISnapRecallDbContext dbContext) : IRequestHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken token)
    {
        var userExists = dbContext.Users.Any(x => x.Id == request.Id);

        if (userExists)
        {
            return;
        }

        var user = new User()
        {
            Id = request.Id,
            Name = request.Name,
            LastExecutedCommand = request.LastExecutedCommand,
            Tag = request.Tag
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(token);
    }
}