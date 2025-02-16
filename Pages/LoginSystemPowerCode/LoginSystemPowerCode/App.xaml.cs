using SQLitePCL;

namespace LoginSystemPowerCode
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Batteries_V2.Init(); // Inicializa SQLite correctamente

            // Forzar el tema claro
            Application.Current.UserAppTheme = AppTheme.Light;

            MainPage = new AppShell();
        }
    }
}
