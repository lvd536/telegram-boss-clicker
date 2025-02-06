namespace ClickerBot.Game.Clicker.Items;
using Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Handlers;

public class ItemsCommand
{
    public static readonly Items sItems1 = new Items
    {
        Name = "Молот Гнева Босса",
        Price = 350,
        Count = 1,
        Damage = 5,
        Level = 1
    };
    public static readonly Items sItems2 = new Items
    {
        Name = "Кинжал Скрытого Тапа",
        Price = 700,
        Count = 1,
        Damage = 10,
        Level = 1
    };
    public static readonly Items sItems3 = new Items
    {
        Name = "Доспехи Непробиваемого Терпения",
        Price = 1400,
        Count = 1,
        Damage = 15,
        Level = 1
    };
    public static readonly Items sItems4 = new Items
    {
        Name = "Доспехи Потерянной Легенды",
        Price = 4000,
        Count = 1,
        Damage = 20,
        Level = 1
    };
    public async Task ItemList(ITelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Users.Include(u => u.Items).FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            if (userData is not null)
            {
                string message = $"⚔️Ваш инвентарь: \n";
                foreach (var item in userData.Items)
                {
                    message +=
                        $"<blockquote> ID: {item.Id}\n Название: {item.Name}\n Уровень: {item.Level}\n Урон: {item.Damage}\n Цена: {item.Price}\n Стоимость улучшения: {item.Price * 2} </blockquote>";
                }

                await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html);
            }
            else await DBMethods.CreatePlayerAsync(msg);
        }
    }

    public async Task ItemUpgrade(ITelegramBotClient botClient, Message msg, int id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Users.Include(u => u.Items).FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var itemData = userData?.Items.FirstOrDefault(i => i.Id == id);
            if (itemData is not null && userData is not null)
            {
                if (userData.Money >= itemData.Price * 2)
                {
                    Console.WriteLine("Start damage" + itemData.Damage);
                    Console.WriteLine("Start money: " + userData.Money);
                    userData.Money -= itemData.Price * 2;
                    itemData.Level += 1;
                    itemData.Damage = itemData.Damage * 2;
                    itemData.Price = itemData.Price * 2;
                    await db.SaveChangesAsync();
                    Console.WriteLine("After " + itemData.Damage);
                    Console.WriteLine("After money: " + userData.Money);
                    await botClient.SendMessage(msg.Chat.Id, $"Вы успешно улучшили предмет {itemData.Name} до {itemData.Level} уровня за {itemData.Price} монет!", ParseMode.Html);
                }
                else
                    await botClient.SendMessage(msg.Chat.Id,
                        $"У вас недостаточно средств для улучшения предмета {itemData.Name}. Необохдимо {itemData.Price * 2} монет", ParseMode.Html);
            }
            else await DBMethods.CreatePlayerAsync(msg);
            
        }
    }
}