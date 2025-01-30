using ClickerBot.Database;
using ClickerBot.Game.Clicker.Profile;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ClickerBot.Game.Clicker.Callbacks;

public class ClickerCallback
{
    public async Task ClickCallback(ITelegramBotClient botClient, Message msg)
    {
        await using (ApplicationContext db = new ApplicationContext())
        {
            try
            {
                var _userData = await db.Users.FirstOrDefaultAsync(u => u.ChatId == msg.Chat.Id);

                if (_userData is not null)
                {
                    if (_userData.Boss == null || _userData.Boss.Health <= 0 || string.IsNullOrEmpty(_userData.Boss.Name))
                    {
                        await Boss.Boss.BossMain(msg);
                        _userData = await db.Users.FirstOrDefaultAsync(u => u.ChatId == msg.Chat.Id);
                    }
                    
                    _userData.Boss.Health -= Convert.ToInt32(_userData.Damage);

                    if (_userData.Boss.Health <= 0)
                    {
                        var bossExp = _userData.Boss.Experience;
                        var bossMoney = _userData.Boss.Money;
                        var bossCashiers = _userData.Boss.Cashiers;
                        var bossName = _userData.Boss.Name;

                        _userData.Money += bossMoney;
                        _userData.Cashiers += bossCashiers;
                        _userData.Experience += (long)bossExp;

                        _userData.Boss = new Database.Boss
                        {
                            Name = string.Empty,
                            Level = 0,
                            Health = 0,
                            Experience = 0,
                            Money = 0,
                            Cashiers = 0
                        };

                        await db.SaveChangesAsync();
                        await LevelUp.LevelUpAsync(botClient, msg);
                        await botClient.SendMessage(msg.Chat.Id,
                            $"🎉 Вы победили {bossName}!\nПолучено: {bossMoney}💰, {_userData.Cashiers} Алмазов и {bossExp} XP" +
                            $"\nТекущая статистика:" +
                            $"\nУровень: {_userData.Level}" +
                            $"\nОпыт: {_userData.Experience}" +
                            $"\nМонет: {_userData.Money}" +
                            $"\nАлмазов: {_userData.Cashiers}");
                    }
                    else
                    {
                        await db.SaveChangesAsync();
                        await botClient.SendMessage(msg.Chat.Id,
                            $"Вы нанесли боссу {_userData.Boss.Name} {_userData.Damage} урона.\nОсталось {_userData.Boss.Health} ХП", ParseMode.Html);
                    }
                }
                else
                {
                    await botClient.SendMessage(msg.Chat.Id,"Похоже вы еще не зарегистрированы в нашей базе. Сейчас исправим!", ParseMode.Html);
                    await DBMethods.CreatePlayerAsync(msg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClickCallback Error: {ex.Message}");
            }
        }
    }
}