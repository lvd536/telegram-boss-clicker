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
                    await botClient.SendMessage(msg.Chat.Id,
                        $"Вы успешно улучшили предмет {itemData.Name} до {itemData.Level} уровня за {itemData.Price} монет!",
                        ParseMode.Html);
                }
                else
                    await botClient.SendMessage(msg.Chat.Id,
                        $"У вас недостаточно средств для улучшения предмета {itemData.Name}. Необохдимо {itemData.Price * 2} монет",
                        ParseMode.Html);
            }
            else await DBMethods.CreatePlayerAsync(msg);
        }
    }

    public async Task ItemCraft(ITelegramBotClient botClient, Message msg, int money, int crystals)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Users.Include(i => i.Items).FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            if (userData is null)
            {
                await DBMethods.CreatePlayerAsync(msg);
                await botClient.SendMessage(msg.Chat.Id, "Вы не зарегистрированы, регистрируем вас. Пожалуйста, попробуйте снова.");
                return;
            }

            if (userData.Money < money || userData.Cashiers < crystals)
            {
                await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно монет или кристаллов для крафта.",
                    ParseMode.Html);
                return;
            }

            double failureChance = 1 - (money / 2000.0) - (crystals / 300.0);
            failureChance = Math.Clamp(failureChance, 0.1, 0.99);

            Random random = new Random();
            double randomNumber = random.NextDouble();

            if (randomNumber <= failureChance)
            {
                userData.Money -= money;
                userData.Cashiers -= crystals;

                await db.SaveChangesAsync();

                await botClient.SendMessage(msg.Chat.Id,
                    $"К сожалению, крафт не удался. Вы потратили {money} монет и {crystals} кристаллов. Шанс: {100 - failureChance * 100}%",
                    ParseMode.Html);
                return;
            }

            var items = new List<(Items item, double chance)>
            {
                (sItems1, 0.5 + (money / 1000.0) + (crystals / 100.0)),
                (sItems2, 0.3 + (money / 1500.0) + (crystals / 150.0)),
                (sItems3, 0.15 + (money / 2000.0) + (crystals / 200.0)),
                (sItems4, 0.05 + (money / 2500.0) + (crystals / 250.0))
            };

            double totalChance = items.Sum(i => i.chance);
            var normalizedItems = items.Select(i => (i.item, i.chance / totalChance)).ToList();

            double cumulativeChance = 0;
            Items craftedItem = null;
            foreach (var (item, chance) in normalizedItems)
            {
                cumulativeChance += chance;
                if (random.NextDouble() <= cumulativeChance)
                {
                    craftedItem = item;
                    break;
                }
            }

            if (craftedItem is null)
            {
                await botClient.SendMessage(msg.Chat.Id, "Не удалось создать предмет. Попробуйте снова.",
                    ParseMode.Html);
                return;
            }

            userData.Money -= money;
            userData.Cashiers -= crystals;

            var newCraftedItem = new Items
            {
                Name = craftedItem.Name,
                Price = craftedItem.Price,
                Count = 1,
                Damage = craftedItem.Damage,
                Level = craftedItem.Level
            };
            userData.Items.Add(newCraftedItem);
            await db.SaveChangesAsync();

            await botClient.SendMessage(msg.Chat.Id, $"Вы успешно скрафтили предмет: {craftedItem.Name} c шансом: {100 - failureChance * 100}%!",
                ParseMode.Html);
        }
    }
}