using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using LoginSystemPowerCode.Models;
using LoginSystemPowerCode.Systems;

namespace LoginSystemPowerCode.ViewModels
{
    public class CarteraViewModel : INotifyPropertyChanged
    {
        private readonly MgtDatabase _mgtDatabase = new MgtDatabase();

        private Usuario _usuario;
        public Usuario Usuario
        {
            get { return _usuario; }
            set
            {
                if (_usuario != value)
                {
                    _usuario = value;
                    OnPropertyChanged(nameof(Usuario));
                }
            }
        }

        private string _usuarioID;
        public string UsuarioID
        {
            get { return _usuarioID; }
            set
            {
                _usuarioID = value;
                CargarUsuario();
                Usuario = _mgtDatabase.ObtenerUsuarioPorId(int.Parse(value));
            }
        }

        public ICommand AnyadirFondosCommand5 { get; }
        public ICommand AnyadirFondosCommand10 { get; }
        public ICommand AnyadirFondosCommand25 { get; }
        public ICommand AnyadirFondosCommand50 { get; }
        public ICommand AnyadirFondosCommand100 { get; }

        /* MENU */
        public ICommand NavegarPerfilCommand { get; }
        public ICommand ViajarHomeCommand { get; }
        public ICommand ViajarSalidaCommand { get; }
        public ICommand ViajarGestionUsuariosCommand { get; }
        public ICommand NavegarGestionUsersCommand { get; }
        

        public CarteraViewModel()
        {
            AnyadirFondosCommand5 = new Command(AnyadirFondos5);
            AnyadirFondosCommand10 = new Command(AnyadirFondos10);
            AnyadirFondosCommand25 = new Command(AnyadirFondos25);
            AnyadirFondosCommand50 = new Command(AnyadirFondos50);
            AnyadirFondosCommand100 = new Command(AnyadirFondos100);

            // MENU //
            ViajarHomeCommand = new Command(async () => await NavegarID("PaginaPrincipal"));
            NavegarPerfilCommand = new Command(async () => await NavegarID("Perfil"));
            ViajarGestionUsuariosCommand = new Command(async () => await NavegarID("GestionUsuarios"));
            ViajarSalidaCommand = new Command(async () => await NavegarA("LoginPage"));
        }

        private void AnyadirFondos5()
        {
            Usuario.Saldo += 5;
            OnPropertyChanged(nameof(Usuario));

            // Guardar en la base de datos
            _mgtDatabase.ActualizarSaldoUsuario(Usuario.Id, Usuario.Saldo);
        }

        private void AnyadirFondos10()
        {
            Usuario.Saldo += 10;
            OnPropertyChanged(nameof(Usuario));

            // Guardar en la base de datos
            _mgtDatabase.ActualizarSaldoUsuario(Usuario.Id, Usuario.Saldo);
        }

        private void AnyadirFondos25()
        {
            Usuario.Saldo += 25;
            OnPropertyChanged(nameof(Usuario));

            // Guardar en la base de datos
            _mgtDatabase.ActualizarSaldoUsuario(Usuario.Id, Usuario.Saldo);
        }

        private void AnyadirFondos50()
        {
            Usuario.Saldo += 50;
            OnPropertyChanged(nameof(Usuario));

            // Guardar en la base de datos
            _mgtDatabase.ActualizarSaldoUsuario(Usuario.Id, Usuario.Saldo);
        }

        private void AnyadirFondos100()
        {
            Usuario.Saldo += 100;
            OnPropertyChanged(nameof(Usuario));

            // Guardar en la base de datos
            _mgtDatabase.ActualizarSaldoUsuario(Usuario.Id, Usuario.Saldo);
        }



        private async Task NavegarID(string pagina)
        {
            await Shell.Current.GoToAsync($"/{pagina}?usuario={UsuarioID}");
        }

        private async Task NavegarA(string pagina)
        {
            await Shell.Current.GoToAsync($"/{pagina}");
        }

        private void CargarUsuario()
        {
            if (int.TryParse(_usuarioID, out int id))
            {
                Usuario = _mgtDatabase.ObtenerUsuarioPorId(id);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

