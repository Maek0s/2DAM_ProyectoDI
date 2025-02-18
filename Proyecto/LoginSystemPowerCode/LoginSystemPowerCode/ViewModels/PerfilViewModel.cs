using LoginSystemPowerCode.Models;
using LoginSystemPowerCode.Pages;
using LoginSystemPowerCode.Systems;

using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;


namespace LoginSystemPowerCode.ViewModels
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        private readonly MgtDatabase _mgtDatabase = new MgtDatabase();
        private readonly Page _page;

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

                    // Calculamos las horas jugadas totales y juegos
                    CalcularTotalHorasJugadas();
                    CalcularCantidadDeJuegos();
                }
            }
        }

        private int _horasJugadasTotales;
        public int HorasJugadasTotales
        {
            get { return _horasJugadasTotales; }
            set
            {
                _horasJugadasTotales = value;
                OnPropertyChanged(nameof(HorasJugadasTotales));
            }
        }

        private int _totalJuegos;
        public int TotalJuegos
        {
            get { return _totalJuegos; }
            set
            {
                _totalJuegos = value;
                OnPropertyChanged(nameof(TotalJuegos));
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

        public ICommand ResetPasswordCommand { get; }
        public ICommand ChangeNicknameCommand { get; }
        public ICommand ChangeNombreCommand { get; }
        public ICommand ChangeProfileImageCommand { get; }

        // MENU //

        public ICommand NavegarPrincipalCommand { get; }
        public ICommand NavegarSaldoCommand { get; }
        public ICommand NavegarGestionUsersCommand { get; }
        public ICommand NavegarLogOutCommand { get; }

        public PerfilViewModel(Page page)
        {
            _page = page;

            ResetPasswordCommand = new Command(ChangePassword);
            ChangeNicknameCommand = new Command(ChangeNickname);
            ChangeNombreCommand = new Command(ChangeNombre);
            ChangeProfileImageCommand = new Command<string>(UpdateProfileImage);


            // MENU //
            NavegarPrincipalCommand = new Command(async () => await NavegarID("PaginaPrincipal"));
            NavegarSaldoCommand = new Command(async () => await NavegarID("Cartera"));
            NavegarGestionUsersCommand = new Command(async () => await NavegarID("GestionUsuarios"));
            NavegarLogOutCommand = new Command(async () => await NavegarA("LoginPage"));
        }

        private async Task NavegarID(string pagina)
        {
            await Shell.Current.GoToAsync($"/{pagina}?usuario={UsuarioID}");
        }

        private async Task NavegarA(string pagina)
        {
            await Shell.Current.GoToAsync($"/{pagina}");
        }

        public PerfilViewModel()
        {

        }

        private void CargarUsuario()
        {
            if (int.TryParse(_usuarioID, out int id))
            {
                Usuario = _mgtDatabase.ObtenerUsuarioPorId(id);
            }
        }

        private void CalcularTotalHorasJugadas()
        {
            HorasJugadasTotales = Usuario?.ListaJuegos?.Sum(juego => juego.HorasJugadas) ?? 0;
        }

        private void CalcularCantidadDeJuegos()
        {
            TotalJuegos = Usuario?.ListaJuegos?.Count ?? 0;
        }

        private async void OnChangeProfileImage_Tapped(object sender, TappedEventArgs e)
        {
            var popup = new ImageSelectionPopup();
            popup.ImageSelected += (selectedImage) =>
            {
                UpdateProfileImage(selectedImage); // Guarda la imagen seleccionada
            };
            _page.ShowPopup(popup); // Muestra el popup
        }

        private async void OnChangeNickname_Tapped(object sender, TappedEventArgs e)
        {
            string nuevoNickname = await _page.DisplayPromptAsync("Editar Nickname", "Introduce tu nuevo nickname:", "OK",
                                                  "Cancelar", "Nuevo nickname", maxLength: 20, keyboard: Keyboard.Text);

            // Si el usuario presiona OK y no deja vac�o el campo
            if (!string.IsNullOrWhiteSpace(nuevoNickname))
            {
                Usuario.Nickname = nuevoNickname;
                OnPropertyChanged(nameof(Usuario.Nickname));

                // Actualizar en la base de datos (si es necesario)
                _mgtDatabase.ActualizarNicknameUsuario(Usuario.Id, nuevoNickname);
            }
        }

        private async void ChangePassword()
        {
            string nuevaPassword = await Application.Current.MainPage.DisplayPromptAsync(
                "Cambiar Contraseña", "Introduce tu nueva contraseña:", "OK", "Cancelar", "Nueva contraseña", maxLength: 20, keyboard: Keyboard.Text);

            if (!string.IsNullOrWhiteSpace(nuevaPassword))
            {
                Usuario.Password = nuevaPassword;
                _mgtDatabase.ActualizarPasswordUsuario(Usuario.Id, nuevaPassword);
                OnPropertyChanged(nameof(Usuario.Password));
            }
        }

        private async void ChangeNickname()
        {
            string nuevoNickname = await Application.Current.MainPage.DisplayPromptAsync(
                "Editar Nickname", "Introduce tu nuevo nickname:", "OK", "Cancelar", "Nuevo nickname", maxLength: 20, keyboard: Keyboard.Text);

            if (!string.IsNullOrWhiteSpace(nuevoNickname))
            {
                Usuario.Nickname = nuevoNickname;
                _mgtDatabase.ActualizarNicknameUsuario(Usuario.Id, nuevoNickname);
                OnPropertyChanged(nameof(Usuario.Nickname));
            }
        }

        private async void ChangeNombre()
        {
            string nuevoNombre = await Application.Current.MainPage.DisplayPromptAsync(
                "Editar Nombre", "Introduce tu nuevo nombre:", "OK", "Cancelar", "Nuevo nombre", maxLength: 20, keyboard: Keyboard.Text);

            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                Usuario.Nombre = nuevoNombre;
                _mgtDatabase.ActualizarUsernameUsuario(Usuario.Id, nuevoNombre);
                OnPropertyChanged(nameof(Usuario.Nombre));
            }
        }

        public void UpdateProfileImage(string imagePath)
        {
            Usuario.Imagen = imagePath;
            _mgtDatabase.ActualizarImagenUsuario(Usuario.Id, imagePath);
            OnPropertyChanged(nameof(Usuario.Imagen));
        }

        // On Property Changed //
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
