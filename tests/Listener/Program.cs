using System.Net;
using System.Net.Sockets;
using System.Text;


BufferTesting bufferTesting = new BufferTesting();

/// Define the port on which to listen for incoming connections
int port = 5001;
// Define the IP address on which to listen for incoming connections
string ipAddress = "127.0.0.1";

// Initialize a TcpListener instance
TcpListener server = new TcpListener(IPAddress.Parse(ipAddress), port);

// Start listening for client requests
server.Start();
Console.WriteLine($"Server started on the address {ipAddress}:{port} ");

try
{
    while (true)
    {
        // Accept the client connection
        TcpClient client = await server.AcceptTcpClientAsync();
        Console.WriteLine("Client connected.");

        // Handle client communication in a separate task
        _ = bufferTesting.HandleClientAsync(client);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    // Stop listening for incoming connections
    server.Stop();
}