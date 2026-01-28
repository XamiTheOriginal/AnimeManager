using System.Collections.Generic;
namespace SerieInfo.Backend;

public interface ISerieManager
{
    // Méthodes d'écriture (Commandes)
    void AddSeries(Serie serie);
    void DeleteSeries(int id);
    void UpdateSeriesStatus(int id, SerieStatus newStatus);
    void UpgradeProgression(int id, int episodeSeen, int seasonSeen);
    
    // Méthodes de lecture (Requêtes)
    Serie? GetSeriesById(int id);
    List<Serie> ListSeries(SerieStatus? status = null);
    List<Serie> Search(string keyword);
    List<Serie> ListByType(List<SerieType> types);
}