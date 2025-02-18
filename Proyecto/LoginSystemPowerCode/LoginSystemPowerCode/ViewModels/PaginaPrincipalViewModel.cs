using LoginSystemPowerCode.Models;
using LoginSystemPowerCode.Systems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Timer = System.Timers.Timer;


namespace LoginSystemPowerCode.ViewModels
{
    public class PaginaPrincipalViewModel : INotifyPropertyChanged
    {
        private Timer _timer;
        private int _imagenActual = 0;

        private MgtDatabase _mgtDatabase = new MgtDatabase();

        private List<int> _ids = new();
        private List<string> _imagenes = new();
        private List<string> _titulos = new();
        private List<string> _descripciones = new();
        private List<string> _precios = new();

        // Propiedades Bindables
        private string _imagen;
        public string Imagen
        {
            get => _imagen;
            set
            {
                _imagen = value;
                OnPropertyChanged(nameof(Imagen));
            }
        }

        private string _titulo;
        public string Titulo
        {
            get => _titulo;
            set
            {
                _titulo = value;
                OnPropertyChanged(nameof(Titulo));
            }
        }

        private string _descripcionTexto;
        public string DescripcionTexto
        {
            get => _descripcionTexto;
            set
            {
                _descripcionTexto = value;
                OnPropertyChanged(nameof(DescripcionTexto));
            }
        }

        private int _juegoId;
        public int JuegoId
        {
            get => _juegoId;
            set
            {
                _juegoId = value;
                OnPropertyChanged(nameof(JuegoId));
            }
        }

        private string _precio;
        public string Precio
        {
            get => _precio;
            set
            {
                _precio = value;
                OnPropertyChanged(nameof(Precio));
            }
        }

        private string _usuarioID;
        public string UsuarioID
        {
            get { return _usuarioID; }
            set
            {
                _usuarioID = value;
                Usuario = _mgtDatabase.ObtenerUsuarioPorId(int.Parse(value));
            }
        }

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

        public ICommand ComprarJuegoCommand { get; }

        // MENU //
        public ICommand NavegarPerfilCommand { get; }
        public ICommand ViajarCarteraCommand { get; }
        public ICommand ViajarSalidaCommand { get;  }
        public ICommand ViajarGestionUsuariosCommand {  get; }

        public PaginaPrincipalViewModel()
        {
            // Inicializar valores
            ActualizarDatos();

            ComprarJuegoCommand = new Command<int>(async (juegoId) => await ComprarJuego(juegoId));

            // MENU //
            NavegarPerfilCommand = new Command(async () => await NavegarID("Perfil"));
            ViajarCarteraCommand = new Command(async () => await NavegarID("Cartera"));
            ViajarGestionUsuariosCommand = new Command(async () => await NavegarID("GestionUsuarios"));
            ViajarSalidaCommand = new Command(async () => await NavegarA("LoginPage"));

            CargarJuegosDesdeBD();

            // Configurar el temporizador para cambiar la imagen cada 10 segundos
            _timer = new Timer(10000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private async Task ComprarJuego(int juegoId)
        {
            Debug.WriteLine("Juego seleccionado (ID): " + juegoId);

            // Primero, verificamos si el usuario ya posee el juego
            bool tieneJuego = await _mgtDatabase.VerificarSiTieneJuego(Usuario.Id, juegoId);
            if (tieneJuego)
            {
                await Application.Current.MainPage.DisplayAlert("¡Ya tienes este juego!",
                                                                "Este juego ya está en tu biblioteca.",
                                                                "OK");
                return;
            }

            // Obtenemos el juego para conocer su precio
            Juego juego = _mgtDatabase.ObtenerJuegoPorId(juegoId);
            if (juego == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                                                                "El juego no existe.",
                                                                "OK");
                return;
            }

            // Comprobamos que el usuario tenga saldo suficiente
            if (Usuario.Saldo < juego.Precio)
            {
                await Application.Current.MainPage.DisplayAlert("Saldo insuficiente",
                                                                "No tienes suficientes fondos para comprar este juego.",
                                                                "OK");
                return;
            }

            // Si el usuario tiene fondos, descontamos el precio del saldo
            Usuario.Saldo -= juego.Precio;
            _mgtDatabase.ActualizarSaldoUsuario(Usuario.Id, Usuario.Saldo);

            // Agregamos el juego a la biblioteca del usuario
            await _mgtDatabase.AgregarJuegoAUsuarioAsync(Usuario.Id, juegoId);
            await Application.Current.MainPage.DisplayAlert("¡Juego añadido!",
                                                            "El juego se ha añadido a tu biblioteca.",
                                                            "OK");
        }



        private async void CargarJuegosDesdeBD()
        {
            List<Juego> juegos = _mgtDatabase.ObtenerTodosLosJuegos();

            if (juegos.Any())
            {
                _ids = juegos.Select(j => j.Id).ToList();
                _imagenes = juegos.Select(j => j.Imagen).ToList();
                _titulos = juegos.Select(j => j.Nombre).ToList();
                _descripciones = juegos.Select(j => j.Descripcion).ToList();
                _precios = juegos.Select(j => $"{j.Precio}€").ToList();

                _imagenActual = 0;
                ActualizarDatos();
            }
        }

        private async Task NavegarID(string pagina)
        {
            await Shell.Current.GoToAsync($"/{pagina}?usuario={UsuarioID}");
        }

        private async Task NavegarA(string pagina)
        {
            await Shell.Current.GoToAsync($"/{pagina}");
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _imagenActual = (_imagenActual + 1) % _imagenes.Count;

            MainThread.BeginInvokeOnMainThread(ActualizarDatos);
        }

        private void ActualizarDatos()
        {
            if (_imagenes.Count == 0) return;

            Imagen = _imagenes[_imagenActual];
            Titulo = _titulos[_imagenActual];
            DescripcionTexto = _descripciones[_imagenActual];
            Precio = _precios[_imagenActual];
            JuegoId = _ids[_imagenActual];
        }

        public void DetenerTemporizador()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
