using animeSamaInfo.Backend;

internal class Program
{
    public static void Main(string[] args)
    {
        AnimeManager manager = new AnimeManager();
       
        Console.WriteLine("\n--- LISTE COMPLÈTE DU CATALOGUE (Lecture de la DB) ---");
        
        List<Animes> catalogueComplet = manager.ListAnimes(); 

        if (catalogueComplet.Any())
        {
            Console.WriteLine($"Total d'animes trouvés : {catalogueComplet.Count}");
            Console.WriteLine("------------------------------------------");
    
            foreach (var anime in catalogueComplet)
            {
               
                Console.WriteLine($"ID: {anime.Id}");
                Console.WriteLine($"  Titre: {anime.Title} (Studio: {anime.Studio})");
                Console.WriteLine($"  Statut: {anime.Status}");
                Console.WriteLine($"  Progression: {anime.EpisodeSeen}/{anime.EpisodeTotal} épisodes");
                Console.WriteLine($"  Genres: {string.Join(", ", anime.AnimeTypes!)}"); 
                Console.WriteLine("------------------------------------------");
            }
        }
        else
        {
            Console.WriteLine("Votre catalogue est vide. Ajoutez des animes d'abord !");
        }
    }
}