namespace WordleGame.View
{
    public partial class ScoreboardPage : ContentPage
    {
        public ScoreboardPage()
        {
            InitializeComponent();
        }

        private async void OnMainMenuClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//main");
        }

        private async void OnGameClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("game");
        }
    }
}
