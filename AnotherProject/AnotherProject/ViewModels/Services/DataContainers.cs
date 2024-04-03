public class Pokemon
{
    public string name { get; set; }
    public string spriteFront { get; set; }
    public string spriteBack { get; set; }
}

public class PokemonDetail
{
    public string name { get; set; }
    public Sprites sprites { get; set; }
}

public class Sprites
{
    public string front_default { get; set; }
    public string back_default { get; set; }
}

public class PokemonResponse
{
    public int count { get; set; }
    public List<PokemonResult> results { get; set; }

    public PokemonResponse()
    {
        this.count = 1000;
    }
}

public class PokemonResult
{
    public string name { get; set; }
    public string url { get; set; }
}

public class APIControl
{
    public int offset { get; set; }
    public int limit { get; set; }

    public APIControl()
    {
        this.offset = 0;
        this.limit = 20;
    }
}


public class Post
{
    public string title { get; set; }
}



public class valorantAPIAgents
{
    public List<Agent> data { get; set; }
}

public class Agent
{
    public string displayName { get; set; }
}

public class Response
{
    public string status { get; set; }
    public string message { get; set; }

    public Response()
    {
        this.message = "Connection Established!";
    }
}