using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Domain;


namespace SnapRecall.Application.Features.Users.UpdateUserCommand;

public class UpdateUserCommandHandler(ISnapRecallDbContext dbContext) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken token)
    {
        var user = dbContext.Users.FirstOrDefault(x => x.Id == request.Id);

        if (user is null)
        {
            return;
        }

        user.Tag = request.Tag;
        user.Name = request.Name;
        user.LastExecutedCommand = request.LastExecutedCommand;

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(token);
    }
}