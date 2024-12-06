using MediatR;
using SnapRecall.Application.Messages.Interfaces;

using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Messages;

public class NonExistentBotMessageHandler(ISnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : IMessage
{
    public async Task OnMessage(Message message, CancellationToken cancellationToken)
    {
        await client.SendMessageAsync(message.Chat.Id, "This operation is not supported", cancellationToken: cancellationToken);
    }
}