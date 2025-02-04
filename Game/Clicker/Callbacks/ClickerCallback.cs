using ClickerBot.Database;
using ClickerBot.Game.Clicker.Profile;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ClickerBot.Game.Clicker.Callbacks;

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
                        await Boss.Boss.BossMain(msg);
                        userData = await db.Users.FirstOrDefaultAsync(u => u.ChatId == msg.Chat.Id);
                    }
                    
                    userData.Boss.Health -= Convert.ToInt32(userData.Damage);

                    if (command)
                    {
                        var message =
                            ($"👿Текущий босс: {userData.Boss.Name}.\n" +
                             $"🌟Уровень босса: {userData.Boss.Level}\n" +
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

                        userData.Money += bossMoney;
                        userData.Cashiers += bossCashiers;
                        userData.Experience += (long)bossExp;
                        userData.KilledBosses++;

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
                            $"🎉 Вы победили {bossName}!\nПолучено: {bossMoney}💰, {userData.Cashiers}💎  и {bossExp}🌟"
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
                        await Boss.Boss.BossMain(msg);
                    }
                    else
                    {
                        await db.SaveChangesAsync();
                        var message =
                            ($"👿Вы нанесли боссу {userData.Boss.Name} {userData.Damage} урона.\n" +
                              $"🌟Уровень босса: {userData.Boss.Level}\n" +
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