namespace ClickerBot.Game.Clicker.Profile;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Database;
using Microsoft.EntityFrameworkCore;
using Handlers;

public class Profile
{
    public async Task ProfileCmdAsync(ITelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Users
                .Include(i => i.Items)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            if (userData is not null)
            {
                var userName = userData.Username;
                if (String.IsNullOrEmpty(userName)) userName = "Не установлен.";
                var level = userData.Level;
                var exp = userData.Experience;
                var money = userData.Money;
                var cashiers = userData.Cashiers;
                var chatId = userData.ChatId;
                var bossName = userData.Boss.Name;
                var itemsCount = userData.Items.Count;
                var killsCount = userData.KilledBosses;
                var rank = userData.Rank;
                var eloCount = userData.Elo;

                var requiredExp = LevelUp.GetRequiredExp(level);
                var progressBar = LevelUp.GetProgressBar(exp, requiredExp);
                
                var message = ($"Ваш игровой персонаж:\n" +
                           $"🤵Ник: {userName}\n" +
                           $"👑Ранг: {rank}\n" +
                           $"📊Elo: {eloCount}\n" +
                           $"🚀Уровень: {level}\n" +
                           $"🌟Опыт: {userData.Experience}/{requiredExp}\n" +
                           $"📊Прогресс: {progressBar}\n" +
                           $"💰Монет: {money}\n" +
                           $"💎Алмазов: {cashiers}\n" +
                           $"☠️Имя текущего босса: {bossName}\n" +
                           $"⚔️Количество предметов: {itemsCount}\n" +
                           $"👹Кол-во убитых боссов: {killsCount}\n" +
                           $"📚ChatId: {chatId}"
                );
                var keyboard = new InlineKeyboardMarkup()
                    .AddButton("🔫Клик!", "OnClick")
                    .AddNewRow()
                    .AddButton("📝Установить имя", "ChangeName")
                    .AddNewRow()
                    .AddButton("🛒Магазин", "Shop") 
                    .AddNewRow()
                    .AddButton("🤑Ежедневная награда", "Daily")
                    .AddNewRow()
                    .AddButton("📶Топ", "Top") 
                    .AddNewRow()
                    .AddButton("Главное меню", "BackToMain");
                try
                {
                    await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html, replyMarkup: keyboard);
                } catch (Exception ex)
                {
                    await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                }
            }
            else
            {
                await DBMethods.CreatePlayerAsync(msg);
            }
        }
    }
}