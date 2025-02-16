using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using LoginSystemPowerCode.Models;
using LoginSystemPowerCode.Pages;
using LoginSystemPowerCode.Systems;
using SQLite;

namespace LoginSystemPowerCode.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private readonly MgtDatabase mgtDatabase = new MgtDatabase();

        private string _username;
        private string _password;
        private string _errorMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginPageViewModel()
        {
            Username = "maek0spam@gmail.com";
            Password = "123";

            LoginCommand = new Command(OnLoginClicked);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        private async void OnLoginClicked()
        {
            ErrorMessage = mgtDatabase.IniciarSesion(Username, Password);

            if (ErrorMessage.Equals(""))
            {
                Usuario usuario = mgtDatabase.ObtenerUsuarioPorCorreo(Username);
                
                await Shell.Current.GoToAsync($"/Perfil?usuario={usuario.Id}");
            }
        }

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
