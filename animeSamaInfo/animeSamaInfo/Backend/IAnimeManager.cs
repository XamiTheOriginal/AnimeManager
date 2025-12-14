using System.Collections.Generic;
namespace animeSamaInfo.Backend;

public interface IAnimeManager
{
    // Méthodes d'écriture (Commandes)
    void AddAnime(Animes anime);
    void DeleteAnime(int id);
    void UpdateAnimeStatus(int id, AnimeStatus newStatus);
    void UpgradeProgression(int id, int episodeSeen, int seasonSeen);
    
    // Méthodes de lecture (Requêtes)
    Animes? GetAnimeById(int id);
    List<Animes> ListAnimes(AnimeStatus? status = null);
    List<Animes> Search(string keyword);
    List<Animes> ListByType(List<AnimeType> types);
}