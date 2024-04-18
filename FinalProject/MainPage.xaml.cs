namespace FinalProject;

public partial class MainPage : ContentPage
{
	private DataProcessing dataProcessing = new DataProcessing();

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

	private async void OnGettingResources(object sender, EventArgs e)
	{
		await dataProcessing.result([URL1.Text, URL2.Text, URL3.Text]);

		foreach (var item in dataProcessing.responses)
		{
			Console.WriteLine($"{item}");
		}
	}

	private async void OnSendingAllData(object sender, EventArgs e)
	{
		Progress<double>[] progressReporter = {
			new Progress<double>(value =>
			{
				dataProcessing.Counter1 =  value;
				PBar1.Progress = value / 100;
			}),
			new Progress<double>(value =>
			{
				dataProcessing.Counter2 =  value;
				PBar2.Progress = value / 100;
			}),
			new Progress<double>(value =>
			{
				dataProcessing.Counter3 =  value;
				PBar3.Progress = value / 100;
			})
		};

		await dataProcessing.toSendData(progressReporter);
	}

	private async void OnCancelSending(object sender, EventArgs e)
	{

		await dataProcessing.cancel();
	}
}

