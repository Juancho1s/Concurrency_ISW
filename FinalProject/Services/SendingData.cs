using System.Net.Sockets;

class SendingData
{
    // Define the server IP address and port
    string serverIpAddress { get; set; }
    int port { get; set; }


    public SendingData()
    {
        serverIpAddress = "127.0.0.1";
        port = 5001;
    }


    public async Task toSendData(CancellationTokenSource cts, IProgress<double> progress, byte[][] dataToSend, int identifier)
    {
        try
        {
            // Initialize a TcpClient instance and connect to the server
            using (TcpClient client = new TcpClient(serverIpAddress, port))
            {
                // Get the network stream for sending data
                NetworkStream stream = client.GetStream();

                for (int i = 0; i < dataToSend.Length; i++)
                {
                    // Check if the task has been canceled
                    cts.Token.ThrowIfCancellationRequested();

                    // Send the data to the server asynchronously
                    await Task.Run(() => stream.Write(dataToSend[i], 0, dataToSend[i].Length));

                    // Report progress
                    progress.Report((double)i / dataToSend.Length * 100);

                    // Introduce a delay to allow for cancellation and to provide feedback
                    await Task.Delay(1);
                }

                Console.WriteLine($"Data sent completed from method: {identifier}");
            }
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"About processes monitoring: {ex.Message} from {identifier}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message} from {identifier}");
        }
    }

    public async Task stopSorting(CancellationTokenSource cts)
    {
        await Task.Run(() => { cts.Cancel(); });
    }
}