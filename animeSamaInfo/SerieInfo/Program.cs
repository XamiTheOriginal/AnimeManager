using System.Runtime.InteropServices.JavaScript;
using SerieInfo.Backend;

internal class Program
{
    public static void Main(string[] args)
    {
        SeriesManager manager = new SeriesManager();
        
        Console.WriteLine("\n--- LISTE COMPLÈTE DU CATALOGUE (Lecture de la DB) ---");
        manager.RemoveDouble();
        List<Serie> catalogueComplet = manager.ListSeries(); 
        
        if (catalogueComplet.Any())
        {
            Console.WriteLine($"Total de séries trouvés : {catalogueComplet.Count}");
            Console.WriteLine("------------------------------------------");
    
            foreach (var anime in catalogueComplet)
            {
               
                Console.WriteLine(anime.ToString()); 
                Console.WriteLine("------------------------------------------");
            }
        }
        else
        {
            Console.WriteLine("Votre catalogue est vide. Ajoutez des séries d'abord !");
        }
    }
}