namespace ClickerBot.Game.Clicker.Items;
using Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Handlers;

public class ItemsCommand
{
    public async Task ItemList(ITelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Users.Include(u => u.Items).FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            string message = $"⚔️Ваш инвентарь: \n";
            foreach (var item in userData.Items)
            {
                message += $"<blockquote> {item.Id}. Название: {item.Name}, Уровень: {item.Name}, Урон: {item.Damage} </blockquote>";
            }
            await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html);
        }
    }
}