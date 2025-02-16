using LoginSystemPowerCode.Pages;
using LoginSystemPowerCode.ViewModels;

namespace LoginSystemPowerCode
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Rutas de navegación
            Routing.RegisterRoute(nameof(MainPage), typeof(LoginSystemPowerCode.MainPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(LoginSystemPowerCode.Pages.RegisterPage));
            Routing.RegisterRoute(nameof(Perfil), typeof(LoginSystemPowerCode.Pages.Perfil));
            Routing.RegisterRoute(nameof(LoginPageViewModel), typeof(LoginSystemPowerCode.ViewModels.LoginPageViewModel));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginSystemPowerCode.Pages.LoginPage));
        }
    }
}
