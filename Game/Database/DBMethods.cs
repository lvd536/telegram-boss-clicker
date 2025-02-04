using Microsoft.VisualBasic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ClickerBot.Database;
using Telegram.Bot;
public class DBMethods
{
    public static async Task CreatePlayerAsync(Message message)
    {
        await using (ApplicationContext db = new ApplicationContext())
        {
            if (db.Users.Any(u => u.ChatId == message.Chat.Id))
            {
                Console.WriteLine("[Debug] Player already exists.");
            }
            else
            { 
                Console.WriteLine($"[Debug] Creating new player from chat: {message.Chat.Id}");
                var boss = new Boss 
                { 
                    Name = string.Empty,
                    Level = 1,
                    Health = 100,
                    Experience = 0,
                    Money = 0,
                    Cashiers = 0
                };
            
                var p = new Player 
                {
                    ChatId = message.Chat.Id,
                    Username = "",
                    Level = 1,
                    Cashiers = 10,
                    Experience = 0,
                    Money = 100,
                    Rank = "Новичок",
                    Damage = 1,
                    Elo = 0,
                    Boss = boss,
                }; 
                await db.Users.AddAsync(p);
               await db.SaveChangesAsync();
               var users = db.Users.ToList();
               foreach (var u in users)
               {
                   Console.WriteLine(u.ChatId);
               }
            }
        }
    }
}