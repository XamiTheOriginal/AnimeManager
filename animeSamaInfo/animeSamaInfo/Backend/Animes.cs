namespace animeSamaInfo.Backend;

public class Animes
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Studio { get; set; }
    public int EpisodeTotal { get; set; }
    public int NumberSeason { get; set; }
    public int EpisodeSeen { get; set; }
    public int SeasonSeen { get; set; }
    public AnimeStatus Status { get; set; }
    public List<AnimeType>? AnimeTypes { get; set;}
    public DateTime AddDate { get; set; }
}