using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnapRecall.Application.BotCommands;
using SnapRecall.Application.Features.Questions;
using SnapRecall.Application.Features.Topics.UpdateTopicCommand;
using SnapRecall.Domain;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;

namespace SnapRecall.Application.SnapRecallBotHandlers
{
    public partial class SnapRecallBot : SimpleTelegramBotBase
    {
        protected override async Task OnMessageAsync(Message message, CancellationToken cancellationToken)
        {
            // Ignore user 777000 (Telegram)
            if (message.From?.Id == TelegramConstants.TelegramId)
            {
                return;
            }

            var myUsername = await Client.GetMeAsync(cancellationToken);
            SetCommandExtractor(myUsername.Username!);
            if (CommandExtractor!.HasCommand(message))
            {
                var (commandName, args) = CommandExtractor.ExtractCommand(message);
                await OnCommandAsync(message, commandName, args, cancellationToken);
                return;
            }


            var hasText = !string.IsNullOrEmpty(message.Text); // True if message has text;
#if DEBUG
            this.logger.LogInformation("New message from chat id: {ChatId}", message!.Chat.Id);
            this.logger.LogInformation(
                "Message: {MessageContent}",
                hasText ? message.Text : "No text"
            );
#endif

            var user = dbContext.Users.FirstOrDefault(x => x.Id == message.From.Id);
            if (user is null)
            {
                return;
            }

            if (user.LastExecutedCommand == Commands.NewTopicBotCommand)
            {
                var unfinishedTopic = dbContext.Topics
                    .Include(x => x.Author)
                    .FirstOrDefault(x => x.Author.Id == message.From.Id && !x.IsCreationFinished);

                if (unfinishedTopic is null)
                {
                    throw new Exception();
                }
                if (hasText)
                {
                    if (!string.IsNullOrEmpty(unfinishedTopic.Name))
                    {
                        Client.SendMessageAsync(message.Chat.Id, "Topic already named. Please add your first question using the button below");
                    }
                    await mediator.Send(new UpdateTopicCommand()
                    {
                        TopicId = unfinishedTopic.Id,
                        Name = message.Text,
                    });

                    await RenderCreateQuestionButton(message, "Good. Now send me a poll with your first question.", cancellationToken);

                }

                else if (message.Poll != null && message.Poll.CorrectOptionId.HasValue)
                {
                    var optionsToAdd = new List<Option>();
                    var options = message.Poll.Options.ToList();
                    for (var i = 0; i < options.Count(); i++)
                    {
                        optionsToAdd.Add(new Option()
                        {
                            Text = options[i].Text,
                            IsCorrect = message.Poll.CorrectOptionId.Value == i,
                        });
                    }

                    await mediator.Send(new AddQuestionCommand()
                    {
                        TopicId = unfinishedTopic.Id,
                        Text = message.Poll.Question,
                        Options = optionsToAdd,

                    }, cancellationToken);
                    await RenderCreateQuestionButton(message, "Question added. Use /done to finish topic creation");
                }
            }

            await base.OnMessageAsync(message, cancellationToken);
        }

        public async Task RenderCreateQuestionButton(Message message, string text = default, CancellationToken cancellationToken = default)
        {
            var keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Create a question")
                    {
                        RequestPoll = new KeyboardButtonPollType{Type = "quiz"}

                    }//column 1 row 1
                }
            };

            var keyboardMarkup = new ReplyKeyboardMarkup(keyboard);

            keyboardMarkup.ResizeKeyboard = true;

            await Client.SendMessageAsync(message.Chat.Id, text, replyMarkup: keyboardMarkup, cancellationToken: cancellationToken);
        }
    }
}
