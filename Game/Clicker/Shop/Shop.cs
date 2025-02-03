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
                InlineKeyboardButton.WithCallbackData("Молот Гнева Босса", "Shop1"),
                InlineKeyboardButton.WithCallbackData("Кинжал Скрытого Тапа", "Shop2")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Доспехи Непробиваемого Терпения", "Shop3"),
                InlineKeyboardButton.WithCallbackData("Плащ Невидимого Фарма", "Shop4")
            }
        });
        var message = $"Магазин предметов: ";
        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html, replyMarkup: keyboard);
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
                        Items sItems1 = new Items
                        {
                            Name = "Молот Гнева Босса",
                            Price = 350,
                            Count = 1,
                            Damage = 5,
                            Level = 1
                        };
                        if (userData.Items.Any(i => i.Name == sItems1.Name))
                        {
                            await botClient.SendMessage(msg.Chat.Id, $"У вас уже есть {sItems1.Name}!", ParseMode.Html);
                        }
                        else
                        {
                            if (userData.Money >= sItems1.Price)
                            {
                                userData.Items.Add(sItems1);
                                userData.Damage += sItems1.Damage;
                                userData.Money -= sItems1.Price;
                                await db.SaveChangesAsync();
                                await botClient.EditMessageText(msg.Chat.Id, msg.Id,$"Вы успешно купили {sItems1.Name} за {sItems1.Price}💰", ParseMode.Html);
                            }
                            else
                                await botClient.SendMessage(msg.Chat.Id,
                                    $"Вам нехватает средств для покупки предмета. Необходимо: {sItems1.Price}💰",ParseMode.Html);
                        }

                        break;
                    case 2:
                        Items sItems2 = new Items
                        {
                            Name = "Кинжал Скрытого Тапа",
                            Price = 700,
                            Count = 1,
                            Damage = 10,
                            Level = 1
                        };
                        if (userData.Items.Any(i => i.Name == sItems2.Name))
                        {
                            await botClient.SendMessage(msg.Chat.Id, $"У вас уже есть {sItems2.Name}!", ParseMode.Html);
                        }
                        else
                        {
                            if (userData.Money >= sItems2.Price)
                            {
                                userData.Items.Add(sItems2);
                                userData.Damage += sItems2.Damage;
                                userData.Money -= sItems2.Price;
                                await db.SaveChangesAsync();
                                await botClient.EditMessageText(msg.Chat.Id, msg.Id,$"Вы успешно купили {sItems2.Name} за {sItems2.Price}💰", ParseMode.Html);
                            }
                            else await botClient.SendMessage(msg.Chat.Id,$"Вам нехватает средств для покупки предмета. Необходимо: {sItems2.Price}💰", ParseMode.Html);
                        }
                        break;
                    case 3:
                        Items sItems3 = new Items
                        {
                            Name = "Доспехи Непробиваемого Терпения",
                            Price = 1400,
                            Count = 1,
                            Damage = 15,
                            Level = 1
                        };
                        if (userData.Items.Any(i => i.Name == sItems3.Name))
                        {
                            await botClient.SendMessage(msg.Chat.Id, $"У вас уже есть {sItems3.Name}!", ParseMode.Html);
                        }
                        else
                        {
                            if (userData.Money >= sItems3.Price)
                            {
                                userData.Items.Add(sItems3);
                                userData.Damage += sItems3.Damage;
                                userData.Money -= sItems3.Price;
                                await db.SaveChangesAsync();
                                await botClient.EditMessageText(msg.Chat.Id, msg.Id,$"Вы успешно купили {sItems3.Name} за {sItems3.Price}💰", ParseMode.Html);
                            }
                            else await botClient.SendMessage(msg.Chat.Id,$"Вам нехватает средств для покупки предмета. Необходимо: {sItems3.Price}💰", ParseMode.Html);
                        }
                        break;
                    case 4:
                        Items sItems4 = new Items
                        {
                            Name = "Доспехи Непробиваемого Терпения",
                            Price = 4000,
                            Count = 1,
                            Damage = 20,
                            Level = 1
                        };
                        if (userData.Items.Any(i => i.Name == sItems4.Name))
                        {
                            await botClient.SendMessage(msg.Chat.Id, $"У вас уже есть {sItems4.Name}!", ParseMode.Html);
                        }
                        else
                        {
                            if (userData.Money >= sItems4.Price)
                            {
                                userData.Items.Add(sItems4);
                                userData.Damage += sItems4.Damage;
                                userData.Money -= sItems4.Price;
                                await db.SaveChangesAsync();
                                await botClient.EditMessageText(msg.Chat.Id, msg.Id,$"Вы успешно купили {sItems4.Name} за {sItems4.Price}💰", ParseMode.Html);
                            }
                            else await botClient.SendMessage(msg.Chat.Id,$"Вам нехватает средств для покупки предмета. Необходимо: {sItems4.Price}💰", ParseMode.Html);
                        }
                        break;
                }
            }
        }
    }
}