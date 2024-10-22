using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.GettingUpdates;

var botToken = "7744751662:AAE1wcsbF_RGEln5MpMd582oxiKgVtqwxvc";

// You need a TelegramBotClient instance if you want access to the Bot API methods.
var client = new TelegramBotClient(botToken);
var me = client.GetMe();
Console.WriteLine("My name is {0}.", me.FirstName);

var updates = client.GetUpdates();
while (true)
{
    if (updates.Any())
    {
        foreach (var update in updates)
        {
            long chatId = update.Message.Chat.Id; // Target chat Id
            client.SendMessage(chatId, "Hello World!"); // Send a message
        }
        var offset = updates.Last().UpdateId + 1;
        updates = client.GetUpdates(offset);
    }
    else
    {
        updates = client.GetUpdates();
    }
}