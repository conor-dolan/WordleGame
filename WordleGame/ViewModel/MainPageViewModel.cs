using System.Windows.Input;

namespace WordleGame.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand NavigateToGameCommand { get; }
        public ICommand NavigateToSettingsCommand { get; }
        public ICommand NavigateToScoreboardCommand { get; }

        public MainPageViewModel()
        {
            Title = "Main Menu";
            NavigateToGameCommand = new Command(async () => await NavigateToAsync("game"));
            NavigateToSettingsCommand = new Command(async () => await NavigateToAsync("settings"));
            NavigateToScoreboardCommand = new Command(async () => await NavigateToAsync("scoreboard"));
        }
    }
}
