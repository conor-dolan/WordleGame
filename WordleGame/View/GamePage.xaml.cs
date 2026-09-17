using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using WordleGame.ViewModel;

namespace WordleGame.View
{
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
            BindingContext = Application.Current?.Handler?.MauiContext?.Services.GetService<WordleViewModel>()
                ?? new WordleViewModel(new Services.WordleService());
        }

        private async void OnMainMenuClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//main");
        }

        private async void OnScoreboardClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("scoreboard");
        }
    }
}