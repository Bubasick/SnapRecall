using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Application.Commands.Interfaces;
using SnapRecall.Application.Features.Users.UpdateUserCommand;
using SnapRecall.Domain;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Commands;

public class BeginTopicBotCommand(ISnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : ICommand
{
    public static string Name = BotCommands.BeginTopicBotCommand;
    public static string Description = "begin topic";

    public static BotCommand Command => new(Name, Description);

    public async Task OnCommand(string previousCommand, string command, Message message, CancellationToken cancellationToken)
    {
        int.TryParse(command.TrimStart(Name.ToCharArray()), CultureInfo.InvariantCulture, out var id);

        var quizzes = dbContext.Quizzes
            .Include(x => x.Options)
            .Include(x => x.Topic)
            .Where(x => x.TopicId == id && x.Topic.IsCreationFinished)
            .ToList();

        var questions = dbContext.Questions
            .Include(x => x.Attachments)
            .Include(x => x.Topic)
            .Where(x => x.TopicId == id && x.Topic.IsCreationFinished)
            .ToList();

        if (quizzes.Count == 0 && questions.Count == 0)
        {
            await client.SendMessageAsync(message.Chat.Id, "topic not found", cancellationToken: cancellationToken);
            return;
        }

        await mediator.Send(
            new UpdateUserCommand()
            {
                Id = message.From.Id,
                Name = message.From.FirstName,
                LastExecutedCommand = Name,
                Tag = message.From.Username,
            },
            cancellationToken);

        foreach (var quiz in quizzes)
        {
            var options = quiz.Options.Select(x => new InputPollOption() { Text = x.Text }).ToList();
            var correctOptionId = quiz.Options.FindIndex(0, x => x.IsCorrect);
            await client.SendPollAsync(message.Chat.Id, quiz.Text, options, type: "quiz", correctOptionId: correctOptionId, cancellationToken: cancellationToken);
        }

        foreach (var question in questions)
        {
            if (question.Attachments.Any())
            {
                var photoAttachments = question.Attachments.Where(x => x.Type == AttachmentType.Photo).Select( x=> new InputMediaPhoto(x.FileId)).ToList();
                var videoAttachments = question.Attachments.Where(x => x.Type == AttachmentType.Video).Select(x => new InputMediaVideo(x.FileId)).ToList();
                var audioAttachments = question.Attachments.Where(x => x.Type == AttachmentType.Audio).Select(x => new InputMediaAudio(x.FileId)).ToList();
                var documentAttachments = question.Attachments.Where(x => x.Type == AttachmentType.File).Select(x => new InputMediaDocument(x.FileId)).ToList();

                if (photoAttachments.Count > 0)
                {
                    if (photoAttachments.Count > 1)
                    { 
                        await client.SendMediaGroupAsync(new SendMediaGroupArgs(message.Chat.Id, photoAttachments), cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await client.SendPhotoAsync(new SendPhotoArgs(message.Chat.Id, photoAttachments[0].Media), cancellationToken: cancellationToken);
                    }
                }

                if (videoAttachments.Count > 0)
                {
                    if (videoAttachments.Count > 1)
                    {
                        await client.SendMediaGroupAsync(new SendMediaGroupArgs(message.Chat.Id, videoAttachments), cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await client.SendVideoAsync(new SendVideoArgs(message.Chat.Id, videoAttachments[0].Media), cancellationToken: cancellationToken);
                    }
                }

                if (audioAttachments.Count > 0)
                {
                    if (audioAttachments.Count > 1)
                    {
                        await client.SendMediaGroupAsync(new SendMediaGroupArgs(message.Chat.Id, audioAttachments), cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await client.SendAudioAsync(new SendAudioArgs(message.Chat.Id, audioAttachments[0].Media), cancellationToken: cancellationToken);
                    }
                }

                if (documentAttachments.Count > 0)
                {
                    if (documentAttachments.Count > 1)
                    {
                        await client.SendMediaGroupAsync(new SendMediaGroupArgs(message.Chat.Id, documentAttachments), cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await client.SendDocumentAsync(new SendDocumentArgs(message.Chat.Id, documentAttachments[0].Media), cancellationToken: cancellationToken);
                    }
                }
            }
            else
            {

                await client.SendMessageAsync(message.Chat.Id, @$"{question.Text}  
||{question.Answer}||", parseMode: "MarkdownV2", cancellationToken: cancellationToken);
            }
        }
    }
}