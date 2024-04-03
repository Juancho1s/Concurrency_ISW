namespace First_MAUI_Proyect;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count += 1;

		if (count == 1)
		{
			CounterBtn.Text = $"Clicked {count} time";
		}
		else
		{
			CounterBtn.Text = $"Clicked {count} times";
			if ((count % 2 != 0 & count % 3 != 0 & count % 5 != 0 & count % 7 != 0 & count != 1) || count == 2 || count == 3 || count == 5 || count == 7)
			{
				Change.Text = "Prior number";
			}
			else
			{
				Change.Text = "Non-prior number";
			}
		}


		SemanticScreenReader.Announce(Change.Text);
		SemanticScreenReader.Announce(CounterBtn.Text);

		_ = myFunction();
		Console.WriteLine("First message");
	}

	private async Task myFunction()
	{
		await Task.Run(() =>
		{
			Thread.Sleep(5000);
			Console.WriteLine("Second message!!");
		});
	}

	private void OnRetryBtn(object sender, EventArgs e)
	{
		HttpClient client = new HttpClient();
		string uri = "https://httpbin.org/status/";

		_ = downloadStringWithRetries(client, uri, new int[] { 500, 300, 400, 200 });
	}

	async Task<string> downloadStringWithRetries(HttpClient client, string uri, int[] codes)
	{
		// Retry after 1 second, then after 3 seconds and so on...

		TimeSpan nextDelay = TimeSpan.FromSeconds(1);

		for (int i = 0; i <= 4; ++i)
		{
			try
			{
				Console.WriteLine(client.GetStringAsync(uri + codes[i]));
				return await client.GetStringAsync(uri + codes[i]);
			}
			catch (Exception e)
			{
				Console.Write("Failed to connect (Trying agian in " + nextDelay + "seconds): " + e);
			}
			await Task.Delay(nextDelay);
			nextDelay += nextDelay;
		}

		// Try one last time, allowing th error to propagate  if it's fatal
		return await client.GetStringAsync(uri);
	}


	private async void OnFromProgress(object sender, EventArgs e)
	{
		PBar.Progress = 0;

		Progress<int> progress = new Progress<int>();
		progress.ProgressChanged += (sender, e) =>
		{
			Console.WriteLine($"progress: {e}%");
			PBar.Progress = Convert.ToDouble(e)/100;
			
		};
		await processData(progress);
		Console.WriteLine("process completed.");
	}

	static async Task processData(IProgress<int> progress)
	{
		for (int i = 0; i <= 100; i++)
		{
			await Task.Delay(1);
			progress.Report(i);
		}
	}
}

