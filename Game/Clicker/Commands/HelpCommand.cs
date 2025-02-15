﻿using Telegram.Bot;
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
            "/items - Просмотр инвентаря\n" +
            "/upgrade [id] - Улучшить предмет\n" +
            "/craft [монеты] [алмазы] - Создать предмет\n" +
            "/daily - Получить ежедневную награду\n" +
            "/top - Посмотреть таблицу лидеров\n" +
            "/help - Показать это сообщение\n\n" +
            
            "<b>⚔️ Игровой процесс:</b>\n" +
            "• Кликайте на боссов, чтобы наносить им урон\n" +
            "• За победу над боссом получайте награды:\n" +
            "  💰 Монеты\n" +
            "  💎 Алмазы\n" +
            "  🌟 Опыт\n\n" +
            
            "<b>🏰 Система этажей:</b>\n" +
            "• Каждый этаж имеет 10 комнат с боссами\n" +
            "• Босс последней комнаты этажа сильнее обычных\n" +
            "• С повышением этажа растут награды и сложность\n\n" +
            
            "<b>📊 Система рангов:</b>\n" +
            "• Новичок (1-4 уровень)\n" +
            "• Любитель (5-9 уровень)\n" +
            "• Профессионал (10-14 уровень)\n" +
            "• Мастер (15-19 уровень)\n" +
            "• Гранд-мастер (20-24 уровень)\n" +
            "• Чемпион (25-29 уровень)\n" +
            "• Легенда (30-34 уровень)\n" +
            "• Бессмертный (35-39 уровень)\n" +
            "• Мифический (40-49 уровень)\n" +
            "• Божественный (50+ уровень)\n\n" +
            
            "<b>🛍️ Магазин:</b>\n" +
            "• 🔨 Молот Гнева Босса (350💰) - Урон +5\n" +
            "• 🗡️ Кинжал Скрытого Тапа (700💰) - Урон +10\n" +
            "• 🛡️ Доспехи Непробиваемого Терпения (1400💰) - Урон +15\n" +
            "• ⛓️ Доспехи Потерянной Легенды (4000💰) - Урон +20\n\n" +
            
            "<b>⚒️ Крафт предметов:</b>\n" +
            "• Используйте команду /craft [монеты] [алмазы]\n" +
            "• Минимальные значения: 100💰 и 10💎\n" +
            "• Чем больше ресурсов, тем выше шанс успеха\n" +
            "• При неудаче ресурсы сгорают\n\n" +
            
            "<b>🔄 Ежедневные награды:</b>\n" +
            "• Используйте /daily каждые 24 часа\n" +
            "• Награда зависит от уровня и прогресса\n" +
            "• Гарантированно получаете монеты и алмазы\n\n" +
            
            "<b>🏆 Таблица лидеров:</b>\n" +
            "• По уровню\n" +
            "• По монетам\n" +
            "• По алмазам\n" +
            "• По убийствам боссов\n" +
            "• По урону\n" +
            "• По ELO рейтингу\n\n" +
            
            "<b>🔗 Полезные ссылки:</b>\n" +
            "• 📱 <a href='https://t.me/lvdshka'>Telegram разработчика</a>\n" +
            "• ⭐️ <a href='https://github.com/lvd536/telegram-boss-clicker'>GitHub проекта</a>\n\n" +
            
            "<i>Удачной охоты на боссов! 🎯</i>";
        
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

        await botClient.SendMessage(
            chatId: msg.Chat.Id,
            text: helpMessage,
            parseMode: ParseMode.Html,
            replyMarkup: keyboard
        );
    }
}