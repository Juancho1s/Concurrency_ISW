using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Text.Json;

class TaskExecutor : BindableObject, INotifyPropertyChanged
{
    public ObservableCollection<AllDataContainer> allRecords { get; private set; }

    private double _counter;

    public double Counter 
    {
        get { return _counter; }
        set
        {
            if (_counter != value)
            {
                _counter = value;
                OnPropertyChanged2(nameof(Counter));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged2;

    protected virtual void OnPropertyChanged2(string propertyName)
    {
        PropertyChanged2?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    HttpClient cliente { get; set; }

    Random_numbersModel random_NumbersModel { get; set; }
    APICalls apiCalls { get; set; }
    APIConfigRandomNumber apiConfig { get; set; }

    public TaskExecutor()
    {
        cliente = new HttpClient();
        apiCalls = new APICalls();
        allRecords = new ObservableCollection<AllDataContainer>();
        random_NumbersModel = new Random_numbersModel();
        apiConfig = new APIConfigRandomNumber();
        Counter = 0.0;
    }


    public async Task getAllData()
    {
        List<AllDataContainer> allRecordsData = await random_NumbersModel.getAllNumbers();

        allRecords.Clear();
        foreach (AllDataContainer item in allRecordsData)
        {
            allRecords.Add(item);
        }

    }

    public async Task deleteAllData()
    {
        allRecords.Clear();
        await random_NumbersModel.deleteAllNumbers();
    }

    public async Task get10000Numbers()
    {
        try
        {

            string resString = await apiCalls.responses(apiConfig.url, new
            {
                jsonrpc = "2.0",
                method = "generateIntegers",
                @params = new
                {
                    apiConfig.apiKey,
                    apiConfig.n,
                    apiConfig.min,
                    apiConfig.max,
                    replacement = true
                },
                id = 1
            });

            Response jsonResponse = JsonSerializer.Deserialize<Response>(resString);
            allRecords.Clear();

            int[] randomNumbers = jsonResponse.result.random.data;
            Console.WriteLine("Random numbers:");
            foreach (int number in randomNumbers)
            {
                Console.WriteLine(number);
                allRecords.Add(new AllDataContainer(1, number));
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"There was something wrong with the connection to the api: {e}");
        }
    }

    public void stopSorting(CancellationTokenSource cts)
    {
        cts.Cancel();
    }

    public async Task Soriting10000Numbers(CancellationTokenSource cts, IProgress<double> progress)
    {
        // Start the sorting process in parallel on four threads
        List<Task> sortingTasks = new List<Task>();

        int[] indexes = {
            0,
            this.allRecords.Count / 4,
            this.allRecords.Count / 2,
            this.allRecords.Count / 4 * 3,
            this.allRecords.Count
        };

        int counter = 0;

        // Execute 4 tasks in parallel
        while (counter < (indexes.Length - 1))
        {
            sortingTasks.Add(Task.Run(() => this.BubbleSortArray(indexes[counter], indexes[counter + 1], cts, progress)));
            await Task.Run(() => counter += 1);
            Console.WriteLine($"{counter}");
        }


        try
        {
            // Wait for all sorting tasks to complete
            await Task.WhenAll(sortingTasks);
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Sorting operation was cancelled: {ex.Message}");
        }
    }

    public async Task BubbleSortArray(int lowerIndex, int upperIndex, CancellationTokenSource cancellationToken, IProgress<double> progress)
    {
        Console.WriteLine("I am here");
        for (int i = lowerIndex; i < (upperIndex - 1); i++)
        {
            cancellationToken.Token.ThrowIfCancellationRequested(); // Check for cancellation request

            for (int j = (lowerIndex); j < (upperIndex - i); j++)
            {
                int number1 = this.allRecords[j].myRandomNumber;
                int number2 = this.allRecords[j + 1].myRandomNumber;

                if (number1 > number2)
                {
                    // Swap arr[j] and arr[j+1]
                    int temp = number1;
                    this.allRecords[j].myRandomNumber = number2;
                    this.allRecords[j + 1].myRandomNumber = temp;
                }
                // Report progress
                progress.Report((double)i / (upperIndex - lowerIndex) * 100);
                Console.WriteLine($"{Counter}%");
            }

            // Introduce a delay to allow for cancellation and to provide feedback
            await Task.Delay(200);
        }
    }
}
