using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SerieInfo.Backend;



using Microsoft.EntityFrameworkCore;

public class SerieDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Serie> Serie { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       
        optionsBuilder.UseSqlite("Data Source=SerieCatalog.db"); 
    }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
   
        modelBuilder.Entity<Serie>()
            .Property(a => a.SerieTypes)
            .HasConversion(
                // Conversion de List<SerieType> vers string (Sérialiseur)
                v => System.Text.Json.JsonSerializer.Serialize(v, new System.Text.Json.JsonSerializerOptions()),
                // Conversion de string vers List<SerieType> (Désérialiseur)
                v => System.Text.Json.JsonSerializer.Deserialize<List<SerieType>>(v, new System.Text.Json.JsonSerializerOptions()) ?? new List<SerieType>()
            )
            
            .Metadata.SetValueComparer(new ValueComparer<List<SerieType>>(
                (c1, c2) => c1!.SequenceEqual(c2!), 
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), 
                c => c.ToList()
            ));
        
        modelBuilder.Entity<Serie>()
            .HasIndex(s => new { s.Title, s.Studio })
            .IsUnique();
        
    }
}