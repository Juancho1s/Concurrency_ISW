namespace AnotherProject;
using System;

public partial class MainPage : ContentPage
{
	int count = 0;
	public PokemonViewModel VMPokemons = new();

	public Progress<double> progress = new Progress<double>();

	public ParallelInvocation parallelInvocation = new ParallelInvocation();

	public NetworkClass networkClass = new NetworkClass();


	public MainPage()
	{
		InitializeComponent();

		BindingContext = new PokemonViewModel(); 
	}


	private void OnCounterClicked(object sender, EventArgs e)
	{
		count += 2;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private void OnShowPokemon(object sender, EventArgs e)
	{

		string idContent = IDValue.Text;
		if (string.IsNullOrWhiteSpace(idContent) | idContent.Equals(""))
		{
			VMPokemons.GetPokemons();
			Pagination.IsVisible = true;
			PBar.IsVisible = true;
			progress.ProgressChanged += (sender, e) =>
			{
				Console.WriteLine($"progress: {e}%");
				PBar.Progress = Convert.ToDouble(e) / 100;
			};

			processData(progress);
		}
		else
		{
			VMPokemons.GetPokemons(idContent);
			VMPokemons.Control.offset = 0;
			PBar.IsVisible = false;
			Pagination.IsVisible = false;
		}


		BindingContext = VMPokemons;
	}

	public void OnNextPage(object sender, EventArgs e)
	{
		VMPokemons.NextPage();
		processData(progress);
	}

	public void OnPreviousPage(object sender, EventArgs e)
	{
		VMPokemons.PreviousPage();
		processData(progress);
	}


	public void OnRequestingAPIs(object sender, EventArgs e)
	{
		// create a list of intems to prcess
		// VMPokemons.getData();
		parallelInvocation.threeAPIsInvocation();
	}

	public void OnPrintParallel(object sender, EventArgs e)
	{
		List<string> myDisplay = parallelInvocation.multipleAPIResponses;


		// foreach (string i in myDisplay)
		// {
		// 	Console.WriteLine("Item: " + i);
		// }

		//Process each items in the list concurrently
		Parallel.ForEach(myDisplay, item =>
		{
			Task.Delay(1000).Wait();
			Console.WriteLine($"Processed item {item}");
		});
		Console.WriteLine("All Items processed.");


	}

	public void OnMergeResources()
	{

	}

	public async void processData(IProgress<double> progress)
	{
		int counter = VMPokemons.pokemonResponse.count;
		int advanced = VMPokemons.Control.offset + VMPokemons.Control.limit;
		double currentPercentage = ((double)advanced / counter) * 100;

		if (VMPokemons.increasing)
		{
			for (int i = VMPokemons.Control.offset; i <= advanced; i++)
			{
				await Task.Delay(1);
				double percentage = ((double)i / counter) * 100;
				progress.Report(percentage);
			}
		}
		else
		{
			for (int i = advanced; i >= VMPokemons.Control.offset; i--)
			{
				await Task.Delay(1);
				double percentage = currentPercentage - ((double)i / counter) * 100;
				progress.Report(percentage);
			}
		}
	}

	private  async void OnNetworkClicked(object sender, EventArgs e){
		networkClass = new NetworkClass();
		List<string> myDisplay = parallelInvocation.multipleAPIResponses;
		Response res = await networkClass.sending("127.0.0.1", 5001, myDisplay);
	}

}