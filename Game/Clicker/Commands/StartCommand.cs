using Telegram.Bot;
using Telegram.Bot.Types;
using ClickerBot.Database;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ClickerBot.Game.Start;

public class StartCommand
{
    public async Task StartCmd(ITelegramBotClient botClient, Message msg)
    {
        await DBMethods.CreatePlayerAsync(msg);

        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Клик!", "OnClick"),
            },
            new[]
            {
                InlineKeyboardButton.WithUrl("📱 Telegram разработчика", "https://t.me/lvdshka"),
                InlineKeyboardButton.WithUrl("⭐️ GitHub source проекта", "https://github.com/lvd536/telegram-boss-clicker"),
            }
        });
        Console.WriteLine("StartCommand Sending");
        await botClient.SendMessage(msg.Chat.Id, "👋Добро пожаловать в Boss Clicker", parseMode: ParseMode.Html,
            replyMarkup: keyboard);
    }
}