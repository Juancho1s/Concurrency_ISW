using System.ComponentModel;
using System.Text;

class DataProcessing : INotifyPropertyChanged
{
    // Declare the variables
    private APICaller aPICaller { get; set; }
    private SendingData sendingData { get; set; }

    private CancellationTokenSource cts { get; set; }
    public string[] responses { get; set; }


    private double _counter1;

    public double Counter1
    {
        get { return _counter1; }
        set
        {
            if (_counter1 != value)
            {
                _counter1 = value;
                OnPropertyChanged2(nameof(Counter1));
            }
        }
    }

    private double _counter2;

    public double Counter2
    {
        get { return _counter2; }
        set
        {
            if (_counter2 != value)
            {
                _counter2 = value;
                OnPropertyChanged2(nameof(Counter2));
            }
        }
    }

    private double _counter3;

    public double Counter3
    {
        get { return _counter3; }
        set
        {
            if (_counter3 != value)
            {
                _counter3 = value;
                OnPropertyChanged2(nameof(Counter3));
            }
        }
    }


    public DataProcessing()
    {
        aPICaller = new APICaller();
        sendingData = new SendingData();
        responses = new string[3];
        cts = new CancellationTokenSource();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged2(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task result(string[] URLs)
    {
        // Start the process in parallel on threads
        List<Task<string>> callingResources = new List<Task<string>>();

        foreach(string url in URLs)
        {
            // Adding Requesting process to the list
            callingResources.Add(aPICaller.GetJsonResponseAsync(url));
        }


        // Wait for all the processes to finish
        await Task.WhenAll(callingResources);

        for (int i = 0; i < URLs.Length; i++)
        {
            responses[i] = await callingResources[i];
        }
    }

    public async Task toSendData(IProgress<double>[] progress)
    {
        cts = new CancellationTokenSource();

        Task[] tasks = new Task[responses.Length];

        for (int i = 0; i < responses.Length; i++)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(responses[i]);

            byte[][] dataContainer = new byte[buffer.Length][];

            for (int k = 0; k < buffer.Length; k++)
            {
                dataContainer[k] = new byte[1];
                dataContainer[k][0] = buffer[k];
            }

            tasks[i] = sendingData.toSendData(cts, progress[i], dataContainer, i);
        }

        await Task.WhenAll(tasks);
    }

    public async Task cancel()
    {
        await Task.Run(()=>{cts.Cancel();});
    }
}