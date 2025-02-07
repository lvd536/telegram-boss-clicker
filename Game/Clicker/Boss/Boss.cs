using Telegram.Bot.Types;
using ClickerBot.Database;
using Microsoft.EntityFrameworkCore;

namespace ClickerBot.Game.Clicker.Boss;

public static class Boss
{
    private static Random rnd = new Random();

    public static async Task BossMain(Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            try
            {
                var plly = await db.Users.FirstOrDefaultAsync(u => u.ChatId == msg.Chat.Id);
                if (plly is not null && (plly.Boss == null || plly.Boss.Health <= 0 || string.IsNullOrEmpty(plly.Boss.Name)))
                {
                    string[] _bossNames = { 
                        "Marshal", 
                        "Hellsing", 
                        "Harry", 
                        "Hunter", 
                        "Killa", 
                        "Vanguard", 
                        "Shadowfang", 
                        "Ironclad", 
                        "Frostbite", 
                        "Blackbeard", 
                        "Warlord", 
                        "Dreadnought", 
                        "Spectre", 
                        "Ragnarok", 
                        "Nightshade", 
                        "Titan", 
                        "Blaze", 
                        "Phantom", 
                        "Overlord", 
                        "Apex"
                    };
                    string bossName = plly.BossRoom >= 10 ? _bossNames[rnd.Next(_bossNames.Length)] + $" Босс {plly.BossFloor} Этажа" : _bossNames[rnd.Next(_bossNames.Length)];
                    int bossLvl = plly.BossRoom >= 10 ? Math.Max(1, rnd.Next(plly.Level * plly.BossFloor, plly.Level * plly.BossFloor + 10)) : Math.Max(1, rnd.Next(plly.Level, plly.Level + 3));
                    int bossHealth = Math.Max(10, rnd.Next(bossLvl * 5, bossLvl * 25));
                    double bossExp = Math.Max(5, rnd.Next((bossLvl * bossHealth) / rnd.Next(2, 5), (bossLvl * bossHealth) * 2));
                    long bossMoney = Math.Max(10, rnd.Next(bossLvl * bossHealth, (bossLvl * bossHealth) * rnd.Next(2, 10)));
                    long bossCashiers = rnd.Next(0, 5);

                    plly.Boss = new Database.Boss
                    {
                        Name = bossName,
                        Level = bossLvl,
                        Health = bossHealth,
                        Experience = bossExp,
                        Money = bossMoney,
                        Cashiers = bossCashiers
                    };
                
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Boss created: {bossName}, HP: {bossHealth} | ChatID: {msg.Chat.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BossMain Error: {ex.Message}");
            }
        }
    }
}