namespace WordleGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            Dispatcher.Dispatch(async () => await Shell.Current.GoToAsync("//login"));

        }
    }
}
