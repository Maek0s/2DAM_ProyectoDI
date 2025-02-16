using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LoginSystemPowerCode.Systems;

namespace LoginSystemPowerCode.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly MgtDatabase mgtDatabase = new MgtDatabase();

        private string _correo;
        private string _password;
        private string _message;

        public RegisterViewModel()
        {


            RegisterCommand = new Command(OnRegister);  // Asignamos el comando para el botón de registro
        }

        // Propiedades para enlazar con la vista (UI)
        public string Correo
        {
            get => _correo;
            set
            {
                _correo = value;
                OnPropertyChanged(nameof(Correo));  // Notifica el cambio de propiedad
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));  // Notifica el cambio de propiedad
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));  // Notifica el cambio de mensaje
            }
        }

        // Comando para registrar usuario
        public ICommand RegisterCommand { get; }

        // Método que ejecuta el registro
        private async void OnRegister()
        {
            Debug.WriteLine("Ejecutando OnRegister()");
            
            if (string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Password))
            {
                Message = "Por favor ingrese correo y contraseña.";
                return;
            }

            if (Password.Length < 6)
            {
                Message = "La contraseña debe tener al menos 6 caracteres.";
                return;
            }

            if (Correo.Contains("@") == false)
            {
                Message = "El correo no es válido. Debe tener el caracter '@'";
                return;
            }

            Message = mgtDatabase.CrearUsuario(Correo, Password);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
