using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace animeSamaInfo;

public class AnimeManager
{
    // Le DbContext gère la connexion à la base de données
    private readonly AnimeDbContext _context = new AnimeDbContext();

    public AnimeManager()
    {
        // Assure que la base de données est créée et à jour au lancement (applique les migrations)
        // Ceci est essentiel pour que le fichier AnimeCatalog.db existe !
        _context.Database.Migrate(); 
    }

    /// <summary>
    /// Ajoute un nouvel anime à la base de données.
    /// </summary>
    public void AddAnime(Animes anime)
    {
        // L'ID sera géré automatiquement par la BDD (auto-incrément)
        anime.AddDate = DateTime.Now;
        _context.Animes.Add(anime);
        _context.SaveChanges();
    }

    /// <summary>
    /// Supprime un anime en utilisant son ID.
    /// </summary>
    public void DeleteAnime(int id)
    {
        // Recherche l'anime à supprimer (FirstOrDefault crée une requête SQL optimisée)
        Animes? animeToDelete = _context.Animes.FirstOrDefault(a => a.Id == id);
        
        if (animeToDelete != null)
        {
            _context.Animes.Remove(animeToDelete);
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Met à jour le statut (Vu, En cours, Favori) d'un anime.
    /// </summary>
    public void UpdateAnimeStatus(int id, AnimeStatus newStatus)
    {
        Animes? anime = _context.Animes.FirstOrDefault(a => a.Id == id);
        
        if (anime != null)
        {
            anime.Status = newStatus;
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Met à jour la progression d'épisodes et de saisons, et ajuste le statut si l'anime est terminé.
    /// </summary>
    public void UpgradeProgression(int id, int episodeSeen, int seasonSeen)
    {
        Animes? anime = _context.Animes.FirstOrDefault(a => a.Id == id);
        
        if (anime == null) return; // L'anime n'existe pas
        
        anime.EpisodeSeen = episodeSeen;
        anime.SeasonSeen = seasonSeen;
        
        if (anime.EpisodeSeen > anime.EpisodeTotal) anime.EpisodeSeen = anime.EpisodeTotal;
        if (anime.SeasonSeen > anime.NumberSeason) anime.SeasonSeen = anime.NumberSeason;

        
        if (anime.SeasonSeen >= anime.NumberSeason && anime.EpisodeSeen >= anime.EpisodeTotal)
        {
            anime.Status = AnimeStatus.Seen;
        }
        else if (anime.EpisodeSeen > 0 || anime.SeasonSeen > 0)
        {
            if (anime.Status != AnimeStatus.Favorite)
            {
                anime.Status = AnimeStatus.Watching;
            }
        }
        
        _context.SaveChanges();
    }

    /// <summary>
    /// Liste tous les animes, ou seulement ceux avec un statut spécifique.
    /// </summary>
    public List<Animes> ListAnimes(AnimeStatus? status = null)
    {
        IQueryable<Animes> query = _context.Animes;

        if (status.HasValue)
        {
            query = query.Where(a => a.Status == status.Value);
        }
        
        // ToList() exécute la requête SQL et retourne le résultat
        return query.ToList(); 
    }

    /// <summary>
    /// Liste les animes qui contiennent au moins un des types donnés.
    /// </summary>
    public List<Animes> ListByType(List<AnimeType> types)
    {
        // Pour les champs stockés en JSON (AnimeTypes), EF Core ne peut pas filtrer directement en SQL.
        // On doit récupérer tous les animes et filtrer en mémoire (ToList() puis Where).
        // Attention : Pour un très grand catalogue, une solution de recherche spécialisée serait préférable.

        return _context.Animes
            .ToList() // Récupère tous les animes
            .Where(a => a.AnimeTypes != null && a.AnimeTypes.Any(gt => types.Contains(gt))) // Filtre en C#
            .ToList();
    }
}