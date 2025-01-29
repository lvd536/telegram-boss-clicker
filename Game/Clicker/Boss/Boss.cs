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
                    Console.WriteLine("Create Boss");
                    string[] _bossNames = { "Marshal", "Hellsing", "Harry", "Hunter", "Killa" };
                    string _bossName = _bossNames[rnd.Next(_bossNames.Length)];
                    int _bossLvl = Math.Max(1, rnd.Next(plly.Level, plly.Level + 3));
                    int _bossHealth = Math.Max(10, rnd.Next(_bossLvl * 5, _bossLvl * 25));
                    double _bossExp = Math.Max(5, rnd.Next((_bossLvl * _bossHealth) / rnd.Next(2, 5), (_bossLvl * _bossHealth) * 2));
                    long _bossMoney = Math.Max(10, rnd.Next(_bossLvl * _bossHealth, (_bossLvl * _bossHealth) * rnd.Next(2, 10)));
                    long _bossCashiers = rnd.Next(0, 5);

                    plly.Boss = new Database.Boss
                    {
                        Name = _bossName,
                        Level = _bossLvl,
                        Health = _bossHealth,
                        Experience = _bossExp,
                        Money = _bossMoney,
                        Cashiers = _bossCashiers
                    };
                
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Boss created: {_bossName}, HP: {_bossHealth}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BossMain Error: {ex.Message}");
            }
        }
    }
}