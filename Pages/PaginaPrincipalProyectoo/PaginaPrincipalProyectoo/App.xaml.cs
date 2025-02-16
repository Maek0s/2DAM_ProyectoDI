namespace PaginaPrincipalProyectoo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Forzar el tema claro
            Application.Current.UserAppTheme = AppTheme.Light;

            MainPage = new AppShell();
        }
    }
}
