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
            new[]{
            InlineKeyboardButton.WithCallbackData("Молот Гнева Босса","Shop1"),
            InlineKeyboardButton.WithCallbackData("Кинжал Скрытого Тапа","Shop2")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Доспехи Непробиваемого Терпения","Shop3"),
                InlineKeyboardButton.WithCallbackData("Плащ Невидимого Фарма","Shop4")
            }
        });
        var message = $"Магазин предметов: ";
        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
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
                        Items item = new Items
                        {
                            Name = "Молот Гнева Босса",
                            Price = 350,
                            Count = 1,
                            Damage = 5,
                            Level = 1
                        };
                        if (userData.Items.Any(i => i.Name == item.Name))
                        {
                            await botClient.SendMessage(msg.Chat.Id, $"У вас уже есть {item.Name}!", ParseMode.Html);
                        }
                        else
                        {
                            if (userData.Money >= item.Price)
                            {
                                userData.Items.Add(item);
                                userData.Damage += item.Damage;
                                userData.Money -= item.Price;
                                await db.SaveChangesAsync();
                            }
                            else await botClient.SendMessage(msg.Chat.Id,
                                $"Вам нехватает средств для покупки предмета. Необходимо: {item.Price}💰", ParseMode.Html);
                        }
                        break;
                    case 2:
                        
                        break;
                    case 3:
                        
                        break;
                    case 4:
                        
                        break;
                }
            }
        }
    }
}