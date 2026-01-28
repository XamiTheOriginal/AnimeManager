using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace SerieInfo.Backend;

public class SeriesManager : ISerieManager
{
    
    private readonly SerieDbContext _context = new SerieDbContext();
    
    public SeriesManager()
    {
        _context.Database.Migrate();
        
        if (!_context.Serie.Any()) //Ajout d'une série "Test" si db non existante ou vide
        {
            _context.Serie.Add(new Serie
            {
                Title = "South Park",
                Studio = "Comedy Central",
                EpisodeTotal = 330,
                NumberSeason = 26,
                Status = SerieStatus.Watching,
                IsFavorite = true,
                AddDate = DateTime.Now
            });

            _context.SaveChanges();
        }
    }

    
    public void AddSeries(Serie serie)
    {
        if (_context.Serie.Any(s => s.Title.ToLower() == serie.Title.ToLower()))
            return;
        
        serie.AddDate = DateTime.Now;
        _context.Serie.Add(serie);
        _context.SaveChanges();
    }

    public void RemoveDouble()
    {
        var groupes = _context.Serie.GroupBy(s => s.Title.ToLower());

        foreach (var groupe in groupes)
        {
            if (groupe.Count() > 1)
            {
                var seriesAGarder = groupe.First();
                var seriesADeleter = groupe.Skip(1);

                foreach (var s in seriesADeleter)
                {
                    _context.Serie.Remove(s);
                }
            }
        }

        _context.SaveChanges();
    }

    
    public void DeleteSeries(int id)
    {
        Serie? serieToDelete = _context.Serie.FirstOrDefault(a => a.Id == id);
        
        if (serieToDelete != null)
        {
            _context.Serie.Remove(serieToDelete);
            _context.SaveChanges();
        }
    }


    public void UpdateSeriesStatus(int id, SerieStatus newStatus)
    {
        Serie? anime = _context.Serie.FirstOrDefault(a => a.Id == id);
        
        if (anime != null)
        {
            anime.Status = newStatus;
            _context.SaveChanges();
        }
    }

    
    public void UpgradeProgression(int id, int episodeSeen, int seasonSeen)
    {
        Serie? serie = _context.Serie.FirstOrDefault(a => a.Id == id);
        
        if (serie == null) return; // La serie n'existe pas
        
        serie.EpisodeSeen = episodeSeen;
        serie.SeasonSeen = seasonSeen;
        
        if (serie.EpisodeSeen > serie.EpisodeTotal) serie.EpisodeSeen = serie.EpisodeTotal;
        if (serie.SeasonSeen > serie.NumberSeason) serie.SeasonSeen = serie.NumberSeason;

        if (serie.SeasonSeen >= serie.NumberSeason && serie.EpisodeSeen >= serie.EpisodeTotal)
        {
            serie.Status = SerieStatus.Seen;
        }
        else if (serie.EpisodeSeen > 0 || serie.SeasonSeen > 0)
        {
            if (serie.Status != SerieStatus.Watching && serie.EpisodeSeen != serie.EpisodeTotal)
            {
                serie.Status= SerieStatus.Watching;
            }
        }
        
        _context.SaveChanges();
    }
    
   
    public Serie? GetSeriesById(int id)
    {
        return _context.Serie.FirstOrDefault(a => a.Id == id);
    }

    
    public List<Serie> ListSeries(SerieStatus? status = null)
    {
        IQueryable<Serie> query = _context.Serie;
        if (status.HasValue)
        {
            query = query.Where(s =>  s.Status ==status.Value);
        }
        
        return query.ToList(); 
    }
    
    
    public List<Serie> Search(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new List<Serie>();
        }
        
        string searchTerm = keyword.Trim().ToLower();

        return _context.Serie
            .Where(a => 
                (a.Title != null && a.Title.ToLower().Contains(searchTerm)) || 
                (a.Studio != null && a.Studio.ToLower().Contains(searchTerm))
            )
            .ToList();
    }
    
    
    public List<Serie> ListSeriesSortedByDate()
    {
        return _context.Serie
            .OrderByDescending(a => a.AddDate) 
            .ToList();
    }

   
    public List<Serie> ListByType(List<SerieType> types)
    {
        if (types == null || !types.Any()) return new List<Serie>();
        
        IQueryable<Serie> query = _context.Serie;
        
        return _context.Serie.AsNoTracking() //AsNoTracking permet d economiser de la memoire en mettant en "read only"
            .AsEnumerable() // On passe en IEnumerable pour le traitement complexe
            .Where(a => a.SerieTypes != null && a.SerieTypes.Any(t => types.Contains(t)))
            .ToList();
    }
}