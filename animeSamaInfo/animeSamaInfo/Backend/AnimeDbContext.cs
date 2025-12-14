using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace animeSamaInfo.Backend;



using Microsoft.EntityFrameworkCore;

public class AnimeDbContext : DbContext
{
    public DbSet<Animes> Animes { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       
        optionsBuilder.UseSqlite("Data Source=AnimeCatalog.db"); 
    }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
   
        modelBuilder.Entity<Animes>()
            .Property(a => a.AnimeTypes)
            .HasConversion(
                // Conversion de List<AnimeType> vers string (Sérialiseur)
                v => System.Text.Json.JsonSerializer.Serialize(v, new System.Text.Json.JsonSerializerOptions()),
                // Conversion de string vers List<AnimeType> (Désérialiseur)
                v => System.Text.Json.JsonSerializer.Deserialize<List<AnimeType>>(v, new System.Text.Json.JsonSerializerOptions()) ?? new List<AnimeType>()
            )
            
            .Metadata.SetValueComparer(new ValueComparer<List<AnimeType>>(
                (c1, c2) => c1!.SequenceEqual(c2!), 
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), 
                c => c.ToList()
            ));
        
    }
}