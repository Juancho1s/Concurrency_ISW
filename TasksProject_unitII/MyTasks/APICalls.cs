using System.Net.Http.Json;


class APICalls
{
    HttpClient client { get; set; }

    public APICalls()
    {
        client = new HttpClient();
    }

    public async Task<string> responses(string url, object request)
    {
        int tempts = 5;
        int waiting = 500;

        while (tempts > 0)
        {
            try
            {

                HttpResponseMessage response = await client.PostAsJsonAsync(url, request);

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
}