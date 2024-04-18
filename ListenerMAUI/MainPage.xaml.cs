namespace ListenerMAUI;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private async void OnStartClicked(object sender, EventArgs e)
	{
		// Define the port on which to listen for incoming connections
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

				// Read the data from the client
				NetworkStream stream = client.GetStream();
				byte[] buffer = new byte[1024];
				int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
				string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
				Console.WriteLine(dataReceived);

				// Close the client connection
				client.Close();
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
	}
}

