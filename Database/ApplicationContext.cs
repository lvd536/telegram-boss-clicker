namespace ClickerBot.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class Player
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public string Username { get; set; }
    public int Level { get; set; }
    public int Elo { get; set; }
    public string Rank { get; set; }
    public long Experience { get; set; }
    public long Money { get; set; }
    public long Cashiers { get; set; }
    public long Damage { get; set; }
    
    public int KilledBosses { get; set; }
    public Boss Boss { get; set; } = new Boss
    {
        Name = "",
        Level = 1,
        Health = 0,
        Experience = 0,
        Money = 0,
        Cashiers = 0
    };
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
    public int PlayerId { get; set; }
    public Player Player { get; set; }
}

public class Boss
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int Health { get; set; }
    public double Experience { get; set; }
    public long Money { get; set; }
    public long Cashiers { get; set; }
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>()
            .OwnsOne(p => p.Boss);

        modelBuilder.Entity<Player>()
            .HasMany(p => p.Items)
            .WithOne(i => i.Player)
            .HasForeignKey(i => i.PlayerId);
        
        modelBuilder.Entity<Items>()
            .HasIndex(i => i.PlayerId);
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