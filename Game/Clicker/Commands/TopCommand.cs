namespace ClickerBot.Game.Clicker.Commands;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Database;
using Microsoft.EntityFrameworkCore;
public class TopCommand
{
    public async Task TopCmd(ITelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var users = db.Users.ToList();
            string message = $"Топ игроков по уровню: ";
            int index = 1;
            users.Sort((a, b) => b.Level - a.Level);
            foreach (var u in users)
            {
                if (String.IsNullOrEmpty(u.Username)) message += $"\n{index}. None - {u.Level}";
                else message += $"\n{index}. {u.Username} - {u.Level}";
                index++;
            }

            try
            {
                await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html);
            } catch (Exception ex)
            {
                await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html);
            }
        }
    }
}