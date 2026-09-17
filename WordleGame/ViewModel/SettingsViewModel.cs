namespace WordleGame.ViewModel;
public class SettingsViewModel : BaseViewModel
{
    private bool isDark;
    private double fontSize = 16;

    public bool IsDark
    {
        get => isDark;
        set
        {
            if (isDark != value)
            {
                isDark = value;
                OnPropertyChanged();
                if (Application.Current != null)
                {
                    Application.Current.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
                }
            }
        }
    }

        public double FontSize
        {
            get => fontSize;
            set
            {
            if (fontSize != value)
            {
                fontSize = value;
                OnPropertyChanged();
            }
        }
    }

}
