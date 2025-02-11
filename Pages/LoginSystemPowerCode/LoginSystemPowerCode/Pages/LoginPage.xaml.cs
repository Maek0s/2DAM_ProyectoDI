using System.Collections.ObjectModel;
using System.Diagnostics;
using LoginSystemPowerCode.Models;
using LoginSystemPowerCode.Pages;

namespace LoginSystemPowerCode.Pages {
    public partial class LoginPage : ContentPage
    {
        private readonly FirebaseAuthService _authService;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = this;

            Debug.WriteLine("LoginPage");
            
            _authService = new FirebaseAuthService(); // Inicializa el servicio

            ePassword.Completed += (s, e) => OnLoginButtonClicked(s, e);

            testing();
        }

        public void testing()
        {
            
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

                // Redirige o realiza acciones después del inicio de sesión exitoso
                await Navigation.PushAsync(new Perfil());
            }
            catch (Exception ex)
            {
                ShowMessage($"Error en las credenciales.");
                Debug.WriteLine(ex);
            }
        }

        private void OnLabelClicked(object sender, EventArgs e)
        {
            // Redirige a la página de registro
            Navigation.PushAsync(new RegisterSystem());
        }

        private void ShowMessage(string message)
        {
            lblErrors.Text = message;
            Debug.WriteLine(message);
        }
    }
}