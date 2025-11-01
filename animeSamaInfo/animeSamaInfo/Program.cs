// Dans Program.cs

using animeSamaInfo;
// Assurez-vous d'avoir bien instancié le manager :
AnimeManager manager = new AnimeManager(); 

Console.WriteLine("\n--- LISTE COMPLÈTE DU CATALOGUE (Lecture de la DB) ---");

// 1. Appel de la méthode sans paramètre
List<Animes> catalogueComplet = manager.ListAnimes(); 

if (catalogueComplet.Any())
{
    Console.WriteLine($"Total d'animes trouvés : {catalogueComplet.Count}");
    Console.WriteLine("------------------------------------------");

    // 2. Boucle pour afficher chaque anime
    foreach (var anime in catalogueComplet)
    {
        // Utilisation des propriétés de l'objet Animes
        Console.WriteLine($"ID: {anime.Id}");
        Console.WriteLine($"  Titre: {anime.Title} (Studio: {anime.Studio})");
        Console.WriteLine($"  Statut: {anime.Status}");
        Console.WriteLine($"  Progression: {anime.EpisodeSeen}/{anime.EpisodeTotal} épisodes");
        // Les AnimeTypes (genres) sont aussi chargés
        Console.WriteLine($"  Genres: {string.Join(", ", anime.AnimeTypes!)}"); 
        Console.WriteLine("------------------------------------------");
    }
}
else
{
    Console.WriteLine("Votre catalogue est vide. Ajoutez des animes d'abord !");
}