namespace ClickerBot.Game.Clicker.Profile;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Database;
using Microsoft.EntityFrameworkCore;

public static class LevelUp
{
    private static long expCheck;
    public static async Task LevelUpAsync(ITelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = await db.Users.FirstOrDefaultAsync(u => u.ChatId == msg.Chat.Id);

            if (userData is not null)
            {
                if (userData.Level >= 1)
                {
                    expCheck = userData.Level * 100;
                }
                else if (userData.Level >= 5)
                {
                    expCheck = userData.Level * 150;
                }
                else if (userData.Level >= 10)
                {
                    expCheck = userData.Level * 200;
                }
                else if (userData.Level >= 15)
                {
                    expCheck = userData.Level * 250;
                }
                else if (userData.Level >= 20)
                {
                    expCheck = userData.Level * 300;
                }
                else
                {
                    expCheck = userData.Level * 350;
                }

                if (userData.Experience >= expCheck)
                {
                    userData.Level++;
                    userData.Experience = 0;
                    await db.SaveChangesAsync();
                    await botClient.SendMessage(msg.Chat.Id, $"Поздравляем! Вы повысили свой уровень до {userData.Level}", ParseMode.Html);
                }
            }
            else
            {
                Console.WriteLine("Level Up Failed. User not found!");
                await DBMethods.CreatePlayerAsync(msg);
            }
        }
    }
}