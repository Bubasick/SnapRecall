using MediatR;
using SnapRecall.Application.Commands.Interfaces;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Messages.Interfaces
{
    public static class SimpleBotMessageFactory
    {
        public static IMessage GetMessageHandler(
            Message message,
            SnapRecallDbContext dbContext,
            ITelegramBotClient client,
            IMediator mediator,
            TelegramSettings settings,
            HttpClient httpClient)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Id == message.From.Id);
            var unfinishedTopic = dbContext.Topics
                .FirstOrDefault(x => x.AuthorId == message.From.Id && !x.IsCreationFinished);

            IMessage handler;
            if (user.LastExecutedCommand == BotCommands.NewTopicBotCommand && unfinishedTopic?.Name == null)
            {
                handler = new AddTopicNameBotMessageHandler(dbContext, client, mediator);
            }
            else if (user.LastExecutedCommand == BotCommands.NewTopicBotCommand && message.Poll != null && message.Poll.CorrectOptionId.HasValue)
            {
                handler = new AddQuizBotMessageHandler(dbContext, client, mediator);
            }
            else if (user.LastExecutedCommand == BotCommands.NewTopicBotCommand && 
                     (!string.IsNullOrEmpty(message.Text) || message.Photo != null || message.Audio != null || message.Video != null || message.Document != null ))
            {
                handler = new AddQuestionBotMessageHandler(dbContext, client, mediator);
            }
            else
            {
                handler = new NonExistentBotMessageHandler(dbContext, client, mediator);
            }

            return handler;
        }
    }
}
