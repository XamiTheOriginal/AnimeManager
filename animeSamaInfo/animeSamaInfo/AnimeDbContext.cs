using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace animeSamaInfo;

// AnimeDbContext.cs

using Microsoft.EntityFrameworkCore;

public class AnimeDbContext : DbContext
{
    // C'est votre table dans la base de données
    public DbSet<Animes> Animes { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // La BDD sera un fichier nommé AnimeCatalog.db
        optionsBuilder.UseSqlite("Data Source=AnimeCatalog.db"); 
    }

    // Extrait de AnimeDbContext.cs, dans la méthode OnModelCreating

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ... votre configuration de AnimeStatus est ici ...

        // Configuration de la colonne AnimeTypes
        modelBuilder.Entity<Animes>()
            .Property(a => a.AnimeTypes)
            .HasConversion(
                // Conversion de List<AnimeType> vers string (Sérialiseur)
                v => System.Text.Json.JsonSerializer.Serialize(v, new System.Text.Json.JsonSerializerOptions()),
                // Conversion de string vers List<AnimeType> (Désérialiseur)
                v => System.Text.Json.JsonSerializer.Deserialize<List<AnimeType>>(v, new System.Text.Json.JsonSerializerOptions()) ?? new List<AnimeType>()
            )
            // NOUVEAU : Ajout du ValueComparer pour les collections
            .Metadata.SetValueComparer(new ValueComparer<List<AnimeType>>(
                (c1, c2) => c1!.SequenceEqual(c2!), // Comment comparer l'égalité
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Comment calculer le HashCode
                c => c.ToList() // Comment copier la valeur
            ));
    
        // ... le reste de votre OnModelCreating ...
    }
}