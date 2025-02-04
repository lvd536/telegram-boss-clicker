using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ClickerBot.Game.Clicker.Commands;

public class HelpCommand
{
    public async Task HelpCmd(ITelegramBotClient botClient, Message msg)
    {
        var helpMessage = 
            "<b>🎮 Boss Clicker - Справочник игрока</b>\n\n" +
            
            "<b>📝 Основные команды:</b>\n" +
            "/start - Начать игру\n" +
            "/profile - Просмотр профиля\n" +
            "/click - Атаковать босса\n" +
            "/shop - Открыть магазин предметов\n" +
            "/setname [имя] - Изменить имя персонажа\n" +
            "/help - Показать это сообщение\n\n" +
            
            "<b>⚔️ Игровой процесс:</b>\n" +
            "• Кликайте на боссов, чтобы наносить им урон\n" +
            "• За победу над боссом получайте награды:\n" +
            "  💰 Монеты\n" +
            "  💎 Алмазы\n" +
            "  🌟 Опыт\n\n" +
            
            "<b>🏆 Прогресс:</b>\n" +
            "• Получайте опыт за убийство боссов\n" +
            "• Повышайте уровень для встречи с более сильными боссами\n" +
            "• Собирайте предметы для усиления персонажа\n\n" +
            
            "<b>🛍️ Магазин:</b>\n" +
            "• 🔨 Молот Гнева Босса (350💰) - Урон +5\n" +
            "• 🗡️ Кинжал Скрытого Тапа (700💰) - Урон +10\n" +
            "• 🛡️ Доспехи Непробиваемого Терпения (1400💰) - Урон +15\n" +
            "• ⛓️ Плащ Невидимого Фарма (4000💰) - Урон +20\n\n" +
            
            "<b>💡 Советы:</b>\n" +
            "• Регулярно проверяйте магазин для покупки новых предметов\n" +
            "• Следите за здоровьем босса и своим прогрессом\n" +
            "• Копите ресурсы для покупки мощных предметов\n\n" +
            
            "<b>🔗 Полезные ссылки:</b>\n" +
            "• 📱 <a href='https://t.me/lvdshka'>Telegram разработчика</a>\n" +
            "• ⭐️ <a href='https://github.com/lvd536/telegram-boss-clicker'>GitHub проекта</a>\n\n" +
            
            "<i>Удачной охоты на боссов! 🎯</i>";
        
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🔫Клик!", "OnClick"),
                InlineKeyboardButton.WithCallbackData("📝Установить имя", "ChangeName")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("🛒Магазин", "Shop"), 
                InlineKeyboardButton.WithCallbackData("🦸Профиль", "Profile")
            }
            ,
            new []
            {
                InlineKeyboardButton.WithCallbackData("🤑Ежедневная награда", "Daily"),
                InlineKeyboardButton.WithCallbackData("📶Топ", "Top")
            }
        });

        await botClient.SendMessage(
            chatId: msg.Chat.Id,
            text: helpMessage,
            parseMode: ParseMode.Html,
            replyMarkup: keyboard
        );
    }
}