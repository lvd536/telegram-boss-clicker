namespace ClickerBot.Game.Clicker.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Database;

public class DailyRewardCommand
{
    private Random _rnd = new Random();
    public async Task DailyRewardCmd(TelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Users.FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            if (userData is not null)
            {
                var userLevel = userData.Level;
                var userMoney = userData.Money;
                var userKills = userData.KilledBosses;
                var userMoneyReward = CalculateDailyMoneyReward(userLevel, userMoney, userKills);
                var userCashierReward = CalculateDailyCashierReward();

                DateTime getrewardtime = userData.DailyGetTime;
                DateTime usertime = DateTime.Now;
                TimeSpan calc = TimeSpan.Parse("-1.00:00:00");
                if (getrewardtime - usertime <= calc)
                {
                    userData.Money += userMoneyReward;
                    userData.Cashiers += userCashierReward;
                    userData.DailyGetTime = usertime;
                    await db.SaveChangesAsync();
                    await botClient.SendMessage(msg.Chat.Id,
                        $"🚀 Вы получили {userMoneyReward}💰 и {userCashierReward}💎!", ParseMode.Html);
                }
                else
                {
                    await botClient.SendMessage(msg.Chat.Id,
                        $"⚠️ Подождите 24 часа с момента получения последней ежедневной награды чтобы получить ее снова",
                        ParseMode.Html);
                }
            }
            else await DBMethods.CreatePlayerAsync(msg);
        }
    }
    
    private int CalculateDailyMoneyReward(int level, long money, int kills)
    {
        int dailyMoney = (int)(money * kills) / (level * 2);
        if (dailyMoney < 100) dailyMoney = _rnd.Next(500, 5500);
        return dailyMoney;
    }
    private int CalculateDailyCashierReward()
    {
        return _rnd.Next(3, 49);
    }
}