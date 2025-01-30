namespace ClickerBot.Game.Clicker.Profile;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Database;
using Microsoft.EntityFrameworkCore;

public class Profile
{
    public async Task ProfileCmdAsync(ITelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = await db.Users.FirstOrDefaultAsync(u => u.ChatId == msg.Chat.Id);
            if (userData is not null)
            {
                var userName = userData.Username;
                var level = userData.Level;
                var exp = userData.Experience;
                var money = userData.Money;
                var cashiers = userData.Cashiers;
                var chatId = userData.ChatId;
                var bossName = userData.Boss.Name;

                var message = ($"Ваш игровой персонаж:\n" +
                           $"🤵Ник: {userName}\n" +
                           $"🚀Уровень: {level}\n" +
                           $"🌟Опыт: {exp}\n" +
                           $"💰Монет: {money}\n" +
                           $"💎Алмазов: {cashiers}\n" +
                           $"☠️Имя текущего босса: {bossName}\n" +
                           $"📚ChatId: {chatId}"
                );

                var keyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🔫Клик!", "OnClick"),
                        InlineKeyboardButton.WithCallbackData("📝Установить имя", "ChangeName")
                    }
                });
                
                await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
            }
            else
            {
                await DBMethods.CreatePlayerAsync(msg);
            }
        }
    }
}