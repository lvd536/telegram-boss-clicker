namespace ClickerBot.Game.Clicker.Callbacks;
using Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Handlers;

public class ClickerCallback
{
    public async Task ClickCallback(ITelegramBotClient botClient, Message msg, bool command)
    {
        await using (ApplicationContext db = new ApplicationContext())
        {
            try
            {
                var userData = await db.Users.FirstOrDefaultAsync(u => u.ChatId == msg.Chat.Id);

                if (userData is not null)
                {
                    if (userData.Boss == null || userData.Boss.Health <= 0 || string.IsNullOrEmpty(userData.Boss.Name))
                    {
                        await Clicker.Boss.Boss.BossMain(msg);
                        userData = await db.Users.FirstOrDefaultAsync(u => u.ChatId == msg.Chat.Id);
                    }
                    
                    userData.Boss.Health -= Convert.ToInt32(userData.Damage);

                    if (command)
                    {
                        var message =
                            ($"👿Текущий босс: {userData.Boss.Name}.\n" +
                             $"🌟Уровень босса: {userData.Boss.Level}\n" +
                             $"Этаж: {userData.BossFloor}" +
                             $"🩸Осталось: {userData.Boss.Health} ХП"
                            );
                        var keyboard = new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("🔫Клик!", "OnClick"),
                                InlineKeyboardButton.WithCallbackData("🦸‍♂️Профиль", "Profile")
                            }
                        });
                        try
                        {
                            await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html, replyMarkup: keyboard);
                        }
                        catch (Exception ex)
                        {
                            await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);   
                        }
                    }
                    
                    if (userData.Boss.Health <= 0)
                    {
                        var bossExp = userData.Boss.Experience;
                        var bossMoney = userData.Boss.Money;
                        var bossCashiers = userData.Boss.Cashiers;
                        var bossName = userData.Boss.Name;
                        string userRank = userData.Rank;
                        int userElo = 0;
                        if (userData.Level <= 50) userRank = RankSystemHandler.GetRank(userData.Level);
                        else userElo = RankSystemHandler.GetElo((int)userData.Damage);
                        string result = (userData.Level <= 50 ? bossExp : userElo.ToString()) + " " + (userData.Level <= 50 ? "🌟" : "ELO");
                        userData.Money += bossMoney;
                        userData.Cashiers += bossCashiers;
                        userData.Experience += (long)bossExp;
                        userData.Rank = userRank;
                        userData.Elo = userElo;
                        userData.KilledBosses++;
                        if (userData.BossFloor >= 10) userData.BossFloor = 0;
                        else userData.BossFloor++;

                        userData.Boss = new Database.Boss
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
                        var message = (
                            $"🎉 Вы победили {bossName}!\nПолучено: {bossMoney}💰, {userData.Cashiers}💎  и {result}"
                        );

                        var keyboard = new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("🔫Клик!", "OnClick"),
                                InlineKeyboardButton.WithCallbackData("🦸‍♂️Профиль", "Profile")
                            }
                        });
                        
                        try
                        {
                            await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html, replyMarkup: keyboard);
                        }
                        catch (Exception ex)
                        {
                            await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);   
                        }
                        await Clicker.Boss.Boss.BossMain(msg);
                    }
                    else
                    {
                        await db.SaveChangesAsync();
                        var message =
                            ($"👿Вы нанесли боссу {userData.Boss.Name} {userData.Damage} урона.\n" +
                              $"🌟Уровень босса: {userData.Boss.Level}\n" +
                              $"Этаж: {userData.BossFloor}" +
                              $"🩸Осталось: {userData.Boss.Health} ХП"
                            );
                        var keyboard = new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("🔫Клик!", "OnClick"),
                                InlineKeyboardButton.WithCallbackData("🦸‍♂️Профиль", "Profile")
                            }
                        });
                        try
                        {
                            await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                                replyMarkup: keyboard);
                        }
                        catch (Exception ex)
                        {
                            await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html,
                                replyMarkup: keyboard);
                        }
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