using Telegram.Bot;
using Telegram.Bot.Types;
using ClickerBot.Database;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ClickerBot.Game.Start;

public class StartCommand
{
    public async Task StartCmd(ITelegramBotClient botClient, UpdateType type, Message msg)
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
                InlineKeyboardButton.WithUrl("⭐️ GitHub source проекта", "https://github.com/lvd536"),
            }
        });
        await botClient.SendMessage(msg.Chat.Id, "Добро пожаловать в Кликер игру!", parseMode: ParseMode.Html, replyMarkup: keyboard);
    }

    public async Task StartCallback(ITelegramBotClient botClient, Update type, Message msg)
    {
        //await botClient.SendMessage(msg.Chat.Id, "test", ParseMode.Html);
        await using (ApplicationContext db = new ApplicationContext())
        {
            var _userData = db.Users.FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            if (_userData is not null)
            {
                _userData.Money++;
                await db.SaveChangesAsync();
                await botClient.SendMessage(msg.Chat.Id, $"Earned +1. Balance: {_userData.Money}", parseMode: ParseMode.Html);
            }
            else
            {
                await DBMethods.CreatePlayerAsync(msg);
            }
        }
    }
}