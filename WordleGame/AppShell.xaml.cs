using Microsoft.Maui.Controls;

namespace WordleGame
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register the route for LoginPage, MainPage and GamePage
            Routing.RegisterRoute("login", typeof(View.LoginPage));
            Routing.RegisterRoute("main", typeof(View.MainPage));
            Routing.RegisterRoute("game", typeof(View.GamePage));
            Routing.RegisterRoute("scoreboard", typeof(View.ScoreboardPage));
            Routing.RegisterRoute("settings", typeof(View.SettingsPage));
        }
    }
}
