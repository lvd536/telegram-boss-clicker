using ClickerBot.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ClickerBot.Game.Clicker.Callbacks;

public class ClickerCallback
{
    public async Task ClickCallback(ITelegramBotClient botClient, Message msg)
    {
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
                await botClient.SendMessage(msg.Chat.Id, "Похоже вы еще не зарегистрированы в нашей базе. Сейчас исправим!", ParseMode.Html);
                await DBMethods.CreatePlayerAsync(msg);
            }
        }
    }
}