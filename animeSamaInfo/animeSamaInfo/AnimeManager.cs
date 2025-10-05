using System.Text.Json;

namespace animeSamaInfo;

public class AnimeManager
{
    private List<Animes> _animes = new();

    public void AddAnime(Animes anime)
    {
        anime.Id = _animes.Count > 0 ? _animes.Max(a => a.Id) + 1 : 1;
        anime.AddDate = DateTime.Now;
        _animes.Add(anime);
    }

    public void DeleteAnime(int id)
    {
        _animes.RemoveAll(a => a.Id == id);
    }

    public void UpdateAnimeStatus(int id, AnimeStatus newStatus)
    {
        Animes? anime = _animes.FirstOrDefault(a => a.Id == id);
        if (anime != null) anime.Status = newStatus;
    }

    public void UpgradeProgression(int id, int episodeSeen, int seasonSeen)
    {
        Animes? anime = _animes.FirstOrDefault(a => a.Id == id);
        
        if (anime != null && anime.EpisodeSeen >= anime.EpisodeTotal && anime.SeasonSeen >= anime.NumberSeason)
        {
            anime.Status = AnimeStatus.Seen;
        }
        if (anime != null)
        {
            anime.EpisodeSeen = episodeSeen;
            anime.SeasonSeen = seasonSeen;
        }
    }

    public List<Animes> ListAnimes(AnimeStatus? status = null)
    {
        if (status == null)
        {
            return _animes;
        }
        return (from a in _animes where a.Status == status select a).ToList();
    }

    public List<Animes> ListByType(List<AnimeType> type)
    {
       return _animes.Where(a=> a.AnimeTypes != null && a.AnimeTypes.Any(gt =>type.Contains(gt))).ToList();
    }

    public void Save(string path)
    {
        string json = JsonSerializer.Serialize(_animes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
    
    public void Load(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _animes = JsonSerializer.Deserialize<List<Animes>>(json) ?? new List<Animes>();
        }
    }
}