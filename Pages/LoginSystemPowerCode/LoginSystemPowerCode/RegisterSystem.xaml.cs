using System.Diagnostics;
using LoginSystemPowerCode.Pages;

namespace LoginSystemPowerCode
{
    public partial class RegisterSystem : ContentPage
    {
        private readonly FirebaseAuthService _authService;

        public RegisterSystem()
        {
            InitializeComponent();
            _authService = new FirebaseAuthService(); // Inicializa el servicio
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
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
                // Llama al servicio para crear el usuario
                string firebaseToken = await _authService.CreateUserWithEmailPassword(email, password);

                // Guarda el estado del usuario
                CurrentUser.FirebaseToken = firebaseToken;
                CurrentUser.Email = email;

                ShowMessage($"Cuenta creada exitosamente para: {email}");

                // Redirige a la página de inicio de sesión o a la página principal
                await Navigation.PushAsync(new Perfil());
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al crear cuenta: {ex.Message}");
            }
        }

        private void OnLabelClicked(object sender, EventArgs e)
        {
            // Redirige a la página de inicio de sesión
            Navigation.PushAsync(new MainPage());
        }

        private void ShowMessage(string message)
        {
            Debug.WriteLine(message);
        }
    }
}