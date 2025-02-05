namespace ClickerBot.Game.Clicker.Commands;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Database;
using Microsoft.EntityFrameworkCore;
public class TopCommand
{
    public async Task TopCmd(ITelegramBotClient botClient, Message msg, int type)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var users = db.Users.ToList();
            string message = string.Empty;
            int index = 1;
            switch (type)
            {
                case 1: // Level
                    index = 1;
                    message = $"🔝Топ игроков по уровню: ";
                    users.Sort((a, b) => b.Level - a.Level);
                    foreach (var u in users)
                    {
                        if (index >= 50) break;
                        if (String.IsNullOrEmpty(u.Username)) message += $"\n{index}. None - {u.Level}";
                        else message += $"\n{index}. {u.Username} - {u.Level}";
                        index++;
                    }
                    break;
                case 2: // Money
                    index = 1;
                    message = $"🔝Топ игроков по монетам: ";
                    users.Sort((a, b) => Convert.ToInt32(b.Money - a.Money));
                    foreach (var u in users)
                    {
                        if (index >= 50) break;
                        if (String.IsNullOrEmpty(u.Username)) message += $"\n{index}. None - {u.Money}";
                        else message += $"\n{index}. {u.Username} - {u.Money}";
                        index++;
                    }
                    break;
                case 3: // Cashiers
                    index = 1;
                    message = $"🔝Топ игроков по алмазам: ";
                    users.Sort((a, b) => Convert.ToInt32(b.Cashiers - a.Cashiers));
                    foreach (var u in users)
                    {
                        if (index >= 50) break;
                        if (String.IsNullOrEmpty(u.Username)) message += $"\n{index}. None - {u.Cashiers}";
                        else message += $"\n{index}. {u.Username} - {u.Cashiers}";
                        index++;
                    }
                    break;
                case 4: // Kills
                    index = 1;
                    message = $"🔝Топ игроков по убийствам: ";
                    users.Sort((a, b) => b.KilledBosses - a.KilledBosses);
                    foreach (var u in users)
                    {
                        if (index >= 50) break;
                        if (String.IsNullOrEmpty(u.Username)) message += $"\n{index}. None - {u.KilledBosses}";
                        else message += $"\n{index}. {u.Username} - {u.KilledBosses}";
                        index++;
                    }
                    break;
                case 5: // Damage
                    index = 1;
                    message = $"🔝Топ игроков по урону: ";
                    users.Sort((a, b) => Convert.ToInt32(b.Damage - a.Damage));
                    foreach (var u in users)
                    {
                        if (index >= 50) break;
                        if (String.IsNullOrEmpty(u.Username)) message += $"\n{index}. None - {u.Damage}";
                        else message += $"\n{index}. {u.Username} - {u.Damage}";
                        index++;
                    }
                    break;
                case 6: // ELO
                    index = 1;
                    message = $"🔝Топ игроков по ELO: ";
                    users.Sort((a, b) => Convert.ToInt32(b.Elo - a.Elo));
                    foreach (var u in users)
                    {
                        if (index >= 50) break;
                        if (String.IsNullOrEmpty(u.Username)) message += $"\n{index}. None - {u.Elo}";
                        else message += $"\n{index}. {u.Username} - {u.Elo}";
                        index++;
                    }
                    break;
            }
            
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("🌟Топ по уровню", "Top"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("💰Топ по монетам", "TopByMoney")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("💎Топ по алмазам", "TopByCashiers"), 
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("👿Топ по убитым боссам", "TopByKills")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("🩸Топ по урону", "TopByDamage"), 
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("📊Топ по ELO", "TopByElo"), 
                }
            });
            
            try
            {
                await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html, replyMarkup: keyboard);
            } catch (Exception ex)
            {
                await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
            }
        }
    }
}