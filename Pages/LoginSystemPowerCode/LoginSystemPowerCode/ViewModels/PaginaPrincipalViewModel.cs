using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private readonly List<string> _imagenes = new()
        {
            "lol.jpg",
            "marvelrivals.jpg",
            "osu.jpg"
        };

        private readonly List<string> _titulos = new()
        {
            "League of Legends",
            "Marvel Rivals",
            "Osu"
        };

        private readonly List<string> _descripcion = new()
        {
            "League of Legends es un videojuego multijugador de arena de batalla.",
            "Marvel Rivals es un videojuego de acción de disparos de héroes en tercera persona.",
            "Osu es un juego fuertemente orientado a la comunidad, con todos los mapas de ritmos y canciones jugables."
        };

        private readonly List<string> _precios = new()
        {
            "20€",
            "40€",
            "5€"
        };

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
            }
        }

        // MENU //

        public ICommand NavegarPerfilCommand { get; }
        public ICommand ViajarCarteraCommand { get; }
        public ICommand ViajarSalidaCommand { get;  }
        public ICommand ViajarGestionUsuariosCommand {  get; }

        public PaginaPrincipalViewModel()
        {
            // Inicializar valores
            ActualizarDatos();

            NavegarPerfilCommand = new Command(viajarPerfil);
            ViajarCarteraCommand = new Command(viajarCartera);
            ViajarSalidaCommand = new Command(viajarSalida);
            ViajarGestionUsuariosCommand = new Command(viajarGestionUsuarios);



            // Configurar el temporizador para cambiar la imagen cada 10 segundos
            _timer = new Timer(10000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private async void viajarPerfil()
        {
            await Shell.Current.GoToAsync($"/Perfil?usuario={UsuarioID}");
        }
        private async void viajarCartera()
        {
            await Shell.Current.GoToAsync($"/Cartera?usuario={UsuarioID}");
        }
        private async void viajarSalida()
        {
            await Shell.Current.GoToAsync($"/LoginPage?");
        }
        private async void viajarGestionUsuarios()
        {
            await Shell.Current.GoToAsync($"/GestionUsuarios?usuario={UsuarioID}");
        }
       




        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _imagenActual = (_imagenActual + 1) % _imagenes.Count;

            MainThread.BeginInvokeOnMainThread(ActualizarDatos);
        }

        private void ActualizarDatos()
        {
            Imagen = _imagenes[_imagenActual];
            Titulo = _titulos[_imagenActual];
            DescripcionTexto = _descripcion[_imagenActual];
            Precio = _precios[_imagenActual];
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
