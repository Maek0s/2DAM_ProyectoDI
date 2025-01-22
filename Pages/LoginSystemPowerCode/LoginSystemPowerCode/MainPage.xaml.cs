using System.Diagnostics;

namespace LoginSystemPowerCode
{
    public partial class MainPage : ContentPage
    {
        private readonly FirebaseAuthService _authService;

        public MainPage()
        {
            InitializeComponent();
            _authService = new FirebaseAuthService(); // Inicializa el servicio

        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string email = eUsername.Text;
            string password = ePassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowMessage("Por favor, ingresa tu correo y contraseña.");
                return;
            }

            try
            {
                // Llama al servicio de Firebase para iniciar sesión
                string firebaseToken = await _authService.LoginWithEmailPassword(email, password);

                bool isVerified = await _authService.IsEmailVerified(firebaseToken);
                if (!isVerified)
                {
                    ShowMessage("Por favor, verifica tu correo antes de iniciar sesión.");
                    return;
                }

                // Guarda el estado del usuario
                CurrentUser.FirebaseToken = firebaseToken;
                CurrentUser.Email = email;

                ShowMessage($"Bienvenido, {email}!");

                // Redirige o realiza acciones después del inicio de sesión exitoso
                await Navigation.PushAsync(new ContentPage { Title = "Dashboard", Content = new Label { Text = "Inicio exitoso." } });
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}");
            }
        }

        private void OnLabelClicked(object sender, EventArgs e)
        {
            // Redirige a la página de registro
            Navigation.PushAsync(new RegisterSystem());
        }


        private void ShowMessage(string message)
        {
           Debug.WriteLine(message);
        }
    }

}
