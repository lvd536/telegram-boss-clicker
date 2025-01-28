using Microsoft.VisualBasic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ClickerBot.Database;
using Telegram.Bot;
public class DBMethods
{
    public static async Task CreatePlayerAsync(Message message)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var Player = db.Users.FirstOrDefault(u => u.ChatId == message.Chat.Id);
            if (Player is not null)
            {
                Console.WriteLine("[Debug] Player already exists.");
            }
            else
            { 
               var p = new Player {ChatId = message.Chat.Id, Username = message.From?.Username ?? message.MessageId.ToString(), Level = 1, Cashiers = 10, Experience = 0, Money = 100, Rank = "Новичок", Damage = 1, Elo = 0};
               await db.Users.AddAsync(p);
               await db.SaveChangesAsync();
            }
        }
    }
}