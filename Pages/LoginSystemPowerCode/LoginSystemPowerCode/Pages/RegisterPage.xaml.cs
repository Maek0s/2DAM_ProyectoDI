using LoginSystemPowerCode.ViewModels;

namespace LoginSystemPowerCode.Pages;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();

        BindingContext = new RegisterViewModel();
    }

    // Evento para redirigir a la pantalla de inicio de sesión
    private async void OnLabelClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage()); // Redirigir a LoginPage
    }
}