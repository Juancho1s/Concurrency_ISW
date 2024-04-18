using System.Net.Sockets;
using System.Text;

class BufferTesting
{
    public async Task sendingData(string dataToSend)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(dataToSend);

        foreach (var item in buffer)
        {
            await Task.Run(() => { Console.WriteLine(item); });
        }
    }

    public async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            // Read the data from the client
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;
            StringBuilder dataReceived = new StringBuilder();
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                dataReceived.Append(data);
                Console.WriteLine(data);
            }

            // Optionally, you can process or respond to the received data here.

            // Close the client connection
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
    }
}