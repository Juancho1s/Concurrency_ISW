using System;

namespace TasksProject_unitII
{
    public partial class MainPage : ContentPage
    {
        TaskExecutor taskExecutor = new TaskExecutor();
        CancellationTokenSource cts { get; set; }
        internal TaskExecutor DataContext { get; }

        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            cts = new CancellationTokenSource();
            BindingContext = taskExecutor;

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

        private async void onGettingAllNumbers(object sender, EventArgs e)
        {
            await taskExecutor.getAllData();
        }

        private async void onDeletingAllNumbers(object sender, EventArgs e)
        {
            await taskExecutor.deleteAllData();
        }

        private async void onGetting10000Numbers(object sender, EventArgs e)
        {
            await taskExecutor.get10000Numbers();
        }

        private async void onSorting10000Numbers(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            PBar.IsVisible = true;
            stoping.IsVisible = true;

            // Create a progress reporter
            var progressReporter = new Progress<double>(value =>
            {
                taskExecutor.Counter =  value;
                PBar.Progress = value / 100;
            });

            await taskExecutor.Soriting10000Numbers(cts, progressReporter);

            // Sorting completed, hide progress bar
            PBar.IsVisible = false;
            stoping.IsVisible = false;
        }

        private void onStopSorting(object sender, EventArgs e)
        {
            PBar.IsVisible = false;
            stoping.IsVisible = false;

            taskExecutor.stopSorting(cts);
        }

    }

}