namespace AnotherProject;
using System;
using System.Collections.ObjectModel;
using System.Text.Json;
// using Xamarin.Forms;



public class PokemonViewModel : BindableObject
{
    public ObservableCollection<Pokemon> Pokemones { get; set; }

    public List<string> multipleAPIResponses { get; set; }

    public bool increasing { get; set; }
    public BindableObject records { get; set; }

    public APICalls apiCalls { get; set; }

    public APIControl Control { get; set; }

    public PokemonResponse pokemonResponse { get; set; }

    public PokemonViewModel()
    {
        apiCalls = new APICalls();
        Control = new APIControl();
        multipleAPIResponses = new List<string>();
        pokemonResponse = new PokemonResponse();
        Pokemones = new ObservableCollection<Pokemon>();
        increasing = true;
    }

    public async void GetPokemons(string pokemonID)
    {
        string url = $"https://pokeapi.co/api/v2/pokemon/{pokemonID}";


        string details = await apiCalls.responses(url);
        if (details.Equals(null))
        {
            Console.WriteLine("There was something wronk requesting the specific Pokemon: " + pokemonID);
            return;
        }
        else
        {
            PokemonDetail pokemon = JsonSerializer.Deserialize<PokemonDetail>(details);

            Pokemon newRecord = new();
            newRecord.name = pokemon.name;
            newRecord.spriteFront = pokemon.sprites.front_default;
            newRecord.spriteBack = pokemon.sprites.back_default;

            PokemonesClear();
            Pokemones.Add(newRecord);
        }
    }

    public async void GetPokemons()
    {
        string url = $"https://pokeapi.co/api/v2/pokemon/?offset={Control.offset}&limit={Control.limit}";

        string json = await apiCalls.responses(url);

        if (json.Equals(null))
        {
            Console.WriteLine("There was something wrong with the api calling. Please try again later.");
            return;
        }
        else
        {
            pokemonResponse = JsonSerializer.Deserialize<PokemonResponse>(json);
            PokemonesClear();
            foreach (var item in pokemonResponse.results)
            {
                string details = await apiCalls.responses(item.url);
                if (details.Equals(null))
                {
                    Console.WriteLine("There was something wrong with the image request. Skipping this one...");
                    continue;
                }
                else
                {
                    PokemonDetail pokemon = JsonSerializer.Deserialize<PokemonDetail>(details);

                    Pokemon newRecord = new();
                    newRecord.name = pokemon.name;
                    newRecord.spriteFront = pokemon.sprites.front_default;
                    newRecord.spriteBack = pokemon.sprites.back_default;
                    Pokemones.Add(newRecord);
                }
            }
        }
    }

    public void NextPage()
    {
        if ((Control.offset + Control.limit) >= pokemonResponse.count)
        {
            return;
        }
        Control.offset += Control.limit;
        increasing = true;
        GetPokemons();
    }

    public void PreviousPage()
    {
        if (Control.offset == 0 | Control.limit >= pokemonResponse.count)
        {
            return;
        }
        Control.offset -= Control.limit;
        increasing = false;
        GetPokemons();
    }

    public void PokemonesClear()
    {
        Pokemones.Clear();
    }


    /*
        This is a method which it's supposed to be on another file, but I Kept it here for simplicity purposes.
    */
    public async void getData()
    {
        string placeholderResponse = await apiCalls.responses("https://jsonplaceholder.typicode.com/posts");
        string valorantAgents = await apiCalls.responses("https://valorant-api.com/v1/agents");
        string pokemonsName = await apiCalls.responses("https://pokeapi.co/api/v2/pokemon/?offset={Control.offset}&limit={Control.limit}");

        if (placeholderResponse.Equals(null) | valorantAgents.Equals(null))
        {
            Console.WriteLine("The method failed. No data retrieved.");
            return;
        }

        Post[] placeHolderAPIPosts = JsonSerializer.Deserialize<Post[]>(placeholderResponse);
        valorantAPIAgents valorantAPIAgents = JsonSerializer.Deserialize<valorantAPIAgents>(valorantAgents);
        PokemonResponse pokemonResponse = JsonSerializer.Deserialize<PokemonResponse>(pokemonsName);


        foreach (Post item in placeHolderAPIPosts)
        {
            multipleAPIResponses.Add(item.title);
        }
        foreach (Agent item in valorantAPIAgents.data)
        {
            multipleAPIResponses.Add(item.displayName);
        }
        foreach (PokemonResult item in pokemonResponse.results)
        {
            multipleAPIResponses.Add(item.name);
        }
    }
}