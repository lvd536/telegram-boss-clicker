namespace ClickerBot.Game.Clicker.Callbacks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Database;
using Microsoft.EntityFrameworkCore;

public class ChangeNameCallback
{
    public async Task ChangeNameAsync(TelegramBotClient botClient, Message msg, string newName)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = await db.Users.FirstOrDefaultAsync(u => u.ChatId == msg.Chat.Id);
            if (userData is not null)
            {
                if (newName.Length > 15)
                {
                    await botClient.SendMessage(msg.Chat.Id, "Максимально допустимая длина имени - 15 символов.", ParseMode.Html);
                }
                else
                {
                    userData.Username = newName;
                    await db.SaveChangesAsync();
                    var keyboard = new InlineKeyboardMarkup(new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🦸Профиль", "Profile")
                    });
                    await botClient.SendMessage(msg.Chat.Id, $"Ваше имя изменено на {userData.Username}",
                        ParseMode.Html, replyMarkup: keyboard);
                }
            }
            else
            {
                Console.WriteLine("ChangeName: Could not find user");
                await DBMethods.CreatePlayerAsync(msg);
            }
        }
    }
}