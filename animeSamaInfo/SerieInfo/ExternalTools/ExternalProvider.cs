using System.Text.Json;
using SerieInfo.Backend;

namespace animeSamaInfo.ExternalTools;

public class ExternalProvider
{
    private string link = "";

    public class SerieDto //Classe pour correspondre à l'API
    {
        public string name { get; set; }        // L'API envoie "name"
        public int runtime { get; set; }       // L'API envoie la durée
        public List<string> genres { get; set; } // L'API envoie des strings
    }

    private List<SerieType> MapGenres(SerieDto serie)
    {
        List<SerieType> res = new List<SerieType>();
        foreach (var genreStr in serie.genres)
        {
            // On essaie de convertir la string en Enum SerieType
            // Le "true" permet d'ignorer la casse (ex: "comedy" marche pour "Comedy")
            if (Enum.TryParse<SerieType>(genreStr, true, out SerieType resultEnum))
            {
                res.Add(resultEnum);
            }
        }
        return res;

        return res;
    }
    
    private List<Serie> ConvertJson(string json)
    {
        var importedSeries = JsonSerializer.Deserialize<List<SerieDto>>(json);
        List<Serie> result = new List<Serie>();
        foreach (var serie in importedSeries)
        {
            Serie currSerie = new Serie
            {
                Title = serie.name,
                Status = SerieStatus.None,
                SerieTypes = MapGenres(serie)
            };
                result.Add(currSerie);
        }

        return result;
    }
    
    
    public async Task getData()
    {
        using HttpClient client = new HttpClient();
        try
        {
            var response = await client.GetAsync(link);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                ConvertJson(json);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erreur : "+e.Message);
        }
    }
}