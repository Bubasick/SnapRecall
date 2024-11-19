using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.BotCommands
{
    public interface ICommand
    {
        public Task OnCommand(string previousCommand, string command, Message message, CancellationToken cancellationToken);
    }
}
