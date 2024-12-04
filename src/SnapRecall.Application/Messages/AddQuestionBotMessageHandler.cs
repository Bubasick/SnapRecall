using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Application.Messages.Interfaces;
using SnapRecall.Domain;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Messages
{
    public class AddQuestionBotMessageHandler(SnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : IMessage
    {
        public async Task OnMessage(Message message, CancellationToken cancellationToken)
        {
            var unfinishedTopic = dbContext.Topics
                .Include(x => x.Author)
                .FirstOrDefault(x => x.Author.Id == message.From.Id && !x.IsCreationFinished);

            var question = new Question();
            question.TopicId = unfinishedTopic.Id;

            if (message.Text != null)
            {
                var texts = message.Text.Split(new string[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.RemoveEmptyEntries);
                if (texts.Length > 2)
                {
                    await client.SendMessageAsync(message.Chat.Id,
                        "Incorrect question format. It should be 2 paragraphs separated by empty line",
                        cancellationToken: cancellationToken);
                }

                question.Text = texts[0];
                if (texts.Length == 2)
                {
                    question.Answer = texts[1];
                }

                dbContext.Questions.Add(question);
                await dbContext.SaveChangesAsync(cancellationToken);

                await RenderCreateQuizButton(message, "Question added. Use /done to finish topic creation",
                    cancellationToken);
            }
            else
            {
                if (message.MediaGroupId != null)
                {
                    var existingAttachment =
                        dbContext.Attachments.FirstOrDefault(x => x.MediaGroupId == message.MediaGroupId);

                    if (existingAttachment != null)
                    {
                        question = dbContext.Questions.FirstOrDefault(x => x.Id == existingAttachment.QuestionId);
                    }
                    else
                    {
                        if (message.Caption != null)
                        {
                            question.Text = message.Caption;
                        }


                        await RenderCreateQuizButton(message, "Question added. Use /done to finish topic creation",
                            cancellationToken);
                    }
                }
                else
                {
                    await RenderCreateQuizButton(message, "Question added. Use /done to finish topic creation",
                        cancellationToken);
                }

                dbContext.Questions.Add(question);
                await dbContext.SaveChangesAsync(cancellationToken);

                var attachment = ResolveAttachment(message, question.Id);
                if (attachment == null)
                {
                    await client.SendMessageAsync(message.Chat.Id,
                        "Could not resolve the attachment",
                        cancellationToken: cancellationToken);
                    return;
                }
                dbContext.Attachments.Add(attachment);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private Attachment? ResolveAttachment(Message message, long questionId)
        {
            Attachment? attachment = null;
            if (message.Photo != null)
            {
                attachment = new Attachment
                {
                    FileId = message.Photo.Last().FileId,
                    MediaGroupId = message.MediaGroupId,
                    QuestionId = questionId,
                    Type = AttachmentType.Photo,
                };
            }
            else if (message.Audio != null)
            {
                attachment = new Attachment
                {
                    FileId = message.Audio.FileId,
                    MediaGroupId = message.MediaGroupId,
                    QuestionId = questionId,
                    Type = AttachmentType.Audio,
                };
            }
            else if (message.Video != null)
            {
                attachment = new Attachment
                {
                    FileId = message.Video.FileId,
                    MediaGroupId = message.MediaGroupId,
                    QuestionId = questionId,
                    Type = AttachmentType.Video,
                };
            }
            else if (message.Document != null)
            {
                attachment = new Attachment
                {
                    FileId = message.Document.FileId,
                    MediaGroupId = message.MediaGroupId,
                    QuestionId = questionId,
                    Type = AttachmentType.File,
                };
            }

            return attachment;
        }

        private async Task RenderCreateQuizButton(Message message, string text = default, CancellationToken cancellationToken = default)
        {
            var keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Create quiz")
                    {
                        RequestPoll = new KeyboardButtonPollType{Type = "quiz"}

                    }
                }
            };

            var keyboardMarkup = new ReplyKeyboardMarkup(keyboard);

            keyboardMarkup.ResizeKeyboard = true;

            await client.SendMessageAsync(message.Chat.Id, text, replyMarkup: keyboardMarkup, cancellationToken: cancellationToken);
        }
    }
}
