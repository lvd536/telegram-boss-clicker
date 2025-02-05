namespace ClickerBot.Game.Clicker.Handlers;

public class RankSystemHandler
{
    private static Random _rnd = new Random();
    public static string GetRank(int level)
    {
        if (level >= 50) return "Божественный";
        if (level >= 40) return "Мифический";
        if (level >= 35) return "Бессмертный";
        if (level >= 30) return "Легенда";
        if (level >= 25) return "Чемпион";
        if (level >= 20) return "Гранд-мастер";
        if (level >= 15) return "Мастер";
        if (level >= 10) return "Профессионал";
        if (level >= 5) return "Любитель";
        return "Новичок";
    }
    
    public static int GetElo(int damage)
    {
        return Math.Max(0, damage + _rnd.Next(39));
    }
}