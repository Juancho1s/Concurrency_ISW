using System.Text.Json;


public class APICalls
{
    public HttpClient client { get; set; }
    public string[] apiUrls { get; set; }

    public string[] definedResponses { get; set; }

    public APICalls()
    {
        client = new HttpClient();
        apiUrls = new string[] {
            "https://jsonplaceholder.typicode.com/posts",
            "https://valorant-api.com/v1/agents",
            "https://pokeapi.co/api/v2/pokemon/?offset={Control.offset}&limit={Control.limit}"
        };
    }

    public async Task<string> responses(string url)
    {
        int tempts = 5;
        int waiting = 500;

        while (tempts > 0)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"The end point wasn't unaccesable (wating for: {waiting} ms)" + e);
                Thread.Sleep(waiting);
                waiting += waiting;
                tempts--;
                if (tempts == 0)
                {
                    return null;
                }
            }
        }
        return null;
    }

    public async Task<string[]> GetApiResponsesInParallel()
    {
        var tasks = new Task<string>[this.apiUrls.Length];

        try
        {
            for (int i = 0; i < this.apiUrls.Length; i++)
            {
                tasks[i] = responses(this.apiUrls[i]);
            }

            // Await all tasks to complete
            return await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            Console.WriteLine($"The connection couldn't be stablished with on of the apis, Error: {e.Message}");
            return null;
        }
    }

    public async Task<string[]> GetApiResponsesInParallel(string[] apiUrl)
    {
        var tasks = new Task<string>[apiUrls.Length];

        try
        {
            for (int i = 0; i < apiUrls.Length; i++)
            {
                tasks[i] = responses(apiUrl[i]);
            }
            // Await all tasks to complete
            return await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            Console.WriteLine($"The connection couldn't be stablished with on of the apis, Error: {e.Message}");
            return null;
        }

        // Await all tasks to complete
    }

    /*
     * These are methodd with a deffined structure which couldn't work in every  case, so I had to make it generic.
    */
    public async void jsonSerializerGetTitle(List<string> multipleAPIResponses)
    {
        string[] definedResponses = await GetApiResponsesInParallel();

        if (definedResponses.Equals(null))
        {
            Console.WriteLine("There was an erro processing your request.");
            return;
        }
        else
        {
            foreach (Post item in JsonSerializer.Deserialize<Post[]>(definedResponses[0]))
            {
                multipleAPIResponses.Add(item.title);
            }
        }

    }

    public async void valorantAPIAgentsGetAgent(List<string> multipleAPIResponses)
    {
        string[] definedResponses = await GetApiResponsesInParallel();

        if (definedResponses.Equals(null))
        {
            Console.WriteLine("There was an errro processing your request.");
            return;
        }
        else
        {
            foreach (Agent item in JsonSerializer.Deserialize<valorantAPIAgents>(definedResponses[1]).data)
            {
                multipleAPIResponses.Add(item.displayName);
            }
        }
    }

    public async void pokemonResponseGetName(List<string> multipleAPIResponses)
    {
        string[] definedResponses = await GetApiResponsesInParallel();

        if (definedResponses.Equals(null))
        {
            Console.WriteLine("There was an errro processing your request.");
            return;
        }
        else
        {
            foreach (PokemonResult item in JsonSerializer.Deserialize<PokemonResponse>(definedResponses[2]).results)
            {
                multipleAPIResponses.Add(item.name);
            }
        }
    }
}