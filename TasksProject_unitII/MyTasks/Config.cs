public class Config
{
    public string host { get; set; }
    public int port { get; set; }
    public string dataBase { get; set; }
    public string user { get; set; }
    public string passwrod { get; set; }


    public Config()
    {
        this.host = "127.0.0.2";
        this.port = 3005;
        this.dataBase = "ordenation_data_holder";
        this.user = "root";
        this.passwrod = "123456789";
    }
}

public class APIConfigRandomNumber
{
    public string apiKey { get; private set; }
    public int n { get; private set; }
    public int min { get; private set; }
    public int max { get; private set; }
    public string url { get; private set; }

    public APIConfigRandomNumber()
    {
        this.apiKey = "ecba5248-379a-44bc-8e5f-de4ddcd9ae51";
        this.n = 100;
        this.min = 1;
        this.max = 10000;
        this.url = "https://api.random.org/json-rpc/2/invoke";
    }
}