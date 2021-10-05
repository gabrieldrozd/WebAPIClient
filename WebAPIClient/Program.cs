using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAPIClient
{
    class Program
    {
        //client inicjalizacja
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await GetHomeLibraryBooks();
        }

        /*
         * async Task
         * client zainicializowny wczesniej
         * w zaleznosci od tego co chcemy dostac:
         * client.GetSTRINGAsync(url do API) - zwraca wiadomosc jako tekst
         * client.GetSTREAMAsync(url do API) - zwraca wiadomosc jako json - do deserializacji
         * 
         */
        private static async Task GetHomeLibraryBooks()
        {
            client.DefaultRequestHeaders.Accept.Clear();

            //var stringTask = client.GetStringAsync("https://localhost:44304/api/Author");
            //var msg = await stringTask;
            //Console.WriteLine(msg);

            //get call
            var streamTask = client.GetStreamAsync("https://localhost:44304/api/Author");
            //deserializacja otrzymanej wiadomosci - POTRZEBNA klasa szablon do deserializacji
            var authors = await JsonSerializer.DeserializeAsync<List<Author>>(await streamTask);

            //wypisanie wiadomosci - po deserializacji mamy obiekt c# gotowy do dalszej obrobki badz do zapisu w porzadany sposob
            foreach (var author in authors)
            {
                Console.WriteLine($"{author.firstName} {author.lastName}");
            }
        }

        //@@ dodatkowo wiecej funkcjonalnego kodu
        //private static async Task ProcessRepositories()
        //{
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        //    client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

        //    //var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
        //    var streamTask = await client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
        //    var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(streamTask);

        //    //var msg = await stringTask;
        //    //Console.WriteLine(msg);
        //    foreach (var repo in repositories)
        //    {
        //        Console.WriteLine(repo.Name);
        //    }
        //}
    }

    //klasa jako szablon - kopiuj wklej w converter - propy albo z duzej albo z malej z anotacja
    public class Author
    {
        // jesli propy w konwencji solid to anotacja - w przeciwnym wypadku wiadomosc pusta bedzie
        //[JsonPropertyName("id")]
        //public int Id { get; set; }

        //jesli propy z malej to anotacja nie jest wymagana
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string bio { get; set; }
        public List<object> books { get; set; }
    }

    public class Repository
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
