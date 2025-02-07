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
                        if (index > 10) break;
                        if (String.IsNullOrEmpty(u.Username)) u.Username = "None";
                        else message += $"<blockquote>{index}. {u.Username}\n Level: {u.Level}\n Rank: {u.Rank}</blockquote>\n";
                        index++;
                    }
                    break;
                case 2: // Money
                    index = 1;
                    message = $"🔝Топ игроков по монетам: ";
                    users.Sort((a, b) => Convert.ToInt32(b.Money - a.Money));
                    foreach (var u in users)
                    {
                        if (index > 10) break;
                        if (String.IsNullOrEmpty(u.Username)) u.Username = "None";
                        else message += $"<blockquote>{index}. {u.Username}\n Money: {u.Money}\n Rank: {u.Rank}</blockquote>\n";
                        index++;
                    }
                    break;
                case 3: // Cashiers
                    index = 1;
                    message = $"🔝Топ игроков по алмазам: ";
                    users.Sort((a, b) => Convert.ToInt32(b.Cashiers - a.Cashiers));
                    foreach (var u in users)
                    {
                        if (index > 10) break;
                        if (String.IsNullOrEmpty(u.Username)) u.Username = "None";
                        else message += $"<blockquote>{index}. {u.Username}\n Diamonds: {u.Cashiers}\n Rank: {u.Rank}</blockquote>\n";
                        index++;
                    }
                    break;
                case 4: // Kills
                    index = 1;
                    message = $"🔝Топ игроков по убийствам: ";
                    users.Sort((a, b) => b.KilledBosses - a.KilledBosses);
                    foreach (var u in users)
                    {
                        if (index > 10) break;
                        if (String.IsNullOrEmpty(u.Username)) u.Username = "None";
                        else message += $"<blockquote>{index}. {u.Username}\n Kills: {u.KilledBosses}\n Rank: {u.Rank}</blockquote>\n";
                        index++;
                    }
                    break;
                case 5: // Damage
                    index = 1;
                    message = $"🔝Топ игроков по урону: ";
                    users.Sort((a, b) => Convert.ToInt32(b.Damage - a.Damage));
                    foreach (var u in users)
                    {
                        if (index > 10) break;
                        if (String.IsNullOrEmpty(u.Username)) u.Username = "None";
                        else message += $"<blockquote>{index}. {u.Username}\n Damage: {u.Damage}\n Rank: {u.Rank}</blockquote>\n";
                        index++;
                    }
                    break;
                case 6: // ELO
                    index = 1;
                    message = $"🔝Топ игроков по ELO: ";
                    users.Sort((a, b) => Convert.ToInt32(b.Elo - a.Elo));
                    foreach (var u in users)
                    {
                        if (index > 10) break;
                        if (String.IsNullOrEmpty(u.Username)) u.Username = "None";
                        else message += $"<blockquote>{index}. {u.Username}\n Elo: {u.Elo}\n Rank: {u.Rank}</blockquote>\n";
                        index++;
                    }
                    break;
            }
            var keyboard = new InlineKeyboardMarkup()
                .AddButton("🌟Топ по уровню", "Top")
                .AddNewRow()
                .AddButton("💰Топ по монетам", "TopByMoney")
                .AddNewRow()
                .AddButton("💎Топ по алмазам", "TopByCashiers")
                .AddNewRow()
                .AddButton("👿Топ по убитым боссам", "TopByKills")
                .AddNewRow()
                .AddButton("🩸Топ по урону", "TopByDamage")
                .AddNewRow()
                .AddButton("📊Топ по ELO", "TopByElo")
                .AddNewRow()
                .AddButton("👾Главное меню", "BackToMain");
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