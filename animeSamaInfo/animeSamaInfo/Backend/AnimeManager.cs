using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace animeSamaInfo.Backend;

public class AnimeManager : IAnimeManager
{
    
    private readonly AnimeDbContext _context = new AnimeDbContext();
    
    public AnimeManager()
    {
        _context.Database.Migrate(); 
    }

    
    public void AddAnime(Animes anime)
    {
        anime.AddDate = DateTime.Now;
        _context.Animes.Add(anime);
        _context.SaveChanges();
    }

    
    public void DeleteAnime(int id)
    {
        Animes? animeToDelete = _context.Animes.FirstOrDefault(a => a.Id == id);
        
        if (animeToDelete != null)
        {
            _context.Animes.Remove(animeToDelete);
            _context.SaveChanges();
        }
    }


    public void UpdateAnimeStatus(int id, AnimeStatus newStatus)
    {
        Animes? anime = _context.Animes.FirstOrDefault(a => a.Id == id);
        
        if (anime != null)
        {
            anime.Status = newStatus;
            _context.SaveChanges();
        }
    }

    
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
    
   
    public Animes? GetAnimeById(int id)
    {
        return _context.Animes.FirstOrDefault(a => a.Id == id);
    }

    
    public List<Animes> ListAnimes(AnimeStatus? status = null)
    {
        IQueryable<Animes> query = _context.Animes;

        if (status.HasValue)
        {
            query = query.Where(a => a.Status == status.Value);
        }
        
        return query.ToList(); 
    }
    
    
    public List<Animes> Search(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new List<Animes>();
        }
        
        string searchTerm = keyword.Trim().ToLower();

        return _context.Animes
            .Where(a => 
                (a.Title != null && a.Title.ToLower().Contains(searchTerm)) || 
                (a.Studio != null && a.Studio.ToLower().Contains(searchTerm))
            )
            .ToList();
    }
    
    
    public List<Animes> ListAnimesSortedByDate()
    {
        return _context.Animes
            .OrderByDescending(a => a.AddDate) 
            .ToList();
    }

   
    public List<Animes> ListByType(List<AnimeType> types)
    {
        return _context.Animes
            .ToList() // Récupère tous les animes
            .Where(a => a.AnimeTypes != null && a.AnimeTypes.Any(gt => types.Contains(gt)))
            .ToList();
    }
}