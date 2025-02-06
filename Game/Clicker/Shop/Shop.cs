using ClickerBot.Game.Clicker.Items;

namespace ClickerBot.Game.Clicker.Shop;

using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Database;
using Microsoft.EntityFrameworkCore;

public class Shop
{
    public async Task ShopCmd(ITelegramBotClient botClient, Message msg)
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🔨Молот Гнева Босса", "Shop1"),
                InlineKeyboardButton.WithCallbackData("🗡️Кинжал Скрытого Тапа", "Shop2")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🛡️Доспехи Непробиваемого Терпения", "Shop3"),
                InlineKeyboardButton.WithCallbackData("⛓️‍Доспехи Потерянной Легенды", "Shop4")
            }
        });
        var message = $"🛍️Магазин предметов: ";
        try
        {
            await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html, replyMarkup: keyboard);
        }
        catch (Exception ex)
        {
            await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
        }
    }

    public async Task ShopCallback(ITelegramBotClient botClient, Message msg, int itemId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Users
                .Include(u => u.Items)
                .FirstOrDefault(p => p.ChatId == msg.Chat.Id);
            if (userData is not null)
            {
                switch (itemId)
                {
                    case 1:
                        /*Items ItemsCommand.sItems1 = new Items
                        {
                            Name = "Молот Гнева Босса",
                            Price = 350,
                            Count = 1,
                            Damage = 5,
                            Level = 1
                        };*/
                        if (userData.Items.Any(i => i.Name == ItemsCommand.sItems1.Name))
                        {
                            await botClient.SendMessage(msg.Chat.Id, $"У вас уже есть {ItemsCommand.sItems1.Name}!", ParseMode.Html);
                        }
                        else
                        {
                            if (userData.Money >= ItemsCommand.sItems1.Price)
                            {
                                userData.Items.Add(ItemsCommand.sItems1);
                                userData.Damage += ItemsCommand.sItems1.Damage;
                                userData.Money -= ItemsCommand.sItems1.Price;
                                await db.SaveChangesAsync();
                                try
                                {
                                    await botClient.EditMessageText(msg.Chat.Id, msg.Id,
                                        $"Вы успешно купили {ItemsCommand.sItems1.Name} за {ItemsCommand.sItems1.Price}💰", ParseMode.Html);
                                }
                                catch (Exception ex)
                                {
                                    await botClient.SendMessage(msg.Chat.Id,
                                        $"Вы успешно купили {ItemsCommand.sItems1.Name} за {ItemsCommand.sItems1.Price}💰", ParseMode.Html);
                                }
                                
                            }
                            else
                                await botClient.SendMessage(msg.Chat.Id,
                                    $"Вам нехватает средств для покупки предмета. Необходимо: {ItemsCommand.sItems1.Price}💰",ParseMode.Html);
                        }

                        break;
                    case 2:
                        if (userData.Items.Any(i => i.Name == ItemsCommand.sItems2.Name))
                        {
                            await botClient.SendMessage(msg.Chat.Id, $"У вас уже есть {ItemsCommand.sItems2.Name}!", ParseMode.Html);
                        }
                        else
                        {
                            if (userData.Money >= ItemsCommand.sItems2.Price)
                            {
                                userData.Items.Add(ItemsCommand.sItems2);
                                userData.Damage += ItemsCommand.sItems2.Damage;
                                userData.Money -= ItemsCommand.sItems2.Price;
                                await db.SaveChangesAsync();
                                try
                                {
                                    await botClient.EditMessageText(msg.Chat.Id, msg.Id,
                                        $"Вы успешно купили {ItemsCommand.sItems2.Name} за {ItemsCommand.sItems2.Price}💰", ParseMode.Html);
                                }
                                catch (Exception ex)
                                {
                                    await botClient.SendMessage(msg.Chat.Id,
                                        $"Вы успешно купили {ItemsCommand.sItems2.Name} за {ItemsCommand.sItems2.Price}💰", ParseMode.Html);
                                }
                            }
                            else await botClient.SendMessage(msg.Chat.Id,$"Вам нехватает средств для покупки предмета. Необходимо: {ItemsCommand.sItems2.Price}💰", ParseMode.Html);
                        }
                        break;
                    case 3:
                        if (userData.Items.Any(i => i.Name == ItemsCommand.sItems3.Name))
                        {
                            await botClient.SendMessage(msg.Chat.Id, $"У вас уже есть {ItemsCommand.sItems3.Name}!", ParseMode.Html);
                        }
                        else
                        {
                            if (userData.Money >= ItemsCommand.sItems3.Price)
                            {
                                userData.Items.Add(ItemsCommand.sItems3);
                                userData.Damage += ItemsCommand.sItems3.Damage;
                                userData.Money -= ItemsCommand.sItems3.Price;
                                await db.SaveChangesAsync();
                                try
                                {
                                    await botClient.EditMessageText(msg.Chat.Id, msg.Id,
                                        $"Вы успешно купили {ItemsCommand.sItems3.Name} за {ItemsCommand.sItems3.Price}💰", ParseMode.Html);
                                }
                                catch (Exception ex)
                                {
                                    await botClient.SendMessage(msg.Chat.Id,
                                        $"Вы успешно купили {ItemsCommand.sItems3.Name} за {ItemsCommand.sItems3.Price}💰", ParseMode.Html);
                                }
                            }
                            else await botClient.SendMessage(msg.Chat.Id,$"Вам нехватает средств для покупки предмета. Необходимо: {ItemsCommand.sItems3.Price}💰", ParseMode.Html);
                        }
                        break;
                    case 4:
                        if (userData.Items.Any(i => i.Name == ItemsCommand.sItems4.Name))
                        {
                            await botClient.SendMessage(msg.Chat.Id, $"У вас уже есть {ItemsCommand.sItems4.Name}!", ParseMode.Html);
                        }
                        else
                        {
                            if (userData.Money >= ItemsCommand.sItems4.Price)
                            {
                                userData.Items.Add(ItemsCommand.sItems4);
                                userData.Damage += ItemsCommand.sItems4.Damage;
                                userData.Money -= ItemsCommand.sItems4.Price;
                                await db.SaveChangesAsync();
                                try
                                {
                                    await botClient.EditMessageText(msg.Chat.Id, msg.Id,
                                        $"Вы успешно купили {ItemsCommand.sItems4.Name} за {ItemsCommand.sItems4.Price}💰", ParseMode.Html);
                                }
                                catch (Exception ex)
                                {
                                    await botClient.SendMessage(msg.Chat.Id,
                                        $"Вы успешно купили {ItemsCommand.sItems4.Name} за {ItemsCommand.sItems4.Price}💰", ParseMode.Html);
                                }
                            }
                            else await botClient.SendMessage(msg.Chat.Id,$"Вам нехватает средств для покупки предмета. Необходимо: {ItemsCommand.sItems4.Price}💰", ParseMode.Html);
                        }
                        break;
                }
            }
        }
    }
}