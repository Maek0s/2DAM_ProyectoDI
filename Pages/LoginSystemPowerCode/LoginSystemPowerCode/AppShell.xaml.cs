using LoginSystemPowerCode.Pages;

namespace LoginSystemPowerCode
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Rutas de navegación
            Routing.RegisterRoute(nameof(MainPage), typeof(LoginSystemPowerCode.MainPage));
            Routing.RegisterRoute(nameof(RegisterSystem), typeof(LoginSystemPowerCode.RegisterSystem));
            Routing.RegisterRoute(nameof(Perfil), typeof(LoginSystemPowerCode.Pages.Perfil));
        }
    }
}
