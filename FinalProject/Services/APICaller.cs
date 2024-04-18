class APICaller
{
    HttpClient client { get; set; }
    private const int MaxAttempts = 5;
    private const int InitialWaitTimeMs = 500;


    // Constructor
    public APICaller()
    {
        client = new HttpClient();
    }


    /*
        Methods to get responses
        / <summary>
        / This C# async method attempts to retrieve a JSON response from a specified URL with exponential
        / backoff retry logic.
        / </summary>
        / <param name="url">The `url` parameter in the `GetJsonResponseAsync` method is a string
        / representing the URL from which you want to retrieve a JSON response asynchronously.</param>
        / <returns>
        / The `GetJsonResponseAsync` method returns a `Task<string?>`. The method attempts to make a GET
        / request to the specified URL using a HttpClient instance. If the request is successful (HTTP
        / status code 2xx), the method returns the response content as a string. If the request fails or
        / encounters an exception, the method retries with exponential backoff up to a maximum number of
        / attempts (defined
        / </returns>
    */
    public async Task<string> GetJsonResponseAsync(string url)
    {
        int attemptsLeft = MaxAttempts;
        int currentWaitTime = InitialWaitTimeMs;

        while (attemptsLeft > 0)
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
                    Console.WriteLine($"Failed to make request: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"The endpoint was unreachable (waiting for: {currentWaitTime} ms). Exception: {e.Message}");
                await Task.Delay(currentWaitTime);
                currentWaitTime *= 2; // Exponential backoff
                attemptsLeft--;

                if (attemptsLeft == 0)
                {
                    Console.WriteLine("Max attempts reached. Aborting.");
                    return "";
                }
            }
        }

        return "";
    }
}