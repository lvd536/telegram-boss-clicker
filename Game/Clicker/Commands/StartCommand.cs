using Telegram.Bot;
using Telegram.Bot.Types;
using ClickerBot.Database;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ClickerBot.Game.Start;

public class StartCommand
{
    public async Task StartCmd(ITelegramBotClient botClient, Message msg)
    {
        await DBMethods.CreatePlayerAsync(msg);

        var keyboard = new InlineKeyboardMarkup()
            .AddButton("⚔️Клик!", "OnClick")
            .AddNewRow()
            .AddButton(InlineKeyboardButton.WithUrl("📱 Telegram разработчика", "https://t.me/lvdshka"))
            .AddButton(InlineKeyboardButton.WithUrl("⭐️ GitHub source проекта", "https://github.com/lvd536/telegram-boss-clicker"));
        
        Console.WriteLine("StartCommand Sending");
        await botClient.SendMessage(msg.Chat.Id, "👋Добро пожаловать в Boss Clicker. Для более подробной информации напишите /help", parseMode: ParseMode.Html,
            replyMarkup: keyboard);
    }
    
    public async Task BackCmd(ITelegramBotClient botClient, Message msg)
    {
        var keyboard = new InlineKeyboardMarkup()
            .AddButton("🔫Клик!", "OnClick")
            .AddButton("📝Установить имя", "ChangeName")
            .AddNewRow()
            .AddButton("🛒Магазин", "Shop")
            .AddButton("🦸Профиль", "Profile")
            .AddNewRow()
            .AddButton("🤑Ежедневная награда", "Daily")
            .AddButton("📶Топ", "Top")
            .AddNewRow()
            .AddButton("📋Список предметов", "ItemsList")
            .AddNewRow()
            .AddButton("⏫Улучшить предмет", "ItemsUpgrade")
            .AddButton("⚒️Скрафтить предмет", "ItemsCraft");
        
        try
        {
            await botClient.EditMessageText(msg.Chat.Id, msg.Id,
                "👋Добро пожаловать в Boss Clicker. Для более подробной информации напишите /help",
                parseMode: ParseMode.Html,
                replyMarkup: keyboard);
        }
        catch (Exception ex)
        {
            await botClient.SendMessage(msg.Chat.Id,
                "👋Добро пожаловать в Boss Clicker. Для более подробной информации напишите /help",
                parseMode: ParseMode.Html,
                replyMarkup: keyboard);
        }
    }
}