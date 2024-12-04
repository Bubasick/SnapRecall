using MediatR;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;

namespace SnapRecall.Application.Commands.Interfaces
{
    public static class SimpleBotCommandFactory
    {
        public static ICommand GetCommandHandler(SnapRecallDbContext dbContext, ITelegramBotClient client, IMediator mediator, string commandName)
        {
            ICommand handler;
            if (commandName == BotCommands.ConfirmTopicCreationBotCommand)
            {
                handler = new ConfirmTopicCreationBotCommand(dbContext, client, mediator);
            }
            else if (commandName == BotCommands.StartBotCommand)
            {
                handler = new StartBotCommand(dbContext, client, mediator);
            }
            else if (commandName == BotCommands.ViewTopicsBotCommand)
            {
                handler = new ViewTopicsBotCommand(dbContext, client, mediator);
            }
            else if (commandName == BotCommands.NewTopicBotCommand)
            {
                handler = new NewTopicBotCommand(dbContext, client, mediator);
            }
            else if (commandName.StartsWith(BotCommands.BeginTopicBotCommand))
            {
                handler = new BeginTopicBotCommand(dbContext, client, mediator);
            }
            else if (commandName.StartsWith(BotCommands.DeleteTopicBotCommand))
            {
                handler = new DeleteTopicBotCommand(dbContext, client, mediator);
            }
            else
            {
                handler = new NonExistentCommand(dbContext, client, mediator);
            }

            return handler;
        }
    }
}
