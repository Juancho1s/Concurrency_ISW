namespace senderToListener;

using System;
using System.Net.Sockets;
using System.Text;

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

    private void OnSendClicked(object sender, EventArgs e)
    {
        // Define the server IP address and port
        string serverIpAddress = "127.0.0.1";
        int port = 5001;

        try
        {
            // Initialize a TcpClient instance and connect to the server
            using (TcpClient client = new TcpClient(serverIpAddress, port))
            {
                // Get the network stream for sending data
                NetworkStream stream = client.GetStream();

                // Prepare the data to be sent
                string dataToSend = "Hello from client!";
                byte[] buffer = Encoding.UTF8.GetBytes(dataToSend);

                // Send the data to the server
                stream.Write(buffer, 0, buffer.Length);

                Console.WriteLine("Data sent to server.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

