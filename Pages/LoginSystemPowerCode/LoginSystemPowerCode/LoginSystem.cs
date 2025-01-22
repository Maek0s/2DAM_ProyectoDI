using System;
using System.Threading.Tasks;
using Firebase.Auth;

namespace LoginSystemPowerCode
{
    public class FirebaseAuthService
    {
        private readonly Firebase.Auth.FirebaseAuthProvider _authProvider;

        public FirebaseAuthService()
        {
            _authProvider = new Firebase.Auth.FirebaseAuthProvider(new FirebaseConfig("AIzaSyAR5FNxz48uvRNsRx_Gh2CTkLtw4vYmi_c"));
        }

        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            var auth = await _authProvider.SignInWithEmailAndPasswordAsync(email, password);
            return auth.FirebaseToken; // Token de autenticación
        }

        public async Task<string> CreateUserWithEmailPassword(string email, string password)
        {
            var auth = await _authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

            // Enviar correo de verificación después de crear la cuenta
            await SendEmailVerification(auth.FirebaseToken);

            return auth.FirebaseToken; // Token de autenticación
        }

        // Enviar correo de verificación
        private async Task SendEmailVerification(string firebaseToken)
        {
            // Obtener el usuario con el token de Firebase
            var user = await _authProvider.GetUserAsync(firebaseToken);

            // Verifica si el correo está verificado, si no, envía el correo de verificación
            if (!user.IsEmailVerified)
            {
                await _authProvider.SendEmailVerificationAsync(firebaseToken);
                Console.WriteLine("Correo de verificación enviado.");
            }
            else
            {
                Console.WriteLine("El correo ya está verificado.");
            }
        }

        public async Task<bool> IsEmailVerified(string firebaseToken)
        {
            var user = await _authProvider.GetUserAsync(firebaseToken);
            return user.IsEmailVerified;
        }
    }
    public static class CurrentUser
    {
        public static string FirebaseToken { get; set; }
        public static string Email { get; set; }
    }

    class LoginSystem
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Bienvenido al sistema de autenticación con Firebase.");

            // Crear una instancia del servicio de autenticación
            var authService = new FirebaseAuthService();

            // Solicitar credenciales al usuario
            Console.Write("Introduce tu correo electrónico: ");
            string email = Console.ReadLine();

            Console.Write("Introduce tu contraseña: ");
            string password = Console.ReadLine();

            try
            {
                // Intentar iniciar sesión
                string firebaseToken = await authService.LoginWithEmailPassword(email, password);

                // Guardar el estado del usuario autenticado
                CurrentUser.FirebaseToken = firebaseToken;
                CurrentUser.Email = email;

                Console.WriteLine($"Inicio de sesión exitoso como: {email}");
                Console.WriteLine($"Token de Firebase: {firebaseToken}");

                // Continuar con la lógica de tu aplicación
                if (IsUserAuthenticated())
                {
                    Console.WriteLine("El usuario está autenticado. Acceso permitido a funciones protegidas.");
                    // Llama a funciones restringidas aquí
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al iniciar sesión: " + ex.Message);
            }
        }

        // Método para verificar si un usuario está autenticado
        static bool IsUserAuthenticated()
        {
            return !string.IsNullOrEmpty(CurrentUser.FirebaseToken);
        }
    }
}
