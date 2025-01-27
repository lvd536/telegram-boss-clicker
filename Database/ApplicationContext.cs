namespace ClickerBot.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class Player
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int Level { get; set; }
    public int Elo { get; set; }
    public string Rank { get; set; }
    public long Experience { get; set; }
    public long Money { get; set; }
    public long Cashiers { get; set; }
    public long Damage { get; set; }
    public List<Items> Items { get; set; } = new List<Items>();

}

public class Items
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Count { get; set; }
    public int Damage { get; set; }
    public short Level { get; set; }
}

public class ApplicationContext : DbContext
{
    public DbSet<Player> Users => Set<Player>();
    public ApplicationContext()
    {
        Database.Migrate();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=database.db");
    }
}

public class SampleContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseSqlite("Data Source=database.db");
        
        return new ApplicationContext();
    }
}