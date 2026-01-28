namespace SerieInfo.Backend;

public class Serie
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Studio { get; set; }
    public int EpisodeTotal { get; set; }
    public int NumberSeason { get; set; }
    public int EpisodeSeen { get; set; }
    public int SeasonSeen { get; set; }
    public bool IsFavorite { get; set; }
    public SerieStatus Status { get; set; }
    public List<SerieType>? SerieTypes { get; set;}
    public DateTime AddDate { get; set; }
   

    public override string ToString()
    {
        string types = SerieTypes != null && SerieTypes.Count > 0 ? string.Join(", ", SerieTypes) : "Aucun";
        return $"ID: {Id}\nTitre: {Title} (Studio: {Studio})\n" +
               $"Statut: {Status}\nFavori: {IsFavorite}\n" +
               $"Progression: {EpisodeSeen}/{EpisodeTotal} épisodes, {SeasonSeen}/{NumberSeason} saisons\n" +
               $"Genres: {types}";
    }

}